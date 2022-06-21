using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Net;
using Keycloak.Net.Models.Roles;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Role>> GetCompositeRoles(string realm, string clientId, string roleName)
        {
            return await _keycloakClient.GetRoleCompositesAsync(realm, clientId, roleName); ;
        }

        public async Task<bool> AddCompositeRoles(string realm, string clientId, string roleName, params Role[] roles)
        {
            Role role = await _keycloakClient.GetRoleByNameAsync(realm, clientId, roleName);


            if (!(role.Composite ?? false))
            {
                return await _keycloakClient.MakeRoleCompositeAsync(realm, role.Id, roles);
            }
            else
            {
                return await _keycloakClient.AddCompositesToRoleAsync(realm, clientId, roleName, roles);
            }
        }


        public async Task<bool> DeleteCompositeRoles(string realm, string clientId, string roleName, params Role[] roles)
        {
            return await _keycloakClient.RemoveCompositesFromRoleAsync(realm, clientId, roleName, roles);
        }
    }
}
