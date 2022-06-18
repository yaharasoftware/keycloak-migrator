using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Migrator.Models;
using Microsoft.Extensions.Logging;

namespace Keycloak.Migrator.DataServices
{
    public class RolesSyncService : IRolesSyncService
    {
        private readonly ILogger<RolesSyncService> _logger;
        private readonly IRolesDataService _rolesDataService;
        private readonly IClientDataService _clientDataService;
        public RolesSyncService(IRolesDataService rolesDataService,
            IClientDataService clientDataService,
            ILogger<RolesSyncService> logger)
        {
            _clientDataService = clientDataService;
            _rolesDataService = rolesDataService;
            _logger = logger;
        }

        public async Task<bool> SyncRoles(RealmExport realmExport, string clientId)
        {
            if (realmExport is null)
            {
                throw new ArgumentNullException(nameof(realmExport));
            }

            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException($"'{nameof(clientId)}' cannot be null or empty.", nameof(clientId));

            }

            if (realmExport.Id is null)
            {
                _logger.LogError("RealmExport.Id is null");
                return false;
            }

            Net.Models.Clients.Client? client = await _clientDataService.GetClient(realmExport.Id, clientId);

            if (client is null)
            {
                _logger.LogError($"Client '{clientId}' not found in realm '{realmExport.Realm}'");
                return false;
            }


            IEnumerable<Net.Models.Roles.Role> roles = await _rolesDataService.GetRoles(realmExport.Id, client.Id);

            return true;
        }
    }
}
