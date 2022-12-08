using Autofac;
using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Migrator.Models;
using System;
using System.CommandLine;
using System.IO;
using System.Linq;

namespace Keycloak.Migrator.Extensions
{
    internal static class SynchronizeCommandExtension
    {
        public static Command AddSynchronizeCommand(this Command command)
        {

            Option<Uri> keycloakUri = new Option<Uri>(
               "--keycloak-url",
               "The Uri to the Keycloak Endpoint."
            )
            {
                IsRequired = true,
            };

            Option<string> keycloakClientId = new Option<string>(
               "--keycloak-client-id",
               "The id of the client in Keycloak to sync."
)
            {
                IsRequired = true,
            };

            Option<string> keycloakPassword = new Option<string>(
                "--keycloak-password",
                "The password to the Keycloak Endpoint."
            )
            {
                IsRequired = true,
            }; ;

            Option<string> keycloakUserName = new Option<string>(
                "--keycloak-username",
                "The user name to the Keycloak Endpoint."
            )
            {
                IsRequired = true,
            };

            Option<FileInfo?> keycloakRealmExport = new Option<FileInfo?>(
                name: "--keycloak-realm-export",
                description: "The path to the realm export.",
                parseArgument: result =>
                {
                    if (result.Tokens.Count == 0)
                    {
                        return null;

                    }
                    string? filePath = result.Tokens.Single().Value;
                    if (!File.Exists(filePath))
                    {
                        result.ErrorMessage = "File does not exist";
                        return null;
                    }
                    else
                    {
                        return new FileInfo(filePath);
                    }
                }
            )
            {
                IsRequired = true,
            };

            Command synchronize = new Command("synchronize", "Synchronize the roles and groups to match the realm export.");

            synchronize.AddOption(keycloakUri);
            synchronize.AddOption(keycloakPassword);
            synchronize.AddOption(keycloakUserName);
            synchronize.AddOption(keycloakRealmExport);
            synchronize.AddOption(keycloakClientId);

            synchronize.SetHandler(async (uri,
                                   password,
                                   userName,
                                   realmExportFile,
                                   clientId) =>
            {
                if (realmExportFile == null)
                {
                    throw new ArgumentNullException(nameof(realmExportFile));
                }

                using var serviceProvider = ServiceProviderFactory.CreateServiceProvider(new KeycloakCredentials()
                {
                    Url = uri,
                    Password = password,
                    UserName = userName,
                }, containerBuilder: null);

                IDataParser dataParser = serviceProvider.Resolve<IDataParser>();
                IRolesSyncService rolesSyncService = serviceProvider.Resolve<IRolesSyncService>();
                IGroupSyncService groupSyncService = serviceProvider.Resolve<IGroupSyncService>();
                IUserSyncService userSyncService = serviceProvider.Resolve<IUserSyncService>();

                RealmExport? realmExport = await dataParser.ParseRealmExport(realmExportFile);

                if (realmExport == null)
                {
                    throw new ArgumentNullException(nameof(realmExport));
                }

                await rolesSyncService.SyncRoles(realmExport, clientId);
                await groupSyncService.SyncGroups(realmExport, clientId);
                await userSyncService.SyncUsers(realmExport, userName);

            }, keycloakUri, keycloakPassword, keycloakUserName, keycloakRealmExport, keycloakClientId);

            command.AddCommand(synchronize);

            return command;
        }
    }
}