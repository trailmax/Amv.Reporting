﻿using System;
using System.Web.Mvc;
using AmvReporting.Domain.Menus;
using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Reports.Commands;
using AmvReporting.Domain.Reports.ViewModels;
using AmvReporting.Infrastructure;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Filters;
using CommonDomain.Persistence;


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
            var model = new CreateReportCommand()
                        {
                            Enabled = true,
                        };
            return View(model);
        }


        [HttpPost]
        public virtual ActionResult Create(CreateReportCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }
            var errors = mediator.ProcessCommand(command);
            AddErrorsToModelState(errors);
            if (ModelState.IsValid)
            {
                return View(command);
            }

            return RedirectToAction(MVC.Report.EditCode(command.AggregateId));
        }


        public virtual ActionResult EditCode(Guid id)
        {
            var report = repository.GetById<ReportAggregate>(id);

            return AutoMappedView<EditReportDetailsViewModel>(report);
        }



        //[RestoreModelState]
        //public virtual ActionResult Edit(Guid id)
        //{
        //    var report = repository.GetById<ReportAggregate>(id);

        //    return AutoMappedView<EditReportDetailsViewModel>(report);
        //}


        //[HttpPost]
        //public virtual ActionResult Edit(EditReportCommand command)
        //{
        //    if (HttpContext.Request.IsAjaxRequest())
        //    {
        //        return ProcessJsonForm(command, "Changes are saved");
        //    }
        //    return ProcessForm(command, RedirectToAction(MVC.Report.Edit(command.AggregateId)), RedirectToAction(MVC.Report.Index()));
        //}
        

        public virtual ActionResult Clone(Guid id)
        {
            var report = repository.GetById<ReportAggregate>(id);

            return AutoMappedView<ReportDetailsViewModel>(MVC.Report.Views.Create, report);
        }


        [HttpPost]
        public virtual ActionResult Delete(DeleteReportCommand command)
        {
            return ProcessJsonForm(command, "Report Deleted");
        }
    }
}