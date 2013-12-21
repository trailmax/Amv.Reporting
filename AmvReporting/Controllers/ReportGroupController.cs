using System;
using System.Web.Mvc;
using AmvReporting.Domain.ReportGroups.Commands;
using AmvReporting.Domain.ReportGroups.Queries;
using AmvReporting.Domain.ReportGroups.ViewModels;
using AmvReporting.Infrastructure;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Filters;

namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public partial class ReportGroupController : BaseController
    {
        private readonly IMediator mediator;

        public ReportGroupController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [RestoreModelState]
        public virtual ActionResult Create()
        {
            return EnrichedView(new ReportGroupViewModel());
        }


        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Create(CreateReportGroupCommand command)
        {
            return ProcessForm(command, RedirectToAction(MVC.ReportGroup.Create()),
                RedirectToAction(MVC.Report.Index()));
        }


        [RestoreModelState]
        public virtual ActionResult Edit(String id)
        {
            var model = mediator.Request(new ReportGroupQuery(id));
            return AutoMappedView<ReportGroupViewModel>(model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(EditReportGroupCommand command)
        {
            return ProcessForm(command, 
                RedirectToAction(MVC.ReportGroup.Edit(command.Id)), 
                RedirectToAction(MVC.Report.Index()));
        }


        public virtual ActionResult Reorder(String id)
        {
            var model = mediator.Request(new GroupReorderQuery(id));

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Reorder(ReorderGroupCommand command)
        {
            return ProcessForm(command, 
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