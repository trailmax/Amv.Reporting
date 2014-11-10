using AmvReporting.Infrastructure.NEventStore;
using Autofac;
using CommonDomain.Core;
using CommonDomain.Persistence;
using CommonDomain.Persistence.EventStore;
using NEventStore;
using Raven.Client;
using Module = Autofac.Module;


namespace AmvReporting.Infrastructure.Autofac
{
    public class NEventStoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => WireupEventStore(c)).As<IStoreEvents>()
                .SingleInstance();

            builder.RegisterType<EventStoreRepository>().As<IRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AggregateFactory>().AsImplementedInterfaces();

            builder.RegisterType<ConflictDetector>().AsImplementedInterfaces();

            base.Load(builder);
        }

        private static IStoreEvents WireupEventStore(IComponentContext componentContext)
        {
            return Wireup.Init()
                         .UsingRavenPersistence(componentContext.Resolve<IDocumentStore>())
                         .UsingJsonSerialization()
                         .Build();


            //.UsingRavenPersistence()
            //    .UsingRavenPersistence()

            //.UsingSqlPersistence("EventStore") // Connection string is in app.config
            //.WithDialect(new MsSqlDialect())
            //.UsingJsonSerialization()

            //.UsingSynchronousDispatchScheduler()
            //.DispatchTo(new DelegateMessageDispatcher(c => DelegateDispatcher.DispatchCommit(bus, c)))
            //.Build();
        }
    }
}