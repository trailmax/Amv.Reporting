using System.Collections.Generic;
using System.Linq;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.ReportGroups.Queries
{
    public class AllReportGroupsQuery : IQuery<IEnumerable<ReportGroup>>
    {
    }

    public class AllReportGroupsQueryHandler : IQueryHandler<AllReportGroupsQuery, IEnumerable<ReportGroup>>
    {
        private readonly IDocumentSession ravenSession;

        public AllReportGroupsQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public IEnumerable<ReportGroup> Handle(AllReportGroupsQuery query)
        {
            var groups = ravenSession.Query<ReportGroup>().Take(int.MaxValue).ToList();

            return groups;
        }
    }
}