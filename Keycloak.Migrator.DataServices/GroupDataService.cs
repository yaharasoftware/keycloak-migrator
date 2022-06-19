using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Net;
using Keycloak.Net.Models.Groups;
using Keycloak.Net.Models.RealmsAdmin;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

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

        public async Task<bool> AddGroup(string realm, Group group)
        {
            return await _keycloakClient.CreateGroupAsync(realm, group);
        }

        public async Task<bool> DeleteGroup(string realm, Group group)
        {
            return await _keycloakClient.DeleteGroupAsync(realm, group.Id);
        }

        public async Task<IEnumerable<Group>> GetGroups(string realm)
        {
            return await _keycloakClient.GetGroupHierarchyAsync(realm);
        }

        public async Task<Group> GetGroup(string realm, string groupId)
        {
            return await _keycloakClient.GetGroupAsync(realm, groupId);
        }

        public async Task<bool> UpdateGroup(string realm, Group group)
        {
            return await _keycloakClient.UpdateGroupAsync(realm, group.Id, group);
        }
    }
}
