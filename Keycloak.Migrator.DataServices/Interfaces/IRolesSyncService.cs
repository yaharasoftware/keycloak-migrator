using System;
using System.Linq;
using MigratorModel = Keycloak.Migrator.Models;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    /// <summary>
    /// Roles Sync Service
    /// </summary>
    public interface IRolesSyncService
    {
        /// <summary>
        /// Synchronizes the roles.
        /// </summary>
        /// <param name="realmExport">The realm export.</param>
        /// <param name="clientIdentifier">The client identifier.</param>
        /// <returns></returns>
        public Task<bool> SyncRoles(MigratorModel.RealmExport realmExport, string clientIdentifier);
    }
}
