using System.Web.Mvc;

namespace AmvReporting.Controllers
{
    public partial class MenuController : BaseController
    {
        public virtual PartialViewResult AdminMenu()
        {
            return PartialView();
        }
    }
}