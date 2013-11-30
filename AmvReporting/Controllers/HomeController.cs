using System.Web.Mvc;

namespace AmvReporting.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}