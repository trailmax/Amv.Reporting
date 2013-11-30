using System.Web.Mvc;

namespace AmvReporting.Controllers
{
    public class AdminController : BaseController
    {
        public ActionResult CreateGraph()
        {
            return View();
        }
    }
}