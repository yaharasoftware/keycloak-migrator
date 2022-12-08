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
        /// Add Roles to User
        /// </summary>
        /// <param name="realm"></param>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public Task<bool> UpdateUserRoles(string realm, User user, IEnumerable<Role> roles);
    }
}
