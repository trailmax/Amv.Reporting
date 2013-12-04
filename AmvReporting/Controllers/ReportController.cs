using System;
using System.Web.Mvc;
using AmvReporting.Domain.ReportDetails.Commands;
using AmvReporting.Domain.ReportDetails.Queries;
using AmvReporting.Domain.ReportDetails.ViewModels;
using AmvReporting.Infrastructure;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Controllers
{
    public partial class ReportController : BaseController
    {
        private readonly IMediator mediator;

        public ReportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public virtual ActionResult Index()
        {
            var reports = mediator.Request(new AllReportsQuery());

            return View(reports);
        }


        [RestoreModelState]
        public virtual ActionResult Create()
        {
            return EnrichedView(new ReportDetailsViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Create(CreateReportCommand command)
        {
            return ProcessForm(command, RedirectToAction(MVC.Report.Create()), RedirectToAction(MVC.Report.Index()));
        }


        [RestoreModelState]
        public virtual ActionResult Edit(String linkName)
        {
            var query = new SingleReportQuery(linkName);

            var report = mediator.Request(query);

            return AutoMappedView<EditReportDetailsViewModel>(report);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(EditReportCommand command)
        {
            return ProcessForm(command, RedirectToAction(MVC.Report.Edit(command.LinkName)), RedirectToAction(MVC.Report.Index()));
        }


        [HttpPost]
        public virtual ActionResult Delete(DeleteReportCommand command)
        {
            return ProcessJsonForm(command, "Report Deleted");
        }
    }
}