using Keycloak.Net.Models.RealmsAdmin;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    /// <summary>
    /// Realm Data Service
    /// </summary>
    public interface IRealmDataService
    {
        /// <summary>
        /// Gets the realm.
        /// </summary>
        /// <param name="realmName">Name of the realm.</param>
        /// <returns></returns>
        public Task<Realm> GetRealm(string realmName);
    }
}
