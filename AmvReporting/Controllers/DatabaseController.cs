using System;
using System.Web.Mvc;
using AmvReporting.Domain.DatabaseConnections.Commands;
using AmvReporting.Domain.DatabaseConnections.Queries;
using AmvReporting.Infrastructure.Filters;


namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public partial class DatabaseController : BaseController
    {
        public virtual ActionResult Index()
        {
            return QueriedView(new AllDatabasesQuery());
        }


        public virtual ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Create(CreateDatabaseDetailsCommand command)
        {
            return ProcessCommand(command, View(command), RedirectToAction(MVC.Database.Index()));
        }


        public virtual ActionResult Edit(String dbId)
        {
            return QueriedView(new DatabaseQuery(dbId)).MapTo<EditDatabaseDetailsCommand>();
        }


        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(EditDatabaseDetailsCommand command)
        {
            return ProcessCommand(command, View(command), RedirectToAction(MVC.Database.Index()));
        }


        [HttpPost]
        public virtual ActionResult Delete(DeleteDatabaseCommand command)
        {
            return ProcessJsonForm(command, "Database Detail record deleted");
        }


        [HttpPost]
        public virtual ActionResult CheckDatabaseConnection(String connectionString)
        {
            return QueriedView(new CheckDatabaseConnectivityQuery(connectionString)).DoJson();
        }
    }
}