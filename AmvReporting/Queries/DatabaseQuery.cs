using System;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Models;
using Raven.Client;

namespace AmvReporting.Queries
{
    public class DatabaseQuery : IQuery<DatabaseDetails>
    {
        public string Id { get; set; }

        public DatabaseQuery(String id)
        {
            Id = id;
        }
    }


    public class DatabaseQueryHandler : IQueryHandler<DatabaseQuery, DatabaseDetails>
    {
        private readonly IDocumentSession ravenSession;

        public DatabaseQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public DatabaseDetails Handle(DatabaseQuery query)
        {
            var databaseDetails = ravenSession.Load<DatabaseDetails>(query.Id);

            return databaseDetails;
        }
    }
}