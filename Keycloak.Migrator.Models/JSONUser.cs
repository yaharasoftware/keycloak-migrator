using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Migrator.Models
{
    /// <summary>
    /// Represents a User from the JSON Migration.
    /// </summary>
    public class JSONUser
    {
        /// <summary>
        /// Gets or sets the User name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("user_name")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the First name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("first_name")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Last name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("last_name")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        /// <value>
        /// The Email.
        /// </value>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Realm Roles.
        /// </summary>
        /// <value>
        /// The Roles.
        /// </value>
        [JsonProperty("realm_roles")]
        public List<string> RealmRoles { get; set; } = new List<string>();

        /// <summary>
        /// Sets Client Roles
        /// </summary>
        [JsonProperty("client_roles")]
        public Dictionary<string, List<string>> ClientRoles { get; set; } = new Dictionary<string, List<string>>();

        /// <summary>
        /// Password to set for the user
        /// </summary>
        public string Password { get; set; } = string.Empty;

        [JsonProperty("temporary_password")]
        public bool TemporaryPassword { get; set; } = true;
    }
}
