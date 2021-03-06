﻿using System;
using System.Web.Mvc;
using AmvReporting.Domain.EventSourcing;
using AmvReporting.Domain.Templates;
using AmvReporting.Domain.Templates.Commands;
using AmvReporting.Domain.Templates.Queries;
using AmvReporting.Infrastructure.Filters;
using CommonDomain.Persistence;


namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public partial class TemplateController : BaseController
    {
        private readonly IRepository repository;

        public TemplateController(IRepository repository)
        {
            this.repository = repository;
        }


        public virtual ActionResult Index()
        {
            return QueriedView(new AllTemplatesQuery());
        }


        public virtual ActionResult Create()
        {
            return View(new CreateTemplateCommand());
        }


        [HttpPost]
        public virtual ActionResult Create(CreateTemplateCommand command)
        {
            command.AggregateId = Guid.NewGuid();
            return ProcessCommand(command, View(command), RedirectToAction(MVC.Template.Index()));
        }


        public virtual ActionResult Update(Guid id)
        {
            var template = repository.GetById<TemplateAggregate>(id);
            return MappedView<UpdateTemplateCommand>(template);
        }


        [HttpPost]
        public virtual ActionResult Update(UpdateTemplateCommand command)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                return ProcessJsonForm(command, "Changes are saved");
            }

            return ProcessCommand(command, View(command), RedirectToAction(MVC.Template.Index()));
        }


        public virtual ActionResult ViewRevisions(Guid id)
        {
            var template = repository.GetById<TemplateAggregate>(id);
            ViewBag.Header = template.Title;
            return QueriedView(new AllAggregateRevisionsQuery(id));
        }


        public virtual ActionResult CompareToLatest(Guid id, int revisionNumber)
        {
            return QueriedView(new CompareTemplateToLatestQuery(id, revisionNumber));
        }
    }
}