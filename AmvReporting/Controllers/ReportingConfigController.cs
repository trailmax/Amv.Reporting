using System.Web.Mvc;
using AmvReporting.Domain.ReportingConfigs.Commands;
using AmvReporting.Domain.ReportingConfigs.Queries;
using AmvReporting.Infrastructure.Filters;


namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public partial class ReportingConfigController : BaseController
    {
        public virtual ActionResult Index()
        {
            return QueriedView(new ReportingConfigQuery()).MapTo<UpdateConfigurationCommand>();
        }


        [HttpPost]
        public virtual ActionResult Index(UpdateConfigurationCommand command)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                return ProcessJsonForm(command, "Configuration Updated");
            }

            return ProcessCommand(command, View(command), RedirectToAction(MVC.ReportingConfig.Index()));
        }
	}
}