using System.Collections;
using System.Collections.Generic;

namespace Keycloak.Migrator.Models
{
    public class RoleCollection<T> where T : IEnumerable, new()
    {
        public T Realm { get; set; } = new T();
        public IDictionary<string, T> Client { get; set; } = new Dictionary<string, T>();

    }
}
