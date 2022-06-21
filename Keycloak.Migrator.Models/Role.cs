using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Migrator.Models
{
    /// <summary>
    /// Represents a role within keycloak.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// Gets or sets a value indicating whether [client role].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [client role]; otherwise, <c>false</c>.
        /// </value>
        public bool ClientRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Role"/> is composite.
        /// </summary>
        /// <value>
        ///   <c>true</c> if composite; otherwise, <c>false</c>.
        /// </value>
        public bool Composite { get; set; }

        /// <summary>
        /// Gets or sets the composites.
        /// </summary>
        /// <value>
        /// The composites.
        /// </value>
        public RoleCollection<List<string>> Composites { get; set; } = new RoleCollection<List<string>>();
        /// <summary>
        /// Gets or sets the container identifier.
        /// </summary>
        /// <value>
        /// The container identifier.
        /// </value>
        public string? ContainerId { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string? Description { get; set; }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = string.Empty;
    }
}
