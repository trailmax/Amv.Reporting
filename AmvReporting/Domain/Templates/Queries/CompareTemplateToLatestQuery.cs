using System;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using CommonDomain.Persistence;
using Newtonsoft.Json;


namespace AmvReporting.Domain.Templates.Queries
{
    public class CompareTemplateToLatestViewModel
    {
        public Guid AggregateId { get; set; }

        public String LatestJson { get; set; }

        public String RevisionJson { get; set; }

        public int RevisionNumber { get; set; }

        public String Title { get; set; }
    }

    public class CompareTemplateToLatestQuery : IQuery<CompareTemplateToLatestViewModel>
    {
        public CompareTemplateToLatestQuery(Guid aggregateId, int revisionNumber)
        {
            AggregateId = aggregateId;
            RevisionNumber = revisionNumber;
        }


        public Guid AggregateId { get; private set; }
        public int RevisionNumber { get; private set; }
    }


    public class CompareTemplateToLatestQueryHandler : IQueryHandler<CompareTemplateToLatestQuery, CompareTemplateToLatestViewModel>
    {
        private readonly IRepository repository;

        public CompareTemplateToLatestQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }


        public CompareTemplateToLatestViewModel Handle(CompareTemplateToLatestQuery query)
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new TypeOnlyContractResolver<TemplateAggregate>(),
            };

            var latest = repository.GetById<TemplateAggregate>(query.AggregateId);
            var latestJson = JsonConvert.SerializeObject(latest, Formatting.Indented, serializerSettings);

            var revision = repository.GetById<TemplateAggregate>(query.AggregateId, query.RevisionNumber);
            var revisionJson = JsonConvert.SerializeObject(revision, Formatting.Indented, serializerSettings);



            var viewmodel = new CompareTemplateToLatestViewModel()
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