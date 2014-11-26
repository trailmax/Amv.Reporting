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

            return MappedView<UpdateConfigurationCommand>(appConfig);
        }


        [HttpPost]
        public virtual ActionResult Index(UpdateConfigurationCommand command)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                return ProcessJsonForm(command, "Configuration Updated");
            }

            if (!ModelState.IsValid)
            {
                return View(command);
            }
            var errors = mediator.ProcessCommand(command);
            AddErrorsToModelState(errors);
            if (!ModelState.IsValid)
            {
                return View(command);
            }

            return RedirectToAction(MVC.ReportingConfig.Index());
        }
	}
}