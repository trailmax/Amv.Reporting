using System.Web.Mvc;
using AmvReporting.Domain.Menus;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Controllers
{
    public partial class MenuController : BaseController
    {
        private readonly IMediator mediator;

        public MenuController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public virtual PartialViewResult AdminMenu()
        {
            return PartialView();
        }

        public virtual ViewResult EditMenu()
        {
            var model = mediator.Request(new MenuModelQuery());

            return View(model);
        }
    }
}