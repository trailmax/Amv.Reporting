using System.Web.Mvc;
using AmvReporting.Domain.AppConfigs.Queries;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Filters;

namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public class AppConfigController : BaseController
    {
        private readonly IMediator mediator;

        public AppConfigController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ActionResult Index()
        {
            var appConfig = mediator.Request(new AppConfigQuery());

            return View(appConfig);
        }
	}
}