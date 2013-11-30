using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AmvReporting.Infrastructure;
using AmvReporting.Infrastructure.Autofac;
using Autofac.Integration.Mvc;

namespace AmvReporting
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // make sure we do separate words with spaces
            ModelMetadataProviders.Current = new ConventionProvider();

            var container = AutofacConfig.Configure();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
