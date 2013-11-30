using System.Web.Mvc;
using AmvReporting.Commands;

namespace AmvReporting.Controllers
{
    public class AdminController : BaseController
    {
        public ActionResult CreateReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateReport(CreateReportCommand command)
        {
            return ProcessForm(command, RedirectToAction("Index", "Home"));
        }
    }
}