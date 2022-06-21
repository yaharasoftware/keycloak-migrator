using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices.Modules
{
    /// <summary>
    /// Data Service Autofac Module
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    public class DataServiceAutofacModule : Autofac.Module
    {
        /// <summary>
        /// Loads the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<RealmDataParseJson>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<ClientDataService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<RolesDataService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<RolesSyncService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<GroupSyncService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<GroupDataService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<RealmDataService>()
                .AsImplementedInterfaces()
                .SingleInstance();


        }
    }
}
