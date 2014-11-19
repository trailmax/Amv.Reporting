using System;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;
using Raven.Client;

namespace AmvReporting.Domain.Reports.Queries
{
    public class SingleReportQuery : IQuery<ReportAggregate>
    {
        public Guid AggregateId { get; set; }

        public SingleReportQuery(Guid aggregateId)
        {
            AggregateId = AggregateId;
        }
    }

    public class SingleReportQueryHandler : IQueryHandler<SingleReportQuery, ReportAggregate>
    {
        private readonly IDocumentSession ravenSession;
        private readonly IRepository repository;


        public SingleReportQueryHandler(IDocumentSession ravenSession, IRepository repository)
        {
            this.ravenSession = ravenSession;
            this.repository = repository;
        }


        public ReportAggregate Handle(SingleReportQuery query)
        {
            var report = repository.GetById<ReportAggregate>(query.AggregateId);

            return report;
        }
    }
}