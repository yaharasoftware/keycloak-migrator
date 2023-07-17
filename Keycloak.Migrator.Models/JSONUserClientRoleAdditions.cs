using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Migrator.Models
{
    /// <summary>
    /// Represents a UserRoleAddition from the JSON Migration.
    /// </summary>
    public class JSONUserClientRoleAdditions
    {
        /// <summary>
        /// Gets or sets the UserName
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("user_name")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Client Name for the roles being added
        /// </summary>
        public string Client { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Roles.
        /// </summary>
        /// <value>
        /// The Roles.
        /// </value>
        public List<string> Roles { get; set; } = new List<string>();
    }
}
