using AmvReporting.Infrastructure.CQRS;
using Autofac;

namespace AmvReporting.Infrastructure.Autofac
{
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


            base.Load(builder);
        }
    }
}