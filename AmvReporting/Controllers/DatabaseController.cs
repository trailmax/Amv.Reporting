using System;
using System.Web.Mvc;
using AmvReporting.Commands;
using AmvReporting.Infrastructure;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Queries;

namespace AmvReporting.Controllers
{
    public partial class DatabaseController : BaseController
    {
        private readonly IMediator mediator;

        public DatabaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public virtual ActionResult Index()
        {
            var databases = mediator.Request(new AllDatabasesQuery());
            return View(databases);
        }

        [RestoreModelState]
        public virtual ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateDatabaseDetailsCommand command)
        {
            return ProcessForm(command, RedirectToAction(MVC.Database.Create()), RedirectToAction(MVC.Database.Index()));
        }

        public virtual ActionResult Edit(object linkname)
        {
            throw new System.NotImplementedException();
        }

        public virtual ActionResult Delete(string dbid)
        {
            throw new System.NotImplementedException();
        }


        [HttpPost]
        public virtual ActionResult CheckDatabaseConnection(String connectionString)
        {
            var result = mediator.Request(new CheckDatabaseConnectivityQuery(connectionString));

            return Json(result);
        }
    }
}