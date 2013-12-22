using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmvReporting.Domain;
using AmvReporting.Infrastructure.Configuration;

namespace AmvReporting.Infrastructure.Filters
{
    public class RoleAuthorizeFilter : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
#if DEBUG
            return true;
#endif

            var roleNames = ConfigurationContext.Current.AdministratorRoleNames();
            if (String.IsNullOrEmpty(roleNames))
            {
                throw new DomainException("Your administrators roles are not set. Please add role name to your web.config/appSettings/AdministratorRoleNames");
            }

            var roles = roleNames.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);

            return roles.Any(r => httpContext.User.IsInRole(r.Trim()));
        }
    }
}