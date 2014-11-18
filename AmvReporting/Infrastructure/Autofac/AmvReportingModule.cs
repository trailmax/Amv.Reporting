using System;
using System.Linq;
using AmvReporting.Infrastructure.Caching;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Events;
using AmvReporting.Infrastructure.ModelEnrichers;
using Autofac;
using Autofac.Core;


namespace AmvReporting.Infrastructure.Autofac
{
    public static class AutofacKeys
    {
        public const String CommandHandler = "commandHandler";
        public const String AggregateCommandHandler = "AggregateCommandHandler";
        public const String TransactedCommandHandler = "TransactedCommandHandler";
        public const String QueryHandler = "queryHandler";
    }


    public class AmvReportingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(MvcApplication).Assembly;
            var types = assembly.GetTypes();


            builder.RegisterGeneric(typeof(NullObjectCommandValidator<>))
                .As(typeof(ICommandValidator<>));


            //Register All Command Handlers
            builder.RegisterTypes(types)
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .InstancePerLifetimeScope();


            //Register All Command Validators
            builder.RegisterTypes(types)
                .AsClosedTypesOf(typeof(ICommandValidator<>))
                .InstancePerLifetimeScope();


            // Register General Error message formatters
            builder.RegisterTypes(types)
                .AsClosedTypesOf(typeof(IErrorMessageFormatter<>))
                .InstancePerLifetimeScope();

            //Register All Query Handlers
            builder.RegisterTypes(types)
                .AsClosedTypesOf(typeof(IQueryHandler<,>))
                .InstancePerLifetimeScope();

            builder.RegisterTypes(types)
                .As(t => t.GetInterfaces()
                    .Where(a => a.IsClosedTypeOf(typeof(IQueryHandler<,>)))
                    .Select(a => new KeyedService(AutofacKeys.QueryHandler, a)))
                .InstancePerLifetimeScope();


            builder.RegisterGenericDecorator(
                typeof(CachedQueryHandlerDecorator<,>),
                typeof(IQueryHandler<,>),
                fromKey: AutofacKeys.QueryHandler).InstancePerLifetimeScope();

            builder.RegisterType<AutofacMediator>().As<IMediator>()
                .InstancePerLifetimeScope();


            builder.RegisterTypes(types)
                .AsClosedTypesOf(typeof(IEventHandler<>))
                .InstancePerLifetimeScope();


            builder.RegisterTypes(types)
                .AsClosedTypesOf(typeof(IModelEnricher<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<MemoryCacheProvider>()
                .As<ICacheProvider>()
                .InstancePerDependency();


            base.Load(builder);
        }
    }
}