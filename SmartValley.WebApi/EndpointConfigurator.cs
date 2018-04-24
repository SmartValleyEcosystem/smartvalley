using System.Data.SqlClient;
using System.Threading.Tasks;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using NServiceBus.Transport.SQLServer;
using SmartValley.Data.SQL.Core;
using SmartValley.Data.SQL.Repositories;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Admin;

namespace SmartValley.WebApi
{
    public static class EndpointConfigurator
    {
        public static Task<IEndpointInstance> StartAsync(IConfiguration configuration)
        {
            var endpointConfiguration = new EndpointConfiguration("SmartValley.Api");

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            ConfigureTransport(endpointConfiguration, connectionString);

            endpointConfiguration
                .Conventions()
                .DefiningCommandsAs(type => type.Namespace != null && type.Namespace.EndsWith("Commands"))
                .DefiningEventsAs(type => type.Namespace != null && type.Namespace.EndsWith("Events"));

            ConfigurePersistence(endpointConfiguration, connectionString);

            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

            ConfigureContainer(endpointConfiguration, connectionString);

            endpointConfiguration
                .Recoverability()
                .Immediate(settings => settings.NumberOfRetries(3))
                .Delayed(settings => settings.NumberOfRetries(5));

            endpointConfiguration.EnableOutbox();
            endpointConfiguration.EnableInstallers();

#if DEBUG
            endpointConfiguration.SetDiagnosticsPath("diagnostics");
#endif

            return Endpoint.Start(endpointConfiguration);
        }

        private static void ConfigureTransport(EndpointConfiguration endpointConfiguration, string connectionString)
        {
            endpointConfiguration
                .UseTransport<SqlServerTransport>()
                .ConnectionString(connectionString)
                .DefaultSchema("msg")
                .Transactions(TransportTransactionMode.SendsAtomicWithReceive);
        }

        private static void ConfigurePersistence(EndpointConfiguration endpointConfiguration, string connectionString)
        {
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();

            persistence.SqlDialect<SqlDialect.MsSqlServer>()
                       .Schema("nsb");

            persistence.ConnectionBuilder(() => new SqlConnection(connectionString));

            persistence.SubscriptionSettings()
                       .DisableCache();
        }

        private static void ConfigureContainer(EndpointConfiguration endpointConfiguration, string connectionString)
        {
            var containerBuilder = new ContainerBuilder();

            var contextOptionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
            contextOptionsBuilder.UseSqlServer(connectionString);

            containerBuilder.Register(context => AppDBContext.CreateEditable(contextOptionsBuilder.Options))
                            .As<IEditableDataContext>();

            containerBuilder.Register(context => AppDBContext.CreateReadOnly(contextOptionsBuilder.Options))
                            .As<IReadOnlyDataContext>();

            containerBuilder.RegisterType<UserRepository>().As<IUserRepository>();
            containerBuilder.RegisterType<AdminService>().As<IAdminService>();

            var container = containerBuilder.Build();

            endpointConfiguration.UseContainer<AutofacBuilder>(customizations => customizations.ExistingLifetimeScope(container));
        }
    }
}