using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Migrator.Models
{
    /// <summary>
    /// Represents a user within keycloak.
    /// </summary>
    public class User
    {
        //public UserAccess Access { get; set; }

        /// <summary>
        /// Gets or sets the Attributes
        /// </summary>
        public Dictionary<string, IEnumerable<string>> Attributes { get; set; } = new Dictionary<string, IEnumerable<string>>();

        //public IEnumerable<UserConsent> ClientConsents { get; set; }

        /// <summary>
        /// Gets or sets the Client Roles
        /// </summary>
        //public Dictionary<string, Role> ClientRoles { get; set; } = new Dictionary<string, Role>();

        /// <summary>
        /// Gets or sets the CreatedTimestamp
        /// </summary>
        public long CreatedTimestamp { get; set; }

        //public IEnumerable<Credentials> Credentials { get; set; }

        //public ReadOnlyCollection<string> DisableableCredentialTypes { get; set; }

        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the EmailVerified
        /// </summary>
        public bool? EmailVerified { get; set; }

        /// <summary>
        /// Gets or sets the Enabled Property
        /// </summary>
        public bool? Enabled { get; set; }

        //public IEnumerable<FederatedIdentity> FederatedIdentities { get; set; }

        /// <summary>
        /// Gets or sets the FederationLink
        /// </summary>
        public string FederationLink { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the FirstName
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Groups
        /// </summary>
        public IEnumerable<string> Groups { get; set; } = Enumerable.Empty<string>();

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the LastName
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the NotBefore
        /// </summary>
        public int? NotBefore { get; set; }

        /// <summary>
        /// Gets or sets the Origin
        /// </summary>
        public string Origin { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the RealmRoles
        /// </summary>
        public IEnumerable<string> RealmRoles { get; set; } = Enumerable.Empty<string>();

        //public ReadOnlyCollection<string> RequiredActions { get; set; }

        /// <summary>
        /// Gets or sets the Self
        /// </summary>
        public string Self { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ServiceAccountClientId
        /// </summary>
        public string ServiceAccountClientId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Totp
        /// </summary>
        public bool? Totp { get; set; }

        /// <summary>
        /// Gets or sets the Username
        /// </summary>
        public string UserName { get; set; } = string.Empty;
    }
}
