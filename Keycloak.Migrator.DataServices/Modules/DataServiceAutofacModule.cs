using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Migrator.DataServices.Modules
{
    public class DataServiceAutofacModule : Autofac.Module
    {
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
        }
    }
}
