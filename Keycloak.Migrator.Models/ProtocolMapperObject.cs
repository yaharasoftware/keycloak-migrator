using System;
using System.Linq;

namespace Keycloak.Migrator.Models
{
    public class ProtocolMapperObject
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Protocol { get; set; }
        public string? ProtocolMapper { get; set; }
        public bool ConsentRequired { get; set; }
        public Config Config { get; set; } = new Config();
    }
}
