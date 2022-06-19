using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Net;
using Keycloak.Net.Models.RealmsAdmin;
using System;
using System.Linq;

namespace Keycloak.Migrator.DataServices
{
    public class RealmDataService : IRealmDataService
    {
        private readonly KeycloakClient _keycloakClient;
        public RealmDataService(KeycloakClient keycloakClient)
        {
            _keycloakClient = keycloakClient;
        }

        public async Task<Realm> GetRealm(string realm)
        {
            return await _keycloakClient.GetRealmAsync(realm);
        }
    }
}
