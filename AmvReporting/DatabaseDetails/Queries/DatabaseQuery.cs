using System;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Models;
using Raven.Client;

namespace AmvReporting.Queries
{
    public class DatabaseQuery : IQuery<DatabaseConnection>
    {
        public string Id { get; set; }

        public DatabaseQuery(String id)
        {
            Id = id;
        }
    }


    public class DatabaseQueryHandler : IQueryHandler<DatabaseQuery, DatabaseConnection>
    {
        private readonly IDocumentSession ravenSession;

        public DatabaseQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public DatabaseConnection Handle(DatabaseQuery query)
        {
            var databaseDetails = ravenSession.Load<DatabaseConnection>(query.Id);

            return databaseDetails;
        }
    }
}