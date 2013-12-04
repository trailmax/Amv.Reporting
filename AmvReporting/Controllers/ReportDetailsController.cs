using System;
using System.Web.Mvc;
using AmvReporting.Commands;
using AmvReporting.Infrastructure;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Queries;
using AmvReporting.ViewModels;

namespace AmvReporting.Controllers
{
    public partial class ReportDetailsController : BaseController
    {
        private readonly IMediator mediator;

        public ReportDetailsController(IMediator mediator)
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
            return ProcessForm(command, RedirectToAction(MVC.ReportDetails.Create()), RedirectToAction(MVC.ReportDetails.Index()));
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
            return ProcessForm(command, RedirectToAction(MVC.ReportDetails.Edit(command.LinkName)), RedirectToAction(MVC.ReportDetails.Index()));
        }


        [HttpPost]
        public virtual ActionResult Delete(DeleteReportCommand command)
        {
            return ProcessJsonForm(command, "Report Deleted");
        }
    }
}