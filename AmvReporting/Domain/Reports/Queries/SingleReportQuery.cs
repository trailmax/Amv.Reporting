using System;
using System.Linq;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.Reports.Queries
{
    public class SingleReportQuery : IQuery<Report>
    {
        public string Id { get; set; }

        public SingleReportQuery(String id)
        {
            Id = id;
        }
    }

    public class SingleReportQueryHandler : IQueryHandler<SingleReportQuery, Report>
    {
        private readonly IDocumentSession ravenSession;

        public SingleReportQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public Report Handle(SingleReportQuery query)
        {
            var report = ravenSession.Load<Report>(query.Id);

            return report;
        }
    }
}