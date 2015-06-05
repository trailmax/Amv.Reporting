using System.Collections.Generic;
using System.Linq;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.Reports.Queries
{
    public class AllReportsQuery : IQuery<IEnumerable<ReportViewModel>>
    {
    }

    public class AllReportsQueryHandler : IQueryHandler<AllReportsQuery, IEnumerable<ReportViewModel>>
    {
        private readonly IDocumentSession ravenSession;

        public AllReportsQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public IEnumerable<ReportViewModel> Handle(AllReportsQuery query)
        {
            var reports = ravenSession.Query<ReportViewModel>().Take(int.MaxValue)
                .ToList();

            return reports;
        }
    }
}