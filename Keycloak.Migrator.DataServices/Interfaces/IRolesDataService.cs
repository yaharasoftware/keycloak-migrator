using Keycloak.Net.Models.Roles;
using System;
using System.Linq;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    public interface IRolesDataService
    {
        public Task<IEnumerable<Role>> GetRoles(string realm, string clientId);
        public Task<bool> UpdateRole(string realm, Role role);
        public Task<bool> DeleteRole(string realm, Role role);
        public Task<bool> AddRole(string realm, string clientId, Role role);
    }
}
