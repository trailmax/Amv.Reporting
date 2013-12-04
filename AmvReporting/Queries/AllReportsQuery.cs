using System.Collections.Generic;
using System.Linq;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Models;
using Raven.Client;

namespace AmvReporting.Queries
{
    public class AllReportsQuery : IQuery<IEnumerable<Report>>
    {
    }

    public class AllReportsQueryHandler : IQueryHandler<AllReportsQuery, IEnumerable<Report>>
    {
        private readonly IDocumentSession ravenSession;

        public AllReportsQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public IEnumerable<Report> Handle(AllReportsQuery query)
        {
            var reports = ravenSession.Query<Report>()
                .ToList();

            return reports;
        }
    }
}