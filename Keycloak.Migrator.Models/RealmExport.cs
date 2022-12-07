using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Migrator.Models
{
    public class RealmExport
    {
        public string? Id { get; set; }
        public string? Realm { get; set; }
        public RoleCollection<List<Role>> Roles { get; set; } = new RoleCollection<List<Role>>();
        public IList<Group> Groups { get; set; } = new List<Group>();
        public IList<User> Users { get; set; } = new List<User>();
    }
}
