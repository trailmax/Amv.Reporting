using System.Web.Mvc;
using AmvReporting.Domain.ReportingConfigs.Commands;
using AmvReporting.Domain.ReportingConfigs.Queries;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Filters;


namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public partial class GlobalConfigController : BaseController
    {
        private readonly IMediator mediator;


        public GlobalConfigController(IMediator mediator)
        {
            this.mediator = mediator;
        }

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

            return ProcessCommand(command, View(command), RedirectToAction(MVC.GlobalConfig.Index()));
        }


        public virtual PartialViewResult GlobalCss()
        {
            var config = mediator.Request(new ReportingConfigQuery());

            return PartialView(config);
        }

        public virtual PartialViewResult GlobalJs()
        {
            var config = mediator.Request(new ReportingConfigQuery());

            return PartialView(config);
        }
	}
}