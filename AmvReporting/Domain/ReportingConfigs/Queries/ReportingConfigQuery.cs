using System;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.ReportingConfigs.Queries
{
    public class ReportingConfigQuery : IQuery<ReportingConfig>
    {
    }


    public class ReportingConfigQueryHandler : IQueryHandler<ReportingConfigQuery, ReportingConfig>
    {
        private readonly IDocumentSession documentSession;

        public ReportingConfigQueryHandler(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public ReportingConfig Handle(ReportingConfigQuery query)
        {
            ReportingConfig reportingConfig = null;

            using (documentSession.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(5)))
            {
                reportingConfig = documentSession.Load<ReportingConfig>(ReportingConfig.IdString);
            }

            reportingConfig = reportingConfig ?? ReportingConfig.New();

            return reportingConfig;
        }
    }
}