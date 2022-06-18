
using Autofac;
using Keycloak.Migrator.DataServices.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Keycloak.Migrator.DataService.Test.Fixtures
{
    public class DataServiceFixture : IDisposable
    {
        /// <summary>
        /// The container builder
        /// </summary>
        private readonly ContainerBuilder _containerBuilder = new ContainerBuilder();


        /// <summary>
        /// The container
        /// </summary>
        private IContainer? _container = null;
        private bool disposedValue;

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public IContainer Container { get => GetContainer(); }


        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <returns></returns>
        private IContainer GetContainer()
        {
            if (_container == null)
            {
                this.RegisterDefaults();
                this.Register(_containerBuilder);

                _container = _containerBuilder.Build();
            }

            return _container;
        }

        /// <summary>
        /// Registers additional modules or types to the  builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected virtual void Register(ContainerBuilder builder)
        {
            // Do nothing.
        }


        /// <summary>
        /// Registers the defaults.
        /// </summary>
        private void RegisterDefaults()
        {


            _containerBuilder
                .Register(ctx => LoggerFactory.Create(builder => builder
                    .AddConsole()
                    .AddDebug())
                )
                .AsImplementedInterfaces()
                .SingleInstance();

            _containerBuilder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            _containerBuilder
                .RegisterModule<DataServiceAutofacModule>();

            _containerBuilder
                .Register((ctx) =>
                {
                    IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddEnvironmentVariables()
                        .AddJsonFile("appSettings.json", optional: true)
                        .AddUserSecrets<DataServiceFixture>();

                    IConfigurationRoot configuration = configurationBuilder.Build();

                    return configuration;
                })
                .As<IConfigurationRoot>()
                .SingleInstance();

        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DataServiceTestFixture()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

