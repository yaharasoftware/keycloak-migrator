using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Net;
using Keycloak.Net.Models.Clients;
using Keycloak.Net.Models.Groups;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices
{
    /// <summary>
    /// Client Data Service against Keycloak.Net
    /// TODO:  Replace Models with application specific models.
    /// </summary>
    /// <seealso cref="Keycloak.Migrator.DataServices.Interfaces.IClientDataService" />
    public class ClientDataService : IClientDataService
    {
        #region Member Variables
        private readonly KeycloakClient _keycloakClient;
        private readonly ILogger<ClientDataService> _logger;
        #endregion

        public ClientDataService(KeycloakClient keycloakClient,
            ILogger<ClientDataService> logger)
        {
            _logger = logger;
            _keycloakClient = keycloakClient;
        }

        public async Task<Client?> GetClient(string realmId, string clientIdName)
        {
            IEnumerable<Client> clients = await _keycloakClient.GetClientsAsync(realmId);
            return clients?.Where(c => c.ClientId == clientIdName).FirstOrDefault();
        }
    }
}
