using System;
using System.Web.Mvc;
using AmvReporting.Domain.Menus;
using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Reports.Commands;
using AmvReporting.Infrastructure.Filters;
using CommonDomain.Persistence;


namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public partial class ReportController : BaseController
    {
        private readonly IRepository repository;

        public ReportController(IRepository repository)
        {
            this.repository = repository;
        }


        public virtual ActionResult Index()
        {
            return View(new MenuModelQuery(true));
        }


        public virtual ActionResult Create()
        {
            var model = new CreateReportCommand()
                        {
                            Enabled = true,
                        };
            return View(model);
        }


        [HttpPost]
        public virtual ActionResult Create(CreateReportCommand command)
        {
            command.AggregateId = Guid.NewGuid();
            return ProcessCommand(command, View(command), RedirectToAction(MVC.Report.UpdateCode(command.AggregateId)));
        }



        public virtual ActionResult UpdateMetadata(Guid id)
        {
            var report = repository.GetById<ReportAggregate>(id);

            return MappedView<UpdateReportMetadataCommand>(report);
        }



        [HttpPost]
        public virtual ActionResult UpdateMetadata(UpdateReportMetadataCommand command)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                return ProcessJsonForm(command, "Changes are saved");
            }

            return ProcessCommand(command, View(command), RedirectToAction(MVC.Report.UpdateMetadata(command.AggregateId)));
        }



        public virtual ActionResult UpdateCode(Guid id)
        {
            var report = repository.GetById<ReportAggregate>(id);

            return MappedView<UpdateReportCodeCommand>(report);
        }


        [HttpPost]
        public virtual ActionResult UpdateCode(UpdateReportCodeCommand command)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                return ProcessJsonForm(command, "Changes are saved");
            }

            return ProcessCommand(command, View(command), RedirectToAction(MVC.Report.UpdateMetadata(command.AggregateId)));
        }


        //TODO Restore Clone
        //public virtual ActionResult Clone(Guid id)
        //{
        //    var report = repository.GetById<ReportAggregate>(id);

        //    return MappedView<ReportDetailsViewModel>(MVC.Report.Views.Create, report);
        //}


        [HttpPost]
        public virtual ActionResult Delete(DeleteReportCommand command)
        {
            return ProcessJsonForm(command, "Report Deleted");
        }
    }
}