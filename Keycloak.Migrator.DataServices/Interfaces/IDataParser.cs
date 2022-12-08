using Keycloak.Migrator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    /// <summary>
    /// Realm Export Data Parser
    /// </summary>
    public interface IDataParser
    {
        /// <summary>
        /// Parses the realm export.
        /// </summary>
        /// <param name="realmExportFile">The realm export file.</param>
        /// <returns></returns>
        Task<RealmExport?> ParseRealmExport(FileInfo realmExportFile);

        /// <summary>
        /// Parses the migration JSON
        /// </summary>
        /// <param name="jsonMigrationFile"></param>
        /// <returns></returns>
        Task<ImportMigrationDataJSON?> ParseMigrationJSON(FileInfo jsonMigrationFile);
    }
}
