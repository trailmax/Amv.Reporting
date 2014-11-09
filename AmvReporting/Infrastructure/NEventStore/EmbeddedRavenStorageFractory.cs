using NEventStore.Persistence;
using NEventStore.Persistence.RavenDB;
using NEventStore.Serialization;
using Raven.Client;


namespace AmvReporting.Infrastructure.NEventStore
{
    public class EmbeddedRavenStorageFractory : IPersistenceFactory
    {
        private readonly IDocumentStore documentStore;
        private readonly RavenPersistenceOptions options;
        private readonly IDocumentSerializer serializer;


        public EmbeddedRavenStorageFractory(IDocumentStore documentStore,  IDocumentSerializer serializer, RavenPersistenceOptions options)
        {
            this.documentStore = documentStore;
            this.options = options;
            this.serializer = serializer;
        }


        public IPersistStreams Build()
        {
            return new RavenPersistenceEngine(documentStore, serializer, options);
        }
    }
}