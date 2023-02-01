using AutoMapper;
using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Migrator.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices
{
    public class ClientUpdateService : IClientUpdateService
    {
        private readonly ILogger<UserSyncService> _logger;
        private readonly IClientDataService _clientDataService;
        private readonly IMapper _mapper;
        public ClientUpdateService(IClientDataService clientDataService,
            IMapper mapper,
            ILogger<UserSyncService> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _clientDataService = clientDataService;
        }

        public async Task<bool> AddRedirectUris(List<string> redirectUris, string realmId, string clientId)
        {
            var client = await _clientDataService.GetClient(realmId, clientId);
            if (client != null)
            {
                redirectUris.AddRange(client.RedirectUris);
                client.RedirectUris = redirectUris;

                return await _clientDataService.UpdateClient(client, realmId, client.Id);
            }
            return false;
        }
    }
}
