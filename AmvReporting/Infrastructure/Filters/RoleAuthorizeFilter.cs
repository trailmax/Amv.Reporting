using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmvReporting.Infrastructure.Configuration;

namespace AmvReporting.Infrastructure.Filters
{
    public class RoleAuthorizeFilter : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var roleNames = ConfigurationContext.Current.AdministratorRoleNames();
            var roles = roleNames.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);

            return roles.Any(r => httpContext.User.IsInRole(r.Trim()));
        }
    }
}