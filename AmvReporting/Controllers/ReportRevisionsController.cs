using System;
using System.Web.Mvc;
using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Reports.Queries;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;
using NEventStore;
using Newtonsoft.Json;


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
            var report = repository.GetById<ReportAggregate>(Bucket.Default, id, revisionNumber);

            return View(report);
        }


        public virtual ActionResult CompareToLatest(Guid id, int revisionNumber)
        {
            var latest = repository.GetById<ReportAggregate>(Bucket.Default, id, int.MaxValue);

            var revision = repository.GetById<ReportAggregate>(Bucket.Default, id, revisionNumber);

            var serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var viewmodel = new CompareToLatestViewModel()
            {
                AggregateId = id,
                Latest = latest,
                LatestJson = JsonConvert.SerializeObject(latest, Formatting.Indented, serializerSettings),
                Revision = revision,
                RevisionJson = JsonConvert.SerializeObject(revision, Formatting.Indented, serializerSettings),
                RevisionNumber = revisionNumber,
            };

            return View(viewmodel);
        }
    }


    public class CompareToLatestViewModel
    {
        public Guid AggregateId { get; set; }

        public ReportAggregate Latest { get; set; }
        public String LatestJson { get; set; }

        public ReportAggregate Revision { get; set; }
        public String RevisionJson { get; set; }

        public int RevisionNumber { get; set; }
    }
}