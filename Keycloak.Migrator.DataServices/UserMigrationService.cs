using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Migrator.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices
{
    public class UserMigrationService : IUserMigrationService
    {
        private readonly ILogger<UserMigrationService> _logger;
        private readonly IUserDataService _userDataService;
        private readonly IRolesDataService _roleDataService;
        //private readonly IRolesDataService
        public UserMigrationService(IUserDataService userDataService,
            IRolesDataService roleDataService,
            ILogger<UserMigrationService> logger)
        {
            _userDataService = userDataService;
            _logger = logger;
            _roleDataService = roleDataService;
        }

        public async Task<bool> MigrateUsers(ImportMigrationDataJSON jsonData)
        {
            var existingUsers = await _userDataService.GetUsers(jsonData.Realm);
            var roles = await _roleDataService.GetRoles(jsonData.Realm);

            foreach (var user in jsonData.Users)
            {
                //If the User doesn't exist in existing Users
                if(!existingUsers.Any(r => r.Email == user.Email))
                {
                    var userSync = new Net.Models.Users.User()
                    {
                        UserName = user.User_name,
                        FirstName = user.First_name,
                        LastName = user.Last_name,
                        Email = user.Email,
                        Enabled = true
                    };
                    //Create User
                    var success = await _userDataService.AddUser(jsonData.Realm, userSync);

                    if (success)
                    {
                        _logger.LogInformation($"Added user '{user.Email}' in realm '{jsonData.Realm}'");
                    }

                    //Add Roles
                    //Get User - for ID
                    existingUsers = await _userDataService.GetUsers(jsonData.Realm);

                    success = await _userDataService.UpdateUserRoles(jsonData.Realm, existingUsers.First(u => u.UserName == user.User_name), roles.Where(r => user.Roles.Contains(r.Name)));

                    if (success)
                    {
                        _logger.LogInformation($"Added roles to user '{user.Email}' in realm '{jsonData.Realm}'");
                    }

                }
            }

            existingUsers = await _userDataService.GetUsers(jsonData.Realm);
            foreach (var roleAdditions in jsonData.User_role_additions)
            {
                var success = await _userDataService.UpdateUserRoles(jsonData.Realm, existingUsers.First(u => u.Email == roleAdditions.Email), roles.Where(r => roleAdditions.Roles.Contains(r.Name)));

                if (success)
                {
                    _logger.LogInformation($"Updated roles on user '{roleAdditions.Email}' in realm '{jsonData.Realm}'");
                }
            }

            return true;
        }

        public async Task<bool> ValidateUsers(ImportMigrationDataJSON jsonData)
        {
            var existingUsers = await _userDataService.GetUsers(jsonData.Realm);
            var fullSuccess = true;

            foreach (var user in jsonData.Users)
            {
                var realmUser = existingUsers.First(u => u.Email == user.Email);
                //If the User doesn't exist, validation fails
                if (realmUser == null)
                {
                    _logger.LogError($"Role '{user.Email}' was not found in realm '{jsonData.Realm}'");
                    fullSuccess = false;
                    continue;
                }
                else
                {

                    foreach (var role in user.Roles)
                    {
                        if(!realmUser.RealmRoles.Contains(role))
                        {
                            _logger.LogError($"Role '{role}' was not found in user '{jsonData.Realm}'");
                            fullSuccess = false;
                        }
                    }
                }
            }

            if (fullSuccess)
            {
                _logger.LogInformation($"All Users Imported Successfully");
            }

            return true;
        }
    }
}
