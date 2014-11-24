using System;
using System.Web.Mvc;
using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Reports.Queries;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;


namespace AmvReporting.Controllers
{
    public partial class ReportRevisionsController : Controller
    {
        private readonly IMediator mediator;
        private readonly IRepository repository;


        public ReportRevisionsController(IMediator mediator, IRepository repository)
        {
            this.mediator = mediator;
            this.repository = repository;
        }


        public virtual ActionResult ViewAllVersions(Guid id)
        {
            var result = mediator.Request(new AllReportRevisionsQuery(id));
            return View(result);
        }


        public virtual ActionResult ViewRevision(Guid id, int revisionNumber)
        {
            var report = repository.GetById<ReportAggregate>(id, revisionNumber);

            return View(report);
        }


        public virtual ActionResult CompareToLatest(Guid id, int revisionNumber)
        {
            var viewmodel = mediator.Request(new CompareToLatestQuery(id, revisionNumber));

            return View(viewmodel);
        }
    }
}