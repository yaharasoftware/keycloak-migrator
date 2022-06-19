using Keycloak.Net.Models.Roles;
using System;
using System.Linq;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    public interface IRolesDataService
    {
        Task<bool> DeleteCompositeRoles(string realm, string clientId, string roleName, Role[] roles);
        Task<bool> AddCompositeRoles(string realm, string clientId, string roleName, Role[] roles);
        Task<IEnumerable<Role>> GetCompositeRoles(string realm, string clientId, string roleName);
        public Task<IEnumerable<Role>> GetRoles(string realm, string clientId);
        public Task<bool> UpdateRole(string realm, Role role);
        public Task<bool> DeleteRole(string realm, Role role);
        public Task<bool> AddRole(string realm, string clientId, Role role);
    }
}
