using System.Collections.Generic;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    /// <summary>
    /// Client Update Service
    /// </summary>
    public interface IClientUpdateService
    {
        /// <summary>
        /// Adds redirect URIs to a client.
        /// </summary>
        /// <param name="redirectUris"></param>
        /// <param name="realmId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Task<bool> AddRedirectUris(List<string> redirectUris, string realmId, string clientId);
    }
}
