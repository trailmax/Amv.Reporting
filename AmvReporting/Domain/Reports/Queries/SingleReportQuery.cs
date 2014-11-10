using System;
using System.Linq;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.Reports.Queries
{
    public class SingleReportQuery : IQuery<ReportViewModel>
    {
        public string Id { get; set; }

        public SingleReportQuery(String id)
        {
            Id = id;
        }
    }

    public class SingleReportQueryHandler : IQueryHandler<SingleReportQuery, ReportViewModel>
    {
        private readonly IDocumentSession ravenSession;

        public SingleReportQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public ReportViewModel Handle(SingleReportQuery query)
        {
            var report = ravenSession.Load<ReportViewModel>(query.Id);

            return report;
        }
    }
}