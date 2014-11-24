using System.Web.Mvc;
using System.Web.Routing;

namespace AmvReporting
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //routes.MapRoute(
            //    name: "Report_home_guid",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index" }
            //);

            routes.MapRoute(
                "Home_Report_legacy",
                "Home/Report/",
                new { action = "LegacyReport", controller = "Home" },
                new { id = @"reports\/\d+" });

            routes.MapRoute(
                "Report_Home_Guid",
                "Home/Report/{id}",
                new { action = "Report", controller = "Home" },
                new { id = @"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index"}
            );
        }
    }
}
