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
    public class JSONUserRoleAdditions
    {
        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        /// <value>
        /// The name.
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
