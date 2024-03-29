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
    public class UserSyncService : IUserSyncService
    {
        private readonly ILogger<UserSyncService> _logger;
        private readonly IUserDataService _userDataService;
        private readonly IMapper _mapper;
        public UserSyncService(IUserDataService userDataService,
            IClientDataService clientDataService,
            IMapper mapper,
            ILogger<UserSyncService> logger)
        {
            _userDataService = userDataService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> SyncUsers(RealmExport realmExport, string loginUser, bool deleteMissingUsers)
        {
            if (realmExport is null)
            {
                throw new ArgumentNullException(nameof(realmExport));
            }

            if (realmExport.Realm is null)
            {
                _logger.LogError("RealmExport.Id is null");
                return false;
            }

            // Get the list of users from the export
            IEnumerable<User> realmExportUsers = realmExport.Users;

            // Get the list of users from keycloak.
            IEnumerable<Net.Models.Users.User> keycloakUsers = await _userDataService.GetUsers(realmExport.Realm);

            // Log the users found in the system.
            _logger.LogInformation($"{keycloakUsers.Count()} users found in keycloak.  {realmExportUsers.Count()} users found in export.");

            if (deleteMissingUsers)
            {
                // Delete the users from keycloak that are not in the export.
                await this.DeleteMissingUsers(realmExport.Realm, realmExportUsers, keycloakUsers, loginUser);

                // Get the updated list from keycloak.
                keycloakUsers = await _userDataService.GetUsers(realmExport.Realm);
            }

            // Add missing users.
            await this.AddMissingUsers(realmExport.Realm, realmExportUsers, keycloakUsers);

            // Get the updated list from keycloak.
            keycloakUsers = await _userDataService.GetUsers(realmExport.Realm);

            // Updated users within keycloak if different.
            await this.UpdateChangedUsers(realmExport.Realm, realmExportUsers, keycloakUsers);

            return true;
        }

        private async Task UpdateChangedUsers(string realm, IEnumerable<User> realmExportUsers, IEnumerable<Net.Models.Users.User> keycloakUsers)
        {
            foreach (User realmExportUser in realmExportUsers)
            {
                Net.Models.Users.User? existingUser = keycloakUsers.Where(kr => kr.UserName == realmExportUser.UserName).FirstOrDefault();

                if (existingUser is null)
                {
                    _logger.LogWarning($"Unable to locate user '{realmExportUser.UserName}' for realm {realm}.");
                    continue;
                }

                if (realmExportUser.FirstName != existingUser.FirstName || realmExportUser.LastName != existingUser.LastName || realmExportUser.Email != existingUser.Email)
                {
                    _logger.LogInformation($"Updating attributes for user '{existingUser.UserName}'");

                    existingUser.FirstName = realmExportUser.FirstName;
                    existingUser.LastName = realmExportUser.LastName;
                    existingUser.Email = realmExportUser.Email;

                    await _userDataService.UpdateUser(realm, existingUser);
                }

            }
        }

        private async Task DeleteMissingUsers(string realm,
            IEnumerable<User> realmExportUsers,
            IEnumerable<Net.Models.Users.User> keycloakUsers,
            string loginUser)
        {
            _logger.LogInformation("Checking for users to delete...");
            foreach (Net.Models.Users.User keycloakUser in keycloakUsers)
            {
                if (keycloakUser.UserName == loginUser)
                {
                    _logger.LogWarning("Login user was not in realm export - ignoring expected deletion.");
                    continue;
                }
                if (!realmExportUsers.Any(x => x.UserName == keycloakUser.UserName))
                {
                    _logger.LogInformation($"Deleting user '{keycloakUser.UserName}' in realm '{realm}'");
                    await _userDataService.DeleteUser(realm, keycloakUser);
                }
            }
        }

        private async Task AddMissingUsers(string realm,
            IEnumerable<User> exportUsers,
            IEnumerable<Net.Models.Users.User> keycloakUsers)
        {
            _logger.LogInformation($"Adding missing users...");

            List<User> newUsers = exportUsers.Where(eu => !keycloakUsers.Any(ku => ku.UserName == eu.UserName)).ToList();

            foreach (User user in newUsers)
            {
                _logger.LogInformation($"Adding role '{user.UserName}' in realm '{realm}'");

                //TODO: All attributes needed?
                var newUser = _mapper.Map<Net.Models.Users.User>(user);
                await _userDataService.AddUser(realm, newUser);
            }

            if (newUsers.Count == 0)
            {
                _logger.LogInformation($"No new roles to add to keycloak.");
            }
        }
    }
}
