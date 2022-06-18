using Autofac;
using Autofac.Extensions.DependencyInjection;
using Keycloak.Migrator.DataServices.Modules;
using Keycloak.Migrator.Models;
using Keycloak.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Linq;

namespace Keycloak.Migrator
{
    internal static class ServiceProviderFactory
    {
        public static IContainer CreateServiceProvider(KeycloakCredentials keycloakCredentials, Action<ContainerBuilder>? containerBuilder)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            IConfigurationRoot config = configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            ContainerBuilder builder = new ContainerBuilder();

            if (containerBuilder != null)
            {
                containerBuilder.Invoke(builder);
            }

            builder.RegisterInstance(keycloakCredentials);

            builder.RegisterModule<DataServiceAutofacModule>();

            builder.Register((ctx) =>
            {
                KeycloakCredentials credentials = ctx.Resolve<KeycloakCredentials>();

                KeycloakClient keycloakClient = new Keycloak.Net.KeycloakClient(credentials.Url?.ToString(),
                           credentials.UserName,
                           credentials.Password);

                return keycloakClient;
            })
            .AsSelf()
            .SingleInstance();


            builder
             .RegisterInstance(config)
             .AsImplementedInterfaces()
             .SingleInstance();

            IServiceCollection loggingConfigurations = new ServiceCollection()
                .AddLogging(loggingBuilder =>
                {
                    // configure Logging with NLog
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    loggingBuilder.AddNLog(config);
                });

            builder.Populate(loggingConfigurations);

            IContainer container = builder.Build();

            return container;
        }
    }
}
