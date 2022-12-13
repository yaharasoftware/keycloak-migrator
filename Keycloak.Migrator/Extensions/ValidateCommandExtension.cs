using Autofac;
using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Migrator.Models;
using System;
using System.CommandLine;
using System.IO;
using System.Linq;

namespace Keycloak.Migrator.Extensions
{
    internal static class ValidateCommandExtension
    {
        public static Command AddValidateCommand(this Command command)
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

            Option<FileInfo?> keycloakJsonMigration = new Option<FileInfo?>(
                name: "--keycloak-json-migration",
                description: "The path to the json migration.",
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

            Command validate = new Command("validate", "Validate the roles and users from JSON File.");

            validate.AddOption(keycloakUri);
            validate.AddOption(keycloakPassword);
            validate.AddOption(keycloakUserName);
            validate.AddOption(keycloakJsonMigration);
            validate.AddOption(keycloakClientId);

            validate.SetHandler(async (uri,
                                   password,
                                   userName,
                                   jsonMigrationFile,
                                   clientId) =>
            {
                if (jsonMigrationFile == null)
                {
                    throw new ArgumentNullException(nameof(jsonMigrationFile));
                }

                using var serviceProvider = ServiceProviderFactory.CreateServiceProvider(new KeycloakCredentials()
                {
                    Url = uri,
                    Password = password,
                    UserName = userName,
                }, containerBuilder: null);

                IDataParser dataParser = serviceProvider.Resolve<IDataParser>();
                IRoleMigrationService roleMigrationService = serviceProvider.Resolve<IRoleMigrationService>();
                IUserMigrationService userMigrationService = serviceProvider.Resolve<IUserMigrationService>();

                ImportMigrationDataJSON? jsonMigrationData = await dataParser.ParseMigrationJSON(jsonMigrationFile);

                if (jsonMigrationData == null)
                {
                    throw new ArgumentNullException(nameof(jsonMigrationData));
                }

                await roleMigrationService.ValidateRoles(jsonMigrationData);
                await userMigrationService.ValidateUsers(jsonMigrationData);

            }, keycloakUri, keycloakPassword, keycloakUserName, keycloakJsonMigration, keycloakClientId);

            command.AddCommand(validate);

            return command;
        }
    }
}