using System.Collections.Generic;
using System.Linq;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.DatabaseConnections.Queries
{
    public class AllDatabasesQuery : IQuery<IEnumerable<DatabaseConnection>>
    {
    }

    public class AllDatabasesQueryHandler : IQueryHandler<AllDatabasesQuery, IEnumerable<DatabaseConnection>>
    {
        private readonly IDocumentSession ravenSession;

        public AllDatabasesQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public IEnumerable<DatabaseConnection> Handle(AllDatabasesQuery query)
        {
            var allDatabases = ravenSession.Query<DatabaseConnection>().Take(int.MaxValue).ToList();

            return allDatabases;
        }
    }
}