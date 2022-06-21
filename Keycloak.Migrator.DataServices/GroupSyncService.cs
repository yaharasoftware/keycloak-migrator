using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Migrator.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices
{
    public class GroupSyncService : IGroupSyncService
    {
        private readonly IGroupDataService _groupDataService;
        private readonly ILogger<GroupSyncService> _logger;
        private readonly IClientDataService _clientDataService;
        private readonly IRolesDataService _rolesDataService;

        public GroupSyncService(IGroupDataService groupDataService,
            IRolesDataService rolesDataService,
            IClientDataService clientDataService,
            ILogger<GroupSyncService> logger)
        {
            _rolesDataService = rolesDataService;
            _clientDataService = clientDataService;
            _groupDataService = groupDataService;
            _logger = logger;

        }

        public async Task<bool> SyncGroups(RealmExport realmExport, string clientId)
        {
            if (realmExport is null)
            {
                throw new ArgumentNullException(nameof(realmExport));
            }

            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException($"'{nameof(clientId)}' cannot be null or empty.", nameof(clientId));

            }

            if (realmExport.Id is null)
            {
                _logger.LogError("RealmExport.Id is null");
                return false;
            }

            string? clientIdentifier = await this.GetClientId(realmExport.Id, clientId);


            if (clientIdentifier is null)
            {
                _logger.LogError($"Unable to locate identifier for client '{clientId}' and realm '{realmExport.Id}'");
                return false;
            }

            List<Group> realmExportGroups = realmExport.Groups.ToList();

            IEnumerable<Net.Models.Groups.Group> keycloakGroups = await _groupDataService.GetGroups(realmExport.Id);

            // Log the groups found in the system.
            _logger.LogInformation($"{keycloakGroups.Count()} groups found in keycloak.  {realmExportGroups.Count()} groups found in export.");

            // Remove groups that are no longer in the export if not associated with other realms or clients.
            await this.DeleteMissingGroups(realmExport.Id, clientId, clientIdentifier, keycloakGroups, realmExportGroups);

            // Add new groups that are defined in the export.
            await this.AddMissingGroups(realmExport.Id, clientId, clientIdentifier, keycloakGroups, realmExportGroups);

            // Retrieve the groups again.
            keycloakGroups = await _groupDataService.GetGroups(realmExport.Id);

            //Update group path if needed.
            await this.UpdateGroups(realmExport.Id, clientIdentifier, keycloakGroups, realmExportGroups);

            //The group roles.
            await this.UpdateGroupRoles(realmExport.Id, clientIdentifier, clientId, realmExportGroups, keycloakGroups);

            return true;
        }

        private async Task<string?> GetClientId(string realm, string clientId)
        {
            Net.Models.Clients.Client? client = await _clientDataService.GetClient(realm, clientId);
            return client?.Id;
        }

        private async Task AddMissingGroups(string realm, string clientId, string clientIdentifier, IEnumerable<Net.Models.Groups.Group> keycloakGroups, List<Group> realmExportGroups)
        {
            _logger.LogInformation("Adding missing groups...");

            List<Group> newGroups = realmExportGroups.Where(er => !keycloakGroups.Any(kr => kr.Name == er.Name)).ToList();

            foreach (Group group in newGroups)
            {
                _logger.LogInformation($"Adding role '{group.Name}' to realm '{realm}'");

                Net.Models.Groups.Group newGroup = new Net.Models.Groups.Group
                {
                    Name = group.Name,
                    Path = group.Path,
                };

                await _groupDataService.AddGroup(realm, newGroup);
            }

            if (newGroups.Count == 0)
            {
                _logger.LogInformation($"No new groups to add to keycloak.");
            }
        }

        private async Task DeleteMissingGroups(string realm, string clientId, string clientIdentifier, IEnumerable<Net.Models.Groups.Group> keycloakGroups, List<Group> realmExportGroup)
        {
            _logger.LogInformation("Deleting groups that have been removed...");

            List<Net.Models.Groups.Group> groupsToDelete = keycloakGroups.Where(kg => !realmExportGroup.Any(eg => eg.Name == kg.Name)).ToList();

            foreach (Net.Models.Groups.Group group in groupsToDelete)
            {
                if (group.RealmRoles.Count() == 0
                    && group.ClientRoles.Keys.Count(k => k != clientId) == 0)
                {
                    _logger.LogInformation("Deleting group '{groupName}'.", group.Name);
                    await _groupDataService.DeleteGroup(realm, group);
                }
                else
                {
                    _logger.LogInformation("The group '{groupName}' is associated with roles outside of the client.  Removing client roles.", group.Name);
                    await this.ReconcileGroupRoles(realm, clientIdentifier, group, Array.Empty<string>(), Array.Empty<Net.Models.Roles.Role>());
                }

            }

            if (groupsToDelete.Count == 0)
            {
                _logger.LogInformation("No groups to delete...");
            }
        }

        private async Task ReconcileGroupRoles(string realm, string clientIdentifier, Net.Models.Groups.Group group, string[] roleList, IEnumerable<Net.Models.Roles.Role> keycloakRoles)
        {
            IEnumerable<Net.Models.Roles.Role> groupRoles = await _groupDataService.GetGroupRoles(realm, clientIdentifier, group.Id);

            List<Net.Models.Roles.Role> rolesToRemove = groupRoles.Where(c => !roleList.Contains(c.Name)).ToList();

            List<string> roleNamesToAdd = roleList.Where(rl => !groupRoles.Any(cr => cr.Name == rl)).ToList();

            List<Net.Models.Roles.Role> rolesToAdd = keycloakRoles.Where(kr => roleNamesToAdd.Contains(kr.Name)).ToList();

            if (rolesToAdd.Count != roleNamesToAdd.Count)
            {
                _logger.LogWarning($"The names of roles to add to group '{group.Name}' in client '{clientIdentifier}' does not match.");
            }

            if (rolesToAdd.Count != 0)
            {

                _logger.LogInformation($"Adding {rolesToAdd.Count} role(s) to group '{group.Name}' in client '{clientIdentifier}' for realm '{realm}'");

                await _groupDataService.AddGroupRoles(realm, group.Id, clientIdentifier, rolesToAdd.ToArray());
            }

            if (rolesToRemove.Count != 0)
            {

                _logger.LogInformation($"Removing {rolesToRemove.Count} role(s) from group '{group.Name}' in client '{clientIdentifier}' for realm '{realm}'");

                await _groupDataService.DeleteGroupRoles(realm, group.Id, clientIdentifier, rolesToRemove.ToArray());

            }
        }

        private async Task UpdateGroups(string realm, string clientIdentifer, IEnumerable<Net.Models.Groups.Group> keycloakGroups, List<Group> realmExportGroups)
        {
            foreach (Group realmExportGroup in realmExportGroups)
            {
                Net.Models.Groups.Group? existingGroup = keycloakGroups.Where(kr => kr.Name == realmExportGroup.Name).FirstOrDefault();

                if (existingGroup is null)
                {
                    _logger.LogWarning($"Unable to locate group '{realmExportGroup.Name}' in client {clientIdentifer} for realm {realm}.");
                    continue;
                }

                if (realmExportGroup.Path != existingGroup.Path)
                {
                    _logger.LogInformation($"Updating path for group '{existingGroup.Name}'");

                    existingGroup.Path = realmExportGroup.Path;
                    await _groupDataService.UpdateGroup(realm, existingGroup);
                }
            }
        }
        private async Task UpdateGroupRoles(string realm, string clientIdentifier, string clientId, List<Group> realmExportGroups, IEnumerable<Net.Models.Groups.Group> keycloakGroups)
        {
            IEnumerable<Net.Models.Roles.Role> roles = await _rolesDataService.GetRoles(realm, clientIdentifier);


            // Update the composite roles.
            foreach (Group group in realmExportGroups)
            {
                Net.Models.Groups.Group? kcGroup = keycloakGroups.Where(kr => kr.Name == group.Name).FirstOrDefault();
                string[] requiredRoles = Array.Empty<string>();

                if (kcGroup is null)
                {
                    _logger.LogWarning($"Unable to locate role '{group.Name}' in client '{clientIdentifier}' for realm '{realm}'.");
                    continue;
                }

                if (group.ClientRoles.ContainsKey(clientId))
                {
                    requiredRoles = group.ClientRoles[clientId].ToArray();
                }

                await ReconcileGroupRoles(realm, clientIdentifier, kcGroup, requiredRoles, roles);
            }
        }
    }
}
