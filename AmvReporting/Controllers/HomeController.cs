using System.Web.Mvc;
using AmvReporting.Infrastructure.CQRS;
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


        public ActionResult Report(int id)
        {
            var query = new ReportResultQuery(id);
            throw new System.NotImplementedException();
        }
    }
}