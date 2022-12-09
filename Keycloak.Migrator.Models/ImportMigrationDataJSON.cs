using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Migrator.Models
{
    public class ImportMigrationDataJSON
    {
        public string Realm { get; set; } = string.Empty;
        public List<JSONRole> Roles { get; set; } = new List<JSONRole>();
        public List<JSONUser> Users { get; set; } = new List<JSONUser>();
        [JsonProperty("user_role_additions")]
        public List<JSONUserRoleAdditions> UserRoleAdditions { get; set; } = new List<JSONUserRoleAdditions>();
    }
}
