using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AmvReporting.Infrastructure;
using AmvReporting.Infrastructure.Autofac;
using AmvReporting.Infrastructure.Automappings;
using Autofac.Integration.Mvc;


namespace AmvReporting
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // make sure we do separate words with spaces
            ModelMetadataProviders.Current = new ConventionProvider();

            AutoMapperBootstrapper.Initialize();

            var container = AutofacConfig.Configure();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }


        //Header removal
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            // Remove the "Server" HTTP Header from response
            var app = sender as HttpApplication;
            if (app == null || app.Context == null)
            {
                return;
            }
            var headers = app.Context.Response.Headers;
            headers.Remove("Server");
            headers.Remove("X-AspNetMvc-Version");
            headers.Remove("X-AspNet-Version");
            headers.Remove("X-Powered-By");
        }

    }
}
