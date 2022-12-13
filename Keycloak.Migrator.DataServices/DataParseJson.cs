using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Migrator.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices
{
    public class DataParseJson : IDataParser
    {
        private readonly ILogger<DataParseJson> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataParseJson"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DataParseJson(ILogger<DataParseJson> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Parses the realm export.
        /// </summary>
        /// <param name="realmExportFile">The realm export file.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">realmExportFile</exception>
        /// <exception cref="System.IO.FileNotFoundException"></exception>
        public async Task<RealmExport?> ParseRealmExport(FileInfo realmExportFile)
        {
            if (realmExportFile is null)
            {
                throw new ArgumentNullException(nameof(realmExportFile));
            }

            if (!realmExportFile.Exists)
            {
                throw new FileNotFoundException();
            }

            string readAllText = await File.ReadAllTextAsync(realmExportFile.FullName);

            if (string.IsNullOrEmpty(readAllText))
            {
                _logger.LogWarning("The file provided as the realm export is empty.");
                return null;
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<RealmExport>(readAllText);
        }

        /// <summary>
        /// Parses the migration JSON
        /// </summary>
        /// <param name="jsonMigrationFile"></param>
        /// <returns></returns>
        public async Task<ImportMigrationDataJSON?> ParseMigrationJSON(FileInfo jsonMigrationFile)
        {
            if (jsonMigrationFile is null)
            {
                throw new ArgumentNullException(nameof(jsonMigrationFile));
            }

            if (!jsonMigrationFile.Exists)
            {
                throw new FileNotFoundException();
            }

            string readAllText = await File.ReadAllTextAsync(jsonMigrationFile.FullName);

            if (string.IsNullOrEmpty(readAllText))
            {
                _logger.LogWarning("The file provided as the realm export is empty.");
                return null;
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<ImportMigrationDataJSON>(readAllText);
        }
    }
}