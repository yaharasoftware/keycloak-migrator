﻿using AutoMapper;
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
        private readonly IClientDataService _clientDataService;
        private readonly IMapper _mapper;
        public UserMigrationService(IUserDataService userDataService,
            IRolesDataService roleDataService,
            IMapper mapper,
            IClientDataService clientDataService,
            ILogger<UserMigrationService> logger)
        {
            _userDataService = userDataService;
            _logger = logger;
            _roleDataService = roleDataService;
            _clientDataService = clientDataService;
            _mapper = mapper;
        }

        public async Task<bool> MigrateUsers(ImportMigrationDataJSON jsonData)
        {
            var existingUsers = await _userDataService.GetUsers(jsonData.Realm);
            var roles = await _roleDataService.GetRoles(jsonData.Realm);

            foreach (var user in jsonData.Users)
            {
                //If the User doesn't exist in existing Users
                if (!existingUsers.Any(r => r.UserName == user.UserName))
                {
                    var userSync = _mapper.Map<Net.Models.Users.User>(user);

                    //Create User
                    var success = await _userDataService.AddUser(jsonData.Realm, userSync);

                    if (success)
                    {
                        _logger.LogInformation($"Added user '{user.UserName}' in realm '{jsonData.Realm}'");
                    }

                    //Get User - for ID
                    existingUsers = await _userDataService.GetUsers(jsonData.Realm);

                    success = await _userDataService.SetUserPassword(jsonData.Realm, existingUsers.First(u => u.UserName == user.UserName), user.Password, user.TemporaryPassword);

                    if (success)
                    {
                        _logger.LogInformation($"Set password for user '{user.UserName}' in realm '{jsonData.Realm}'");
                    }

                    //Add Realm Roles
                    //Get User - for up to date information
                    existingUsers = await _userDataService.GetUsers(jsonData.Realm);

                    success = await _userDataService.UpdateUserRealmRoles(jsonData.Realm, existingUsers.First(u => u.UserName == user.UserName), roles.Where(r => user.RealmRoles.Contains(r.Name)));

                    if (success)
                    {
                        _logger.LogInformation($"Added roles to user '{user.UserName}' in realm '{jsonData.Realm}'");
                    }

                    //Add Client Roles
                    foreach (var clientRoleList in user.ClientRoles)
                    {
                        var client = await _clientDataService.GetClient(jsonData.Realm, clientRoleList.Key);
                        if (client != null)
                        {
                            var existingClientRoles = await _roleDataService.GetRoles(jsonData.Realm, client.Id);
                            success = await _userDataService.UpdateUserClientRoles(jsonData.Realm, 
                                            existingUsers.First(u => u.UserName == user.UserName),
                                            client.Id,
                                            existingClientRoles.Where(existingRole => clientRoleList.Value.Any(newRoles => newRoles == existingRole.Name)));

                            if (success)
                            {
                                _logger.LogInformation($"Added roles to user '{user.UserName}' for client '{clientRoleList.Key}' in realm '{jsonData.Realm}'");
                            }
                        }
                    }

                }
            }

            existingUsers = await _userDataService.GetUsers(jsonData.Realm);
            //Realm Role additions
            foreach (var roleAdditions in jsonData.UserRealmRoleAdditions)
            {
                var success = await _userDataService.UpdateUserRealmRoles(jsonData.Realm, existingUsers.First(u => u.UserName == roleAdditions.UserName), roles.Where(r => roleAdditions.Roles.Contains(r.Name)));

                if (success)
                {
                    _logger.LogInformation($"Updated roles on user '{roleAdditions.UserName}' in realm '{jsonData.Realm}'");
                }
            }

            //Client Role Additions
            foreach (var roleAdditions in jsonData.UserClientRoleAdditions)
            {
                var client = await _clientDataService.GetClient(jsonData.Realm, roleAdditions.Client);
                if (client != null)
                {
                    var existingClientRoles = await _roleDataService.GetRoles(jsonData.Realm, client.Id);
                    var success = await _userDataService.UpdateUserClientRoles(jsonData.Realm,
                                    existingUsers.First(u => u.UserName == roleAdditions.UserName),
                                    client.Id,
                                    existingClientRoles.Where(existingRole => roleAdditions.Roles.Any(newRoles => newRoles == existingRole.Name)));

                    if (success)
                    {
                        _logger.LogInformation($"Added roles to user '{roleAdditions.UserName}' for client '{roleAdditions.Client}' in realm '{jsonData.Realm}'");
                    }
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
                var realmUser = existingUsers.First(u => u.UserName == user.UserName);
                //If the User doesn't exist, validation fails
                if (realmUser == null)
                {
                    _logger.LogError($"Role '{user.UserName}' was not found in realm '{jsonData.Realm}'");
                    fullSuccess = false;
                    continue;
                }
                else
                {

                    foreach (var role in user.RealmRoles)
                    {
                        if (!realmUser.RealmRoles.Contains(role))
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
