using AmvReporting.Infrastructure.NEventStore;
using Autofac;
using CommonDomain.Core;
using CommonDomain.Persistence;
using CommonDomain.Persistence.EventStore;
using NEventStore;
using Module = Autofac.Module;


namespace AmvReporting.Infrastructure.Autofac
{
    public class NEventStoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => WireupEventStore()).As<IStoreEvents>()
                .SingleInstance();

            builder.RegisterType<EventStoreRepository>().As<IRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AggregateFactory>().AsImplementedInterfaces();

            builder.RegisterType<ConflictDetector>().AsImplementedInterfaces();

            base.Load(builder);
        }

        private static IStoreEvents WireupEventStore()
        {
            return Wireup.Init()
                         .UsingRavenPersistence(RavenDbModule.CreateDocumentStore)
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