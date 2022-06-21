using Keycloak.Net.Models.Groups;
using Keycloak.Net.Models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    /// <summary>
    /// Group Data Service
    /// </summary>
    public interface IGroupDataService
    {
        /// <summary>
        /// Gets the group roles.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="clientIdentifier">The client identifier.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<Role>> GetGroupRoles(string realm, string clientIdentifier, string groupId);

        /// <summary>
        /// Adds the group.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        public Task<bool> AddGroup(string realm, Group group);

        /// <summary>
        /// Adds the group roles.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="clientIdentifier">The client identifier.</param>
        /// <param name="roles">The roles.</param>
        /// <returns></returns>
        Task<bool> AddGroupRoles(string realm, string groupId, string clientIdentifier, IEnumerable<Role> roles);

        /// <summary>
        /// Deletes a group.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        public Task<bool> DeleteGroup(string realm, Group group);

        /// <summary>
        /// Deletes roles from a group.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="clientIdentifier">The client identifier.</param>
        /// <param name="roles">The roles.</param>
        /// <returns></returns>
        Task<bool> DeleteGroupRoles(string realm, string groupId, string clientIdentifier, IEnumerable<Role> roles);

        /// <summary>
        /// Gets the groups for a realm.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        Task<Group> GetGroup(string realm, string groupId);

        /// <summary>
        /// Gets the groups.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <returns></returns>
        public Task<IEnumerable<Group>> GetGroups(string realm);

        /// <summary>
        /// Updates the group.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        public Task<bool> UpdateGroup(string realm, Group group);
    }
}
