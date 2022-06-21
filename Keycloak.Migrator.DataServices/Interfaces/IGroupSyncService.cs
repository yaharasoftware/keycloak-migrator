using System;
using System.Linq;
using MigratorModel = Keycloak.Migrator.Models;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    /// <summary>
    /// Group Sync Service
    /// </summary>
    public interface IGroupSyncService
    {
        /// <summary>
        /// Synchronizes the groups.
        /// </summary>
        /// <param name="realmExport">The realm export.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public Task<bool> SyncGroups(MigratorModel.RealmExport realmExport, string clientId);
    }
}
