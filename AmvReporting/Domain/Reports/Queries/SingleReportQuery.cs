using System;
using System.Linq;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.ReportDetails.Queries
{
    public class SingleReportQuery : IQuery<Report>
    {
        public string LinkName { get; set; }

        public SingleReportQuery(String linkName)
        {
            LinkName = linkName;
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
            var report = ravenSession
                .Query<Report>()
                .SingleOrDefault(r => r.LinkName == query.LinkName);

            return report;
        }
    }
}