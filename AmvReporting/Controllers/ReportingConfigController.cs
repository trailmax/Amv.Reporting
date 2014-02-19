using System.Web.Mvc;
using AmvReporting.Domain.ReportingConfigs.Commands;
using AmvReporting.Domain.ReportingConfigs.Queries;
using AmvReporting.Infrastructure;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Filters;

namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public partial class ReportingConfigController : BaseController
    {
        private readonly IMediator mediator;

        public ReportingConfigController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [RestoreModelState]
        public virtual ActionResult Index()
        {
            var appConfig = mediator.Request(new ReportingConfigQuery());

            return View(appConfig);
        }


        [HttpPost]
        public virtual ActionResult Index(UpdateConfigurationCommand command)
        {
            return ProcessForm(command, 
                RedirectToAction(MVC.ReportingConfig.Index()),
                RedirectToAction(MVC.Report.Index()));
        }
	}
}