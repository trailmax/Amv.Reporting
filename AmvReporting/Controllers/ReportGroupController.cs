using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AmvReporting.Domain.ReportGroups.Commands;
using AmvReporting.Domain.ReportGroups.Queries;
using AmvReporting.Domain.ReportGroups.ViewModels;
using AmvReporting.Infrastructure;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Controllers
{
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
            var report = mediator.Request(new ReportGroupQuery(id));
            return AutoMappedView<ReportGroupViewModel>(report);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(EditReportGroupCommand command)
        {
            return ProcessForm(command, RedirectToAction(MVC.ReportGroup.Edit(command.Id)), 
                RedirectToAction(MVC.Report.Index()));
        }

        [HttpPost]
        public virtual ActionResult Delete(String id)
        {
            var command = new DeleteReportGroupCommand()
                                               {
                                                   Id = id,
                                               };
            return ProcessJsonForm(command, "Group Deleted");
        }
	}
}