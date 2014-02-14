using System;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.AppConfigs.Queries
{
    public class AppConfigQuery : IQuery<AppConfig>
    {
    }

    //TODO TESTS
    public class AppConfigQueryHandler : IQueryHandler<AppConfigQuery, AppConfig>
    {
        private readonly IDocumentSession documentSession;

        public AppConfigQueryHandler(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public AppConfig Handle(AppConfigQuery query)
        {
            AppConfig appConfig = null;

            using (documentSession.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(5)))
            {
                appConfig = documentSession.Load<AppConfig>(AppConfig.IdString);
            }

            appConfig = appConfig ?? AppConfig.New();

            return appConfig;
        }
    }
}