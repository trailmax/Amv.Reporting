using System.Collections.Generic;
using System.Linq;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Models;
using Raven.Client;

namespace AmvReporting.Queries
{
    public class AllDatabasesQuery : IQuery<IEnumerable<DatabaseDetails>>
    {
    }

    public class AllDatabasesQueryHandler : IQueryHandler<AllDatabasesQuery, IEnumerable<DatabaseDetails>>
    {
        private readonly IDocumentSession ravenSession;

        public AllDatabasesQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public IEnumerable<DatabaseDetails> Handle(AllDatabasesQuery query)
        {
            var allDatabases = ravenSession.Query<DatabaseDetails>().ToList();

            return allDatabases;
        }
    }
}