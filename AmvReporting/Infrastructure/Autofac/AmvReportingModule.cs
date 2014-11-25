using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using AmvReporting.Infrastructure.Caching;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Events;
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

            builder.RegisterType<MemoryCacheProvider>()
                .As<ICacheProvider>()
                .InstancePerDependency();

            builder.Register(c => GetPrincipal()).As<IPrincipal>().InstancePerDependency();

            base.Load(builder);
        }


        private static IPrincipal GetPrincipal()
        {
            // there is no good/easy/elegant way to check if HttpContext.Current is present or null.
            // so the safest is to do try-catch. And exception will only be trown only in cases where there 
            // is no Http request - in application_startup. And there it is not critical
            try
            {
                return HttpContext.Current.User ?? new ClaimsPrincipal(new ClaimsIdentity());
            }
            catch (Exception)
            {
                // return blank principal with no authentication
                var identity = new ClaimsIdentity();
                return new ClaimsPrincipal(identity);
            }
        }
    }
}