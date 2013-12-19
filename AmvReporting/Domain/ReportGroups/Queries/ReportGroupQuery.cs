using System;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.ReportGroups.Queries
{
    public class ReportGroupQuery : IQuery<ReportGroup>
    {
        public string Id { get; set; }

        public ReportGroupQuery(String id)
        {
            Id = id;
        }
    }

    public class ReportGroupQueryHandler : IQueryHandler<ReportGroupQuery, ReportGroup>
    {
        private readonly IDocumentSession ravenSession;

        public ReportGroupQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public ReportGroup Handle(ReportGroupQuery query)
        {
            var requestedGroup = ravenSession.Load<ReportGroup>(query.Id);

            return requestedGroup;
        }
    }
}