using System;
using System.Linq;
using System.Threading.Tasks;
using MigratorModel = Keycloak.Migrator.Models;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    /// <summary>
    /// User Sync Service
    /// </summary>
    public interface IUserSyncService
    {
        /// <summary>
        /// Synchronizes the users.
        /// </summary>
        /// <param name="realmExport">The realm export.</param>
        /// <returns></returns>
        public Task<bool> SyncUsers(MigratorModel.RealmExport realmExport);
    }
}
