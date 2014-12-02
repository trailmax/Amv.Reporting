using NEventStore;
using NEventStore.Persistence.RavenDB;
using NEventStore.Serialization;
using Raven.Client;


namespace AmvReporting.Infrastructure.NEventStore
{
    public class RavenEmbeddedPersistenceWireup : PersistenceWireup
    {
        public RavenEmbeddedPersistenceWireup(Wireup wireup, IDocumentStore documentStore)
            : base(wireup)
        {
            Container.Register(c => new EmbeddedRavenStorageFractory(documentStore, new DocumentObjectSerializer(), new RavenPersistenceOptions()).Build());
        }
        
        public RavenEmbeddedPersistenceWireup(Wireup wireup, IDocumentStore documentStore, RavenPersistenceOptions persistenceOptions)
            : base(wireup)
        {
            Container.Register(c => new EmbeddedRavenStorageFractory(documentStore, new DocumentObjectSerializer(), persistenceOptions).Build());
        }
    }
}