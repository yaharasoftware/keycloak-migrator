using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Migrator.Models
{
    public class KeycloakCredentials
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public Uri? Url { get; set; }
    }
}
