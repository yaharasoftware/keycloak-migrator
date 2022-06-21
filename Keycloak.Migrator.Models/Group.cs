using System;
using System.Collections.Generic;
using System.Linq;

namespace Keycloak.Migrator.Models
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public IList<string> RealmRoles { get; set; } = new List<string>();
        public IDictionary<string, IList<string>> ClientRoles { get; set; } = new Dictionary<string, IList<string>>();
        public IList<string> SubGroups { get; set; } = new List<string>();

    }
}
