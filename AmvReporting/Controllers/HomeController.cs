using System;
using System.Web.Mvc;
using AmvReporting.Domain.ReportDetails;
using AmvReporting.Domain.ReportDetails.Queries;
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
            var reports = mediator.Request(new AllReportsQuery());

            return View(reports);
        }


        public virtual ActionResult Report(String linkName)
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