using System;
using System.Web.Mvc;
using AmvReporting.Domain.Menus;
using AmvReporting.Domain.ReportingConfigs.Queries;
using AmvReporting.Domain.Reports.Queries;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Controllers
{
    public partial class HomeController : BaseController
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        public virtual ActionResult Index()
        {
            var model = mediator.Request(new MenuModelQuery());
            return View(model);
        }


        public virtual ActionResult Report(Guid id)
        {
            var query = new ReportResultQuery(id);
            var result = mediator.Request(query);

            var config = mediator.Request(new ReportingConfigQuery());
            result.GlobalCss = config.GlobalCss;
            result.GlobalJs = config.GlobalJavascript;

            return View(result);
        }
    }
}