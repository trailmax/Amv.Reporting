using System;
using System.Web.Mvc;
using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Reports.Queries;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Controllers
{
    [AllowAnonymous]
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

            return View(result);
        }
    }
}