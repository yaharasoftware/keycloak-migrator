using System;
using System.Linq;
using MigratorModel = Keycloak.Migrator.Models;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    public interface IGroupSyncService
    {
        public Task<bool> SyncGroups(MigratorModel.RealmExport realmExport);
    }
}
