using System;
using System.Web.Mvc;

namespace AmvReporting.Infrastructure.Filters
{
    public class RequireSecureConnectionFilter : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.HttpContext.Request.IsLocal)
            {
                return;
            }

            if (filterContext.HttpContext.Request.IsSecureConnection)
            {
                filterContext.HttpContext.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");
            }

            base.OnAuthorization(filterContext);
        }
    }
}