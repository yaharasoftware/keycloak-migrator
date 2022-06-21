using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Migrator.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices
{
    public class RealmDataParseJson : IRealmDataParser
    {
        private readonly ILogger<RealmDataParseJson> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RealmDataParseJson"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public RealmDataParseJson(ILogger<RealmDataParseJson> logger)
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
    }
}