using System;
using System.Web.Mvc;
using AmvReporting.Commands;
using AmvReporting.Infrastructure;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Queries;
using AmvReporting.ViewModels;

namespace AmvReporting.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IMediator mediator;

        public AdminController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ActionResult Index()
        {
            var reports = mediator.Request(new AllReportsQuery());

            return View(reports);
        }


        [RestoreModelState]
        public ActionResult CreateReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateReport(CreateReportCommand command)
        {
            return ProcessForm(command, RedirectToAction("CreateReport"), RedirectToAction("Index", "Admin"));
        }


        [RestoreModelState]
        public ActionResult EditReport(String linkName)
        {
            var query = new SingleReportQuery(linkName);

            var report = mediator.Request(query);

            return AutoMappedView<EditReportDetailsViewModel>(report);
        }


        [HttpPost]
        public ActionResult EditReport(EditReportCommand command)
        {
            return ProcessForm(command, RedirectToAction("EditReport", new { LinkName = command.LinkName}), RedirectToAction("Index"));
        }


        public ActionResult DeleteReport(String linkName)
        {
            var query = new SingleReportQuery(linkName);
            var report = mediator.Request(query);

            return AutoMappedView<EditReportDetailsViewModel>(report);
        }

        [HttpPost]
        public ActionResult DeleteReport(DeleteReportCommand command)
        {
            return ProcessForm(command, RedirectToAction("Index"));
        }
    }
}