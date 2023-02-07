using Autofac;
using Keycloak.Migrator.DataServices.Interfaces;
using Keycloak.Migrator.Models;
using System;
using System.CommandLine;
using System.IO;
using System.Linq;

namespace Keycloak.Migrator.Extensions
{
    internal static class ClientCommandExtension
    {
        public static Command AddClientCommand(this Command command)
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
               "The id of the client in Keycloak to update."
)
            {
                IsRequired = true,
            };

            Option<string> keycloakRealmId = new Option<string>(
               "--keycloak-realm-id",
               "The realm id of the realm in Keycloak to update."
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

            Option<string> redirectUris = new Option<string>(
                "--redirect-uris",
                "Semicolon delimited list of redirect URIs to add to the client"
            )
            {
                IsRequired = true,
            };

            Command client = new Command("client", "Commands related to Client Functionality.");

            Command clientUpdate = new Command("update", "Commands related to updating client functionality");

            Command addRedirectUri = new Command("add-redirect-uri", "Adds redirect URIs to a client");

            clientUpdate.AddCommand(addRedirectUri);

            client.AddCommand(clientUpdate);


            addRedirectUri.AddOption(keycloakUri);
            addRedirectUri.AddOption(keycloakPassword);
            addRedirectUri.AddOption(keycloakUserName);
            addRedirectUri.AddOption(keycloakClientId);
            addRedirectUri.AddOption(redirectUris);
            addRedirectUri.AddOption(keycloakRealmId);

            addRedirectUri.SetHandler(async (uri,
                                   password,
                                   userName,
                                   clientId,
                                   redirectUris,
                                   realmId) =>
            {

                using var serviceProvider = ServiceProviderFactory.CreateServiceProvider(new KeycloakCredentials()
                {
                    Url = uri,
                    Password = password,
                    UserName = userName,
                }, containerBuilder: null);

                IClientUpdateService clientUpdateService = serviceProvider.Resolve<IClientUpdateService>();

                await clientUpdateService.AddRedirectUris(redirectUris.Split(';').ToList(), realmId, clientId);

            }, keycloakUri, keycloakPassword, keycloakUserName, keycloakClientId, redirectUris, keycloakRealmId);

            command.AddCommand(client);

            return command;
        }
    }
}