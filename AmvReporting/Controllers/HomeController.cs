using System;
using System.Web.Mvc;
using AmvReporting.Domain.Menus;
using AmvReporting.Domain.Migrations;
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
            return View(new MenuModelQuery());
        }


        public virtual ActionResult ReportAggregate(Guid id)
        {
            var result = mediator.Request(new ReportResultQuery(id));

            var config = mediator.Request(new ReportingConfigQuery());
            result.GlobalCss = config.GlobalCss;
            result.GlobalJs = config.GlobalJavascript;

            return View(result);
        }


        public virtual ActionResult Report(string id)
        {
            var migrationDocument = mediator.Request(new EventStoreMigrationDictionaryQuery());
            Guid migrationId;
            if (migrationDocument.TryGetValue(id, out migrationId))
            {
                return RedirectToAction(MVC.Home.ReportAggregate(migrationId));
            }

            return RedirectToAction(MVC.Home.Index());
        }
    }
}