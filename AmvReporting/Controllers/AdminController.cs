using System;
using System.Web.Mvc;
using AmvReporting.Commands;

namespace AmvReporting.Controllers
{
    public class AdminController : BaseController
    {
        public ActionResult CreateGraph()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateGraph(CreateGraphCommand command)
        {
            return ProcessForm(command, RedirectToAction("Index", "Home"));
        }
    }
}