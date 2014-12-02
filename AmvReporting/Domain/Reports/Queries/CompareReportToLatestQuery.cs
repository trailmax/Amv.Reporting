using System;
using System.Web;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using CommonDomain.Persistence;
using Newtonsoft.Json;


namespace AmvReporting.Domain.Reports.Queries
{
    public class CompareReportToLatestViewModel
    {
        public Guid AggregateId { get; set; }

        public String LatestJson { get; set; }

        public String RevisionJson { get; set; }

        public int RevisionNumber { get; set; }

        public String Title { get; set; }
    }

    public class CompareReportToLatestQuery : IQuery<CompareReportToLatestViewModel>
    {
        public CompareReportToLatestQuery(Guid aggregateId, int revisionNumber)
        {
            AggregateId = aggregateId;
            RevisionNumber = revisionNumber;
        }


        public Guid AggregateId { get; private set; }
        public int RevisionNumber { get; private set; }
    }


    public class CompareReportToLatestQueryHandler : IQueryHandler<CompareReportToLatestQuery, CompareReportToLatestViewModel>
    {
        private readonly IRepository repository;

        public CompareReportToLatestQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }


        public CompareReportToLatestViewModel Handle(CompareReportToLatestQuery query)
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new TypeOnlyContractResolver<ReportAggregate>(),
            };

            var latest = repository.GetById<ReportAggregate>(query.AggregateId);
            var latestJson = JsonConvert.SerializeObject(latest, Formatting.Indented, serializerSettings);

            var revision = repository.GetById<ReportAggregate>(query.AggregateId, query.RevisionNumber);
            var revisionJson = JsonConvert.SerializeObject(revision, Formatting.Indented, serializerSettings);



            var viewmodel = new CompareReportToLatestViewModel()
            {
                AggregateId = query.AggregateId,
                RevisionJson = revisionJson.Unescape(),
                LatestJson = latestJson.Unescape(),
                RevisionNumber = query.RevisionNumber,
                Title = latest.Title,
            };

            return viewmodel;
        }


    }
}