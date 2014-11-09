using System;
using NEventStore;
using NEventStore.Persistence.RavenDB;
using Raven.Client;


namespace AmvReporting.Infrastructure.NEventStore
{
    public static class RavenPersistenceWireupExtensions
    {
        public static RavenEmbeddedPersistenceWireup UsingRavenPersistence(
            this Wireup wireup,
            Func<IDocumentStore> documentStoreDelegeate)
        {
            var documentStore = documentStoreDelegeate.Invoke();
            return new RavenEmbeddedPersistenceWireup(wireup, documentStore);
        }


        public static RavenEmbeddedPersistenceWireup UsingRavenPersistence(
            this Wireup wireup,
            Func<IDocumentStore> documentStoreDelegeate,
            RavenPersistenceOptions options)
        {
            var documentStore = documentStoreDelegeate.Invoke();
            return new RavenEmbeddedPersistenceWireup(wireup, documentStore, options);
        }


        public static RavenEmbeddedPersistenceWireup UsingRavenPersistence(
            this Wireup wireup,
            IDocumentStore documentStore)
        {
            return new RavenEmbeddedPersistenceWireup(wireup, documentStore);
        }


        public static RavenEmbeddedPersistenceWireup UsingRavenPersistence(
            this Wireup wireup,
            IDocumentStore documentStore,
            RavenPersistenceOptions options)
        {
            return new RavenEmbeddedPersistenceWireup(wireup, documentStore, options);
        }
    }
}