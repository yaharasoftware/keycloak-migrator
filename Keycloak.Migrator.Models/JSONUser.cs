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
        /// Gets or sets the Roles.
        /// </summary>
        /// <value>
        /// The Roles.
        /// </value>
        public List<string> Roles { get; set; } = new List<string>();
    }
}
