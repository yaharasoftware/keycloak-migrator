using Autofac;
using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Migrator.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CommandLine;
using System.Linq;

namespace Keycloak.Migrator.Extensions
{
    internal static class MigrateCommandExtension
    {
        public static Command AddMigrateCommand(this Command command)
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

            Command migrate = new Command("migrate", "Migrate the roles and groups to match the realm export.");

            migrate.AddOption(keycloakUri);
            migrate.AddOption(keycloakPassword);
            migrate.AddOption(keycloakUserName);
            migrate.AddOption(keycloakRealmExport);
            migrate.AddOption(keycloakClientId);

            migrate.SetHandler(async (uri,
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

                IRealmDataParser dataParser = serviceProvider.Resolve<IRealmDataParser>();
                IRolesSyncService rolesSyncService = serviceProvider.Resolve<IRolesSyncService>();
                IGroupSyncService groupSyncService = serviceProvider.Resolve<IGroupSyncService>();

                RealmExport? realmExport = await dataParser.ParseRealmExport(realmExportFile);

                if (realmExport == null)
                {
                    throw new ArgumentNullException(nameof(realmExport));
                }

                await rolesSyncService.SyncRoles(realmExport, clientId);
                await groupSyncService.SyncGroups(realmExport, clientId);

            }, keycloakUri, keycloakPassword, keycloakUserName, keycloakRealmExport, keycloakClientId);

            command.AddCommand(migrate);

            return command;
        }
    }
}