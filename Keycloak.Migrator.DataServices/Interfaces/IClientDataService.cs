using Keycloak.Net.Models.Clients;
using System;
using System.Linq;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    /// <summary>
    /// Client Data Service.
    /// </summary>
    public interface IClientDataService
    {
        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="realmId">The realm identifier.</param>
        /// <param name="clientIdName">Name of the client identifier.</param>
        /// <returns></returns>
        public Task<Client?> GetClient(string realmId, string clientIdName);
    }
}
