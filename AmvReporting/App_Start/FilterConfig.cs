using System.Web.Mvc;
using AmvReporting.Infrastructure.Configuration;
using AmvReporting.Infrastructure.Filters;

namespace AmvReporting
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            if (ConfigurationContext.Current.RequireHttps())
            {
                filters.Add(new RequireSecureConnectionFilter());
            }
        }
    }
}
