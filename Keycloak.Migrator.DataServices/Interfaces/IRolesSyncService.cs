using System;
using System.Linq;
using MigratorModel = Keycloak.Migrator.Models;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    public interface IRolesSyncService
    {
        public Task<bool> SyncRoles(MigratorModel.RealmExport realmExport, string clientId);
    }
}
