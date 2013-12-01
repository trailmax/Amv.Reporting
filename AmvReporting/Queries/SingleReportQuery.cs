using System;
using System.Linq;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Models;
using Raven.Client;

namespace AmvReporting.Queries
{
    public class SingleReportQuery : IQuery<ReportDetails>
    {
        public string LinkName { get; set; }

        public SingleReportQuery(String linkName)
        {
            LinkName = linkName;
        }
    }

    public class SingleReportQueryHandler : IQueryHandler<SingleReportQuery, ReportDetails>
    {
        private readonly IDocumentSession ravenSession;

        public SingleReportQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public ReportDetails Handle(SingleReportQuery query)
        {
            var report = ravenSession
                .Query<ReportDetails>()
                .SingleOrDefault(r => r.LinkName == query.LinkName);

            return report;
        }
    }
}