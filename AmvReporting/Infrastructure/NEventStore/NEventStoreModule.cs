using System.Collections.Generic;
using System.Reflection;
using Autofac;
using CommonDomain.Core;
using CommonDomain.Persistence;
using CommonDomain.Persistence.EventStore;
using NEventStore;
using Raven.Client;
using Module = Autofac.Module;


namespace AmvReporting.Infrastructure.NEventStore
{
    public class NEventStoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(WireupEventStore).As<IStoreEvents>()
                .SingleInstance();

            builder.RegisterType<EventStoreRepository>()
                .As<IRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AggregateFactory>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<ConflictDetector>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(NEventStoreModule)))
                .Where(t => typeof(IPipelineHook).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            base.Load(builder);
        }

        private static IStoreEvents WireupEventStore(IComponentContext componentContext)
        {
            return Wireup.Init()
                         .UsingRavenPersistence(componentContext.Resolve<IDocumentStore>())
                         .InitializeStorageEngine()
                         .UsingJsonSerialization()
                         .HookIntoPipelineUsing(componentContext.Resolve<IEnumerable<IPipelineHook>>())
                         .Build();
        }
    }
}