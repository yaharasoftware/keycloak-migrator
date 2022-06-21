using Keycloak.Migrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    /// <summary>
    /// Realm Export Data Parser
    /// </summary>
    public interface IRealmDataParser
    {
        /// <summary>
        /// Parses the realm export.
        /// </summary>
        /// <param name="realmExportFile">The realm export file.</param>
        /// <returns></returns>
        Task<RealmExport?> ParseRealmExport(FileInfo realmExportFile);
    }
}
