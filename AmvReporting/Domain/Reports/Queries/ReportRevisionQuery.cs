using System;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;
using NEventStore;


namespace AmvReporting.Domain.Reports.Queries
{
    public class ReportRevisionQuery : IQuery<ReportAggregate>
    {
        public ReportRevisionQuery(Guid id, int revisionNumber)
        {
            Id = id;
            RevisionNumber = revisionNumber;
        }


        public Guid Id { get; set; }
        public int RevisionNumber { get; set; }
    }


    public class ReportRevisionQueryHandler : IQueryHandler<ReportRevisionQuery, ReportAggregate>
    {
        private readonly IRepository repository;
        private readonly IStoreEvents storeEvents;


        public ReportRevisionQueryHandler(IRepository repository, IStoreEvents storeEvents)
        {
            this.repository = repository;
            this.storeEvents = storeEvents;
        }


        public ReportAggregate Handle(ReportRevisionQuery query)
        {
            var report = repository.GetById<ReportAggregate>(Bucket.Default, query.Id, query.RevisionNumber);

            return report;
        }
    }
}