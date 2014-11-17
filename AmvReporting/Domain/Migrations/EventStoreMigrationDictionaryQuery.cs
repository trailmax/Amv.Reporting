using AmvReporting.Infrastructure.CQRS;
using Raven.Client;


namespace AmvReporting.Domain.Migrations
{
    public class EventStoreMigrationDictionaryQuery : IQuery<EventStoreMigrationDictonary>
    {
    }


    public class EventStoreMigrationDictionaryQueryHandler : IQueryHandler<EventStoreMigrationDictionaryQuery, EventStoreMigrationDictonary>
    {
        private readonly IDocumentSession documentSession;


        public EventStoreMigrationDictionaryQueryHandler(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }


        public EventStoreMigrationDictonary Handle(EventStoreMigrationDictionaryQuery query)
        {
            var result = documentSession.Load<EventStoreMigrationDictonary>(EventStoreMigrationDictonary.DefaultId);

            result = result ?? new EventStoreMigrationDictonary();

            return result;
        }
    }
}