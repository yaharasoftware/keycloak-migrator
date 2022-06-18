using Keycloak.Migrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    public interface IRealmDataParser
    {
        Task<RealmExport?> ParseRealmExport(FileInfo realmExportFile);
    }
}
