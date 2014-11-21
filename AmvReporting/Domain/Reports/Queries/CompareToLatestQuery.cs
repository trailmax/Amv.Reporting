using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;
using NEventStore;
using Newtonsoft.Json;


namespace AmvReporting.Domain.Reports.Queries
{
    public class CompareToLatestViewModel
    {
        public Guid AggregateId { get; set; }

        public String LatestJson { get; set; }

        public String RevisionJson { get; set; }

        public int RevisionNumber { get; set; }
    }

    public class CompareToLatestQuery : IQuery<CompareToLatestViewModel>
    {
        public CompareToLatestQuery(Guid aggregateId, int revisionNumber)
        {
            AggregateId = aggregateId;
            RevisionNumber = revisionNumber;
        }


        public Guid AggregateId { get; private set; }
        public int RevisionNumber { get; private set; }
    }


    public class CompareToLatestQueryHandler : IQueryHandler<CompareToLatestQuery, CompareToLatestViewModel>
    {
        private readonly IRepository repository;
        private readonly IStoreEvents storeEvents;

        public CompareToLatestQueryHandler(IRepository repository, IStoreEvents storeEvents)
        {
            this.repository = repository;
            this.storeEvents = storeEvents;
        }


        public CompareToLatestViewModel Handle(CompareToLatestQuery query)
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var latest = repository.GetById<ReportAggregate>(query.AggregateId);
            var latestJson = JsonConvert.SerializeObject(latest, Formatting.Indented, serializerSettings);

            var revision = repository.GetById<ReportAggregate>(query.AggregateId, query.RevisionNumber);
            var revisionJson = JsonConvert.SerializeObject(revision, Formatting.Indented, serializerSettings);



            var viewmodel = new CompareToLatestViewModel()
            {
                AggregateId = query.AggregateId,
                RevisionJson = revisionJson,
                LatestJson = latestJson,
                RevisionNumber = query.RevisionNumber,
            };

            return viewmodel;
        }
    }
}