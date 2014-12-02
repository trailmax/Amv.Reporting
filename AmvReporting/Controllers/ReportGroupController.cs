using System;
using System.Web.Mvc;
using AmvReporting.Domain.ReportGroups.Commands;
using AmvReporting.Domain.ReportGroups.Queries;
using AmvReporting.Infrastructure.Filters;


namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public partial class ReportGroupController : BaseController
    {
        public virtual ActionResult Create()
        {
            var command = new CreateReportGroupCommand()
                              {
                                  Enabled = true,
                              };
            return View(command);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Create(CreateReportGroupCommand command)
        {
            return ProcessCommand(command, View(command), RedirectToAction(MVC.Report.Index()));
        }


        public virtual ActionResult Edit(String id)
        {
            return QueriedView(new ReportGroupQuery(id)).MapTo<EditReportGroupCommand>();
        }


        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(EditReportGroupCommand command)
        {
            return ProcessCommand(command, View(command), RedirectToAction(MVC.Report.Index()));
        }


        public virtual ActionResult Reorder(String id)
        {
            return QueriedView(new GroupReorderQuery(id));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Reorder(ReorderGroupCommand command)
        {
            return ProcessCommand(command, 
                RedirectToAction(MVC.ReportGroup.Reorder(command.ParentGroupId)),
                RedirectToAction(MVC.Report.Index()));
        }


        [HttpPost]
        public virtual ActionResult Delete(String id)
        {
            var command = new DeleteReportGroupCommand(id);

            return ProcessJsonForm(command, "Group Deleted");
        }
	}
}