using System;
using System.Web.Mvc;
using AmvReporting.Domain.EventSourcing;
using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Reports.Queries;
using CommonDomain.Persistence;


namespace AmvReporting.Controllers
{
    public partial class ReportRevisionsController : BaseController
    {
        private readonly IRepository repository;


        public ReportRevisionsController(IRepository repository)
        {
            this.repository = repository;
        }


        public virtual ActionResult ViewAllVersions(Guid id)
        {
            return View(new AllAggregateRevisionsQuery(id));
        }


        public virtual ActionResult ViewRevision(Guid id, int revisionNumber)
        {
            var report = repository.GetById<ReportAggregate>(id, revisionNumber);

            return View(report);
        }


        public virtual ActionResult CompareToLatest(Guid id, int revisionNumber)
        {
            return View(new CompareReportToLatestQuery(id, revisionNumber));
        }
    }
}