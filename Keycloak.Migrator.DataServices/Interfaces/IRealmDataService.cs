using Keycloak.Net.Models.RealmsAdmin;
using System;
using System.Linq;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    public interface IRealmDataService
    {
        public Task<Realm> GetRealm(string realmName);
    }
}
