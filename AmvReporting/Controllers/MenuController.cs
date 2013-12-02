using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmvReporting.Controllers
{
    public class MenuController : BaseController
    {
        public PartialViewResult AdminMenu()
        {
            return PartialView();
        }
    }
}