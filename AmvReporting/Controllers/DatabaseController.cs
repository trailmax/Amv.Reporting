using System;
using System.Web.Mvc;
using AmvReporting.Domain.DatabaseConnections.Commands;
using AmvReporting.Domain.DatabaseConnections.Queries;
using AmvReporting.Infrastructure;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Filters;
using AutoMapper;


namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
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

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Create(CreateDatabaseDetailsCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }
            var errors = mediator.ProcessCommand(command);
            AddErrorsToModelState(errors);
            if (!ModelState.IsValid)
            {
                return View(command);
            }
            return RedirectToAction(MVC.Database.Index());

            //return ProcessForm(command, RedirectToAction(MVC.Database.Create()), RedirectToAction(MVC.Database.Index()));
        }

        [RestoreModelState]
        public virtual ActionResult Edit(String dbId)
        {
            var database = mediator.Request(new DatabaseQuery(dbId));

            var viewModel = Mapper.Map<EditDatabaseDetailsCommand>(database);

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(EditDatabaseDetailsCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }
            var errors = mediator.ProcessCommand(command);
            AddErrorsToModelState(errors);
            if (!ModelState.IsValid)
            {
                return View(command);
            }

            return RedirectToAction(MVC.Database.Index());
            //return ProcessForm(command, RedirectToAction(MVC.Database.Edit(command.Id)), RedirectToAction(MVC.Database.Index()));
        }


        [HttpPost]
        public virtual ActionResult Delete(DeleteDatabaseCommand command)
        {
            return ProcessJsonForm(command, "Database Detail record deleted");
        }


        [HttpPost]
        public virtual ActionResult CheckDatabaseConnection(String connectionString)
        {
            var result = mediator.Request(new CheckDatabaseConnectivityQuery(connectionString));

            return Json(result);
        }
    }
}