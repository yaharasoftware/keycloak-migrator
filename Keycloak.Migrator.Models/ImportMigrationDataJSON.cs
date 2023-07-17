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
        [JsonProperty("realm_roles")]
        public List<JSONRole> RealmRoles { get; set; } = new List<JSONRole>();
        [JsonProperty("client_roles")]
        public Dictionary<string, List<JSONRole>> ClientRoles { get; set; } = new Dictionary<string, List<JSONRole>>();
        public List<JSONUser> Users { get; set; } = new List<JSONUser>();
        [JsonProperty("user_realm_role_additions")]
        public List<JSONUserRealmRoleAdditions> UserRealmRoleAdditions { get; set; } = new List<JSONUserRealmRoleAdditions>();

        [JsonProperty("user_client_role_additions")]
        public List<JSONUserClientRoleAdditions> UserClientRoleAdditions { get; set; } = new List<JSONUserClientRoleAdditions>();
    }
}
