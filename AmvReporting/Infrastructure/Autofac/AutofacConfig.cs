using Autofac;
using Autofac.Integration.Mvc;

namespace AmvReporting.Infrastructure.Autofac
{
    public class AutofacConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterFilterProvider();

            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);

            builder.RegisterModelBinderProvider();
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterSource(new ViewRegistrationSource());

            builder.RegisterModule(new AmvReportingModule());
            builder.RegisterModule(new RavenDbModule());

            var container = builder.Build();

            return container;
        }
    }
}