using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Migrator.Models;
using Microsoft.Extensions.Logging;

namespace Keycloak.Migrator.DataServices
{
    public class RolesSyncService : IRolesSyncService
    {
        private readonly ILogger<RolesSyncService> _logger;
        private readonly IRolesDataService _rolesDataService;
        private readonly IClientDataService _clientDataService;
        public RolesSyncService(IRolesDataService rolesDataService,
            IClientDataService clientDataService,
            ILogger<RolesSyncService> logger)
        {
            _clientDataService = clientDataService;
            _rolesDataService = rolesDataService;
            _logger = logger;
        }

        public async Task<bool> SyncRoles(RealmExport realmExport, string clientId)
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

            // Get the list of roles from the export
            List<Role> realmExportRoles = realmExport.Roles.Client.ContainsKey(clientId) ? realmExport.Roles.Client[clientId] : new List<Role>();

            // Get the list of roles from keycloak.
            IEnumerable<Net.Models.Roles.Role> keycloakRoles = await _rolesDataService.GetRoles(realmExport.Id, clientIdentifier);

            // Log the roles found in the system.
            _logger.LogInformation($"{keycloakRoles.Count()} roles found in keycloak.  {realmExportRoles.Count()} roles found in export.");

            // Delete the roles from keycloak that are not in the export.
            await this.DeleteMissingRoles(realmExport.Id, clientIdentifier, realmExportRoles, keycloakRoles);

            // Get the updated list from keycloak.
            keycloakRoles = await _rolesDataService.GetRoles(realmExport.Id, clientIdentifier);

            // Add missing roles.
            await this.AddMissingRoles(realmExport.Id, clientIdentifier, realmExportRoles, keycloakRoles);

            // Get the updated list from keycloak.
            keycloakRoles = await _rolesDataService.GetRoles(realmExport.Id, clientIdentifier);

            // Updated the description of the role within keycloak if different.
            await this.UpdateChangedRoles(realmExport.Id, clientIdentifier, realmExportRoles, keycloakRoles);

            // Get the updated list from keycloak.
            keycloakRoles = await _rolesDataService.GetRoles(realmExport.Id, clientIdentifier);

            // Update Composite Roles
            await this.UpdateCompositeRoles(realmExport.Id, clientIdentifier, clientId, realmExportRoles, keycloakRoles);


            return true;
        }

        private async Task<string?> GetClientId(string realm, string clientId)
        {
            Net.Models.Clients.Client? client = await _clientDataService.GetClient(realm, clientId);
            return client?.Id;
        }


        private async Task UpdateChangedRoles(string realm, string clientIdentifer, List<Role> realmExportRoles, IEnumerable<Net.Models.Roles.Role> keycloakRoles)
        {
            foreach (Role realmExportRole in realmExportRoles)
            {
                Net.Models.Roles.Role? existingRole = keycloakRoles.Where(kr => kr.Name == realmExportRole.Name).FirstOrDefault();

                if (existingRole is null)
                {
                    _logger.LogWarning($"Unable to locate role '{realmExportRole.Name}' in client {clientIdentifer} for realm {realm}.");
                    continue;
                }

                if (realmExportRole.Description != existingRole.Description)
                {
                    _logger.LogInformation($"Updating description for role '{existingRole.Name}'");

                    existingRole.Description = realmExportRole.Description;
                    await _rolesDataService.UpdateRole(realm, existingRole);
                }

            }
        }

        private async Task DeleteMissingRoles(string realm,
            string clientIdentifier,
            IEnumerable<Role> realmExportRoles,
            IEnumerable<Net.Models.Roles.Role> keycloakRoles)
        {
            _logger.LogInformation("Checking for roles to delete...");
            foreach (Net.Models.Roles.Role keycloakRole in keycloakRoles)
            {
                if (!realmExportRoles.Any(x => x.Name == keycloakRole.Name))
                {
                    _logger.LogInformation($"Deleting role '{keycloakRole.Name}' from client '{clientIdentifier}' in realm '{realm}'");
                    await _rolesDataService.DeleteRole(realm, keycloakRole);
                }
            }
        }

