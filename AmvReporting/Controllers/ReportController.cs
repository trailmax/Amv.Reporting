using System;
using System.Web.Mvc;
using AmvReporting.Domain.Menus;
using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Reports.Commands;
using AmvReporting.Domain.Reports.Queries;
using AmvReporting.Domain.Reports.ViewModels;
using AmvReporting.Infrastructure;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Filters;
using CommonDomain.Persistence;
using Newtonsoft.Json;


namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public partial class ReportController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IRepository repository;

        public ReportController(IMediator mediator, IRepository repository)
        {
            this.mediator = mediator;
            this.repository = repository;
        }


        public virtual ActionResult Index()
        {
            var model = mediator.Request(new MenuModelQuery(true));

            return View(model);
        }


        [RestoreModelState]
        public virtual ActionResult Create()
        {
            var model = new ReportDetailsViewModel()
                        {
                            Enabled = true,
                        };
            return EnrichedView(model);
        }




        [HttpPost]
        public virtual ActionResult Create(CreateReportCommand command)
        {
            return ProcessForm(command, RedirectToAction(MVC.Report.Create()), id => RedirectToAction(MVC.Report.Edit(id)));
        }


        public virtual ActionResult ShowAggregate(Guid id)
        {
            var reportAggregate = repository.GetById<ReportAggregate>(id);

            ViewBag.Json = JsonConvert.SerializeObject(reportAggregate, Formatting.Indented);

            return View();
        }


        [RestoreModelState]
        public virtual ActionResult Edit(Guid id)
        {
            var query = new SingleReportQuery(id);

            var report = mediator.Request(query);

            return AutoMappedView<EditReportDetailsViewModel>(report);
        }


        [HttpPost]
        public virtual ActionResult Edit(EditReportCommand command)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                return ProcessJsonForm(command, "Changes are saved");
            }
            return ProcessForm(command, RedirectToAction(MVC.Report.Edit(command.Id)), RedirectToAction(MVC.Report.Index()));
        }



        public virtual ActionResult Clone(Guid id)
        {
            throw new NotImplementedException();

            //var query = new SingleReportQuery(id);

            //var report = mediator.Request(query);

            //return AutoMappedView<ReportDetailsViewModel>(MVC.Report.Views.Create, report);
        }

        [HttpPost]
        public virtual ActionResult Delete(DeleteReportCommand command)
        {
            return ProcessJsonForm(command, "Report Deleted");
        }
    }
}