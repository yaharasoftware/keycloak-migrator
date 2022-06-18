using Keycloak.Net.Models.Clients;
using System;
using System.Linq;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    public interface IClientDataService
    {
        public Task<Client?> GetClient(string realmId, string clientIdName);
    }
}
