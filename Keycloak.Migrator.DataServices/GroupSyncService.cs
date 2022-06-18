using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Migrator.Models;
using System;
using System.Linq;

namespace Keycloak.Migrator.DataServices
{
    public class GroupSyncService : IGroupSyncService
    {
        public Task<bool> SyncGroups(RealmExport realmExport)
        {
            throw new NotImplementedException();
        }
    }
}
