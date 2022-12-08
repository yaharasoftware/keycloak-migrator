using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Migrator.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices
{
    public class RoleMigrationService : IRoleMigrationService
    {
        private readonly ILogger<RoleMigrationService> _logger;
        private readonly IRolesDataService _rolesDataService;
        public RoleMigrationService(IRolesDataService rolesDataService,
            ILogger<RoleMigrationService> logger)
        {
            _rolesDataService = rolesDataService;
            _logger = logger;
        }

        public async Task<bool> MigrateRoles(ImportMigrationDataJSON jsonData)
        {
            var existingRoles = await _rolesDataService.GetRoles(jsonData.Realm);

            foreach(var role in jsonData.Roles)
            {
                //If the Role doesn't exist in existing roles
                if(!existingRoles.Any(r => r.Name == role.Name))
                {
                    var success = await _rolesDataService.AddRole(jsonData.Realm, new Net.Models.Roles.Role()
                    {
                        Name = role.Name,
                        Description = role.Description,
                    });

                    if(success)
                    {
                        _logger.LogInformation($"Added role '{role.Name}' in realm '{jsonData.Realm}'");
                    }
                }
            }
            return true;
        }

        public async Task<bool> ValidateRoles(ImportMigrationDataJSON jsonData)
        {
            var existingRoles = await _rolesDataService.GetRoles(jsonData.Realm);
            var fullSuccess = true;

            foreach (var role in jsonData.Roles)
            {
                //If the Role doesn't exist - validation has failed
                if (!existingRoles.Any(r => r.Name == role.Name))
                {
                    _logger.LogError($"Role '{role.Name}' was not found in realm '{jsonData.Realm}'");
                    fullSuccess = false;
                }
            }

            if(fullSuccess)
            {
                _logger.LogInformation($"All Roles Imported Successfully");
            }

            return true;
        }
    }
}
