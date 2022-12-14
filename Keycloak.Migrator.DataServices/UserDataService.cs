using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Net;
using Keycloak.Net.Models.Roles;
using Keycloak.Net.Models.Users;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices
{
    public class UserDataService : IUserDataService
    {
        private readonly KeycloakClient _keycloakClient;
        private readonly ILogger<RolesDataService> _logger;
        public UserDataService(KeycloakClient keycloakClient,
            ILogger<RolesDataService> logger)
        {
            _logger = logger;
            _keycloakClient = keycloakClient;
        }

        public async Task<bool> AddUser(string realm, User user)
        {
            return await _keycloakClient.CreateUserAsync(realm, user);
        }

        public async Task<bool> DeleteUser(string realm, User user)
        {
            return await _keycloakClient.DeleteUserAsync(realm, user.Id);
        }

        public async Task<IEnumerable<User>> GetUsers(string realm)
        {
            return await _keycloakClient.GetUsersAsync(realm);
        }

        public async Task<bool> UpdateUser(string realm, User user)
        {
            return await _keycloakClient.UpdateUserAsync(realm, user.Id, user);
        }

        public async Task<bool> UpdateUserRoles(string realm, User user, IEnumerable<Role> roles)
        {
            return await _keycloakClient.AddRealmRoleMappingsToUserAsync(realm, user.Id, roles);
        }

        public async Task<bool> SetUserPassword(string realm, User user, string password, bool temporary = true)
        {
            return await _keycloakClient.ResetUserPasswordAsync(realm, user.Id, password, temporary);
        }
    }
}
