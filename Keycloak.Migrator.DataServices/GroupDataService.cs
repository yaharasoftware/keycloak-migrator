using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Net;
using Keycloak.Net.Models.Groups;
using Keycloak.Net.Models.Roles;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices
{
    public class GroupDataService : IGroupDataService
    {
        private readonly ILogger<GroupDataService> _logger;
        private readonly KeycloakClient _keycloakClient;

        public GroupDataService(KeycloakClient keycloakClient,
            ILogger<GroupDataService> logger)

        {
            _keycloakClient = keycloakClient;
            _logger = logger;
        }

        /// <summary>
        /// Adds the group.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        public async Task<bool> AddGroup(string realm, Group group)
        {
            return await _keycloakClient.CreateGroupAsync(realm, group);
        }

        /// <summary>
        /// Deletes a group.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        public async Task<bool> DeleteGroup(string realm, Group group)
        {
            return await _keycloakClient.DeleteGroupAsync(realm, group.Id);
        }

        /// <summary>
        /// Gets the groups.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Group>> GetGroups(string realm)
        {
            return await _keycloakClient.GetGroupHierarchyAsync(realm);
        }

        /// <summary>
        /// Gets the groups for a realm.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        public async Task<Group> GetGroup(string realm, string groupId)
        {
            return await _keycloakClient.GetGroupAsync(realm, groupId);
        }

        /// <summary>
        /// Updates the group.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        public async Task<bool> UpdateGroup(string realm, Group group)
        {
            return await _keycloakClient.UpdateGroupAsync(realm, group.Id, group);
        }

        /// <summary>
        /// Gets the group roles.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="clientIdentifier">The client identifier.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Role>> GetGroupRoles(string realm, string clientIdentifier, string groupId)
        {
            return await _keycloakClient.GetClientRoleMappingsForGroupAsync(realm, groupId, clientIdentifier);
        }

        /// <summary>
        /// Adds the group roles.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="clientIdentifier">The client identifier.</param>
        /// <param name="roles">The roles.</param>
        /// <returns></returns>
        public async Task<bool> AddGroupRoles(string realm, string groupId, string clientIdentifier, IEnumerable<Role> roles)
        {
            return await _keycloakClient.AddClientRoleMappingsToGroupAsync(realm, groupId, clientIdentifier, roles);
        }

        /// <summary>
        /// Deletes roles from a group.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="clientIdentifier">The client identifier.</param>
        /// <param name="roles">The roles.</param>
        /// <returns></returns>
        public async Task<bool> DeleteGroupRoles(string realm, string groupId, string clientIdentifier, IEnumerable<Role> roles)
        {
            return await _keycloakClient.DeleteClientRoleMappingsFromGroupAsync(realm, groupId, clientIdentifier, roles);
        }
    }
}
