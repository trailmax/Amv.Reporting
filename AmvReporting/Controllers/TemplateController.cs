using System;
using System.Web.Mvc;
using AmvReporting.Domain.Templates;
using AmvReporting.Domain.Templates.Commands;
using AmvReporting.Domain.Templates.Queries;
using CommonDomain.Persistence;


namespace AmvReporting.Controllers
{
    public partial class TemplateController : BaseController
    {
        private readonly IRepository repository;


        public TemplateController(IRepository repository)
        {
            this.repository = repository;
        }

        public virtual ActionResult Index()
        {
            return View(new AllTemplatesQuery());
        }

        public virtual ActionResult Create()
        {
            return View(new CreateTemplateCommand());
        }

        [HttpPost]
        public virtual ActionResult Create(CreateTemplateCommand command)
        {
            command.AggregateID = Guid.NewGuid();
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
            return ProcessCommand(command, View(command), RedirectToAction(MVC.Template.Index()));
        }
    }
}