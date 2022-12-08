using Keycloak.Migrator.Models;
using Keycloak.Net.Models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    /// <summary>
    /// Roles Data Service
    /// </summary>
    public interface IRoleMigrationService
    {
        public Task<bool> MigrateRoles(ImportMigrationDataJSON jsonData);

        public Task<bool> ValidateRoles(ImportMigrationDataJSON jsonData);
    }
}
