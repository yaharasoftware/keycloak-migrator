using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Net;
using Keycloak.Net.Models.Clients;
using Keycloak.Net.Models.Roles;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices
{
    public class RolesDataService : IRolesDataService
    {
        private readonly KeycloakClient _keycloakClient;
        private readonly ILogger<RolesDataService> _logger;
        public RolesDataService(KeycloakClient keycloakClient,
            ILogger<RolesDataService> logger)
        {
            _logger = logger;
            _keycloakClient = keycloakClient;
        }

        public async Task<bool> AddRole(string realm, string clientId, Role role)
        {
            return await _keycloakClient.CreateRoleAsync(realm, clientId, role);
        }

        public async Task<bool> DeleteRole(string realm, Role role)
        {
            return await _keycloakClient.DeleteRoleByIdAsync(realm, role.Id);
        }

        public async Task<IEnumerable<Role>> GetRoles(string realm, string clientId)
        {
            IEnumerable<Role> roles = await _keycloakClient.GetRolesAsync(realm, clientId);
            return roles;
        }

        public async Task<bool> UpdateRole(string realm, Role role)
        {
            return await _keycloakClient.UpdateRoleByIdAsync(realm, role.Id.ToString(), role);
        }
    }
}
