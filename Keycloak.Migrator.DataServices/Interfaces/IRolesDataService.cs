using Keycloak.Net.Models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    /// <summary>
    /// Roles Data Service
    /// </summary>
    public interface IRolesDataService
    {
        /// <summary>
        /// Deletes the composite roles.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="clientIdentifier">The client identifier.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="roles">The roles.</param>
        /// <returns></returns>
        Task<bool> DeleteCompositeRoles(string realm, string clientIdentifier, string roleName, Role[] roles);

        /// <summary>
        /// Adds the composite roles.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="clientIdentifier">The client identifier.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="roles">The roles.</param>
        /// <returns></returns>
        Task<bool> AddCompositeRoles(string realm, string clientIdentifier, string roleName, Role[] roles);

        /// <summary>
        /// Gets the composite roles.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="clientIdentifier">The client identifier.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        Task<IEnumerable<Role>> GetCompositeRoles(string realm, string clientIdentifier, string roleName);

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <returns></returns>
        public Task<IEnumerable<Role>> GetRoles(string realm);

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="clientIdentifier">The client identifier.</param>
        /// <returns></returns>
        public Task<IEnumerable<Role>> GetRoles(string realm, string clientIdentifier);

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Task<bool> UpdateRole(string realm, Role role);

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Task<bool> DeleteRole(string realm, Role role);

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Task<bool> AddRole(string realm, string clientId, Role role);

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Task<bool> AddRole(string realm, Role role);
    }
}
