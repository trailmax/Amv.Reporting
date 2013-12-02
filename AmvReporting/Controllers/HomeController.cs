using System;
using System.Web.Mvc;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Models;
using AmvReporting.Queries;

namespace AmvReporting.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ActionResult Index()
        {
            var reports = mediator.Request(new AllReportsQuery());

            return View(reports);
        }


        public ActionResult Report(String linkName)
        {
            var query = new ReportResultQuery(linkName);
            var result = mediator.Request(query);

            if (result.ReportType == ReportType.Table)
            {
                return View("Table", result);
            }
            return View(result);
        }
    }
}