using System.Collections.Generic;
using System.Linq;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Models;
using Raven.Client;

namespace AmvReporting.Queries
{
    public class AllReportsQuery : IQuery<IEnumerable<ReportDetails>>
    {
    }

    public class AllReportsQueryHandler : IQueryHandler<AllReportsQuery, IEnumerable<ReportDetails>>
    {
        private readonly IDocumentSession ravenSession;

        public AllReportsQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public IEnumerable<ReportDetails> Handle(AllReportsQuery query)
        {
            var reports = ravenSession.Query<ReportDetails>()
                .ToList();

            return reports;
        }
    }
}