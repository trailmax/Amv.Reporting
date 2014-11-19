using System;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;


namespace AmvReporting.Domain.Reports.Queries
{
    public class SingleReportQuery : IQuery<ReportAggregate>
    {
        public Guid AggregateId { get; set; }

        public SingleReportQuery(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }

    public class SingleReportQueryHandler : IQueryHandler<SingleReportQuery, ReportAggregate>
    {
        private readonly IRepository repository;


        public SingleReportQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }


        public ReportAggregate Handle(SingleReportQuery query)
        {
            var report = repository.GetById<ReportAggregate>(query.AggregateId);

            return report;
        }
    }
}