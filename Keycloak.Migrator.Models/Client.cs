using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Migrator.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string? ClientId { get; set; }
        public IList<ProtocolMapperObject> ProtocolMappers { get; set; } = new List<ProtocolMapperObject>();
    }
}