        private async Task AddMissingRoles(string realm,
            string clientIdentifier,
            IEnumerable<Role> exportRoles,
            IEnumerable<Net.Models.Roles.Role> keycloakRoles)
        {
            _logger.LogInformation($"Adding missing roles...");

            List<Role> newRoles = exportRoles.Where(er => !keycloakRoles.Any(kr => kr.Name == er.Name)).ToList();

            foreach (Role role in newRoles)
            {
                _logger.LogInformation($"Adding role '{role.Name}' to client '{clientIdentifier}' in realm '{realm}'");
                Net.Models.Roles.Role newRole = new Net.Models.Roles.Role
                {
                    Name = role.Name,
                    Description = role.Description,
                };

                await _rolesDataService.AddRole(realm, clientIdentifier, newRole);
            }

            if (newRoles.Count == 0)
            {
                _logger.LogInformation($"No new roles to add to keycloak.");
            }
        }
        private async Task UpdateCompositeRoles(string realm, string clientIdentifier, string clientId, List<Role> realmExportRoles, IEnumerable<Net.Models.Roles.Role> keycloakRoles)
        {
            List<Role> rerComposite = realmExportRoles
                .Where(er => er.Composite)
                .ToList();

            // Update the composite roles.
            foreach (Role role in rerComposite)
            {
                Net.Models.Roles.Role? kcRole = keycloakRoles.Where(kr => kr.Name == role.Name).FirstOrDefault();
                string[] requiredRoles = Array.Empty<string>();

                if (kcRole is null)
                {
                    _logger.LogWarning($"Unable to locate role '{role.Name}' in client '{clientIdentifier}' for realm '{realm}'.");
                    continue;
                }

                if (role.Composites.Client.ContainsKey(clientId))
                {
                    requiredRoles = role.Composites.Client[clientId].ToArray();
                }

                await ReconcileRoles(realm, clientIdentifier, clientId, kcRole, requiredRoles, keycloakRoles);
            }

            // Clear roles that are no longer composite for the realm.
            List<Net.Models.Roles.Role> rolesToClear = keycloakRoles.Where(kr => kr.Composite ?? false).Where(kr => !rerComposite.Any(rer => rer.Name == kr.Name)).ToList(); ;

            foreach (Net.Models.Roles.Role role in rolesToClear)
            {
                await ReconcileRoles(realm, clientIdentifier, clientId, role, Array.Empty<string>(), keycloakRoles);
            }

        }

        private async Task ReconcileRoles(string realm, string clientIdentifier, string clientId, Net.Models.Roles.Role role, string[] roleList, IEnumerable<Net.Models.Roles.Role> keycloakRoles)
        {
            IEnumerable<Net.Models.Roles.Role> compositeRoles = await _rolesDataService.GetCompositeRoles(realm, clientIdentifier, role.Name);

            List<Net.Models.Roles.Role> rolesToRemove = compositeRoles.Where(c => !roleList.Contains(c.Name)).ToList();

            List<string> roleNamesToAdd = roleList.Where(rl => !compositeRoles.Any(cr => cr.Name == rl)).ToList();

            List<Net.Models.Roles.Role> rolesToAdd = keycloakRoles.Where(kr => roleNamesToAdd.Contains(kr.Name)).ToList();

            if (rolesToAdd.Count != roleNamesToAdd.Count)
            {
                _logger.LogWarning($"The names of roles to add to role '{role.Name}' in client '{clientIdentifier}' does not match.");
            }

            if (rolesToAdd.Count != 0)
            {

                _logger.LogInformation($"Adding {rolesToAdd.Count} role(s) to '{role.Name}' in client '{clientIdentifier}' for realm '{realm}'");

                await _rolesDataService.AddCompositeRoles(realm, clientIdentifier, role.Name, rolesToAdd.ToArray());
            }

            if (rolesToRemove.Count != 0)
            {

                _logger.LogInformation($"Removing {rolesToRemove.Count} role(s) to '{role.Name}' in client '{clientIdentifier}' for realm '{realm}'");

                await _rolesDataService.DeleteCompositeRoles(realm, clientIdentifier, role.Name, rolesToRemove.ToArray());

            }
        }
    }
}
