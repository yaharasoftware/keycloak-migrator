using System.Collections;

namespace Keycloak.Migrator.Models
{
    public class RoleCollection<T> where T : IEnumerable, new()
    {
        public T Realm { get; set; } = new T();
        public IDictionary<string, T> Client { get; set; } = new Dictionary<string, T>();

    }
}
