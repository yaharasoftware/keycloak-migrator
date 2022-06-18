using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Net;
using Keycloak.Net.Models.Clients;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices
{
    public class ClientDataService : IClientDataService
    {
        private readonly KeycloakClient _keycloakClient;
        private readonly ILogger<ClientDataService> _logger;

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
