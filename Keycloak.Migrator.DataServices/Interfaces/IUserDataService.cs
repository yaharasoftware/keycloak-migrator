using Keycloak.Net.Models.Roles;
using Keycloak.Net.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    /// <summary>
    /// User Data Service
    /// </summary>
    public interface IUserDataService
    {
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <returns></returns>
        public Task<IEnumerable<User>> GetUsers(string realm);

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<bool> UpdateUser(string realm, User user);

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<bool> DeleteUser(string realm, User user);

        /// <summary>
        /// Adds the User.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<bool> AddUser(string realm, User user);

        /// <summary>
        /// Add Realm Roles to User
        /// </summary>
        /// <param name="realm"></param>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public Task<bool> UpdateUserRealmRoles(string realm, User user, IEnumerable<Role> roles);

        /// <summary>
        /// Add Client Roles to User
        /// </summary>
        /// <param name="realm"></param>
        /// <param name="user"></param>
        /// <param name="clientId"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public Task<bool> UpdateUserClientRoles(string realm, User user, string clientId, IEnumerable<Role> roles);

        /// <summary>
        /// Set the user password - temporary or not
        /// </summary>
        /// <param name="realm"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="temporary"></param>
        /// <returns></returns>
        public Task<bool> SetUserPassword(string realm, User user, string password, bool temporary = true);
    }
}
