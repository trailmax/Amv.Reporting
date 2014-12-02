using AmvReporting.Infrastructure.Autofac;
using AmvReporting.Infrastructure.Caching;
using AmvReporting.Infrastructure.Configuration;
using Autofac;
using CommonDomain.Persistence;
using Ploeh.AutoFixture;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;


namespace AmvReporting.Tests.ZeroFriction
{
    public abstract class IntegrationTestsBase
    {
        internal readonly IContainer Container;
        internal readonly IFixture Fixture;
        internal readonly IRepository Repository;
        internal readonly IDocumentSession DocumentSession;
        internal readonly IDocumentStore DocumentStore;

        public IntegrationTestsBase()
        {
            ConfigurationContext.Current = new StubDomainConfiguration();

            Fixture = new Fixture(new GreedyEngineParts());

            Container = AutofacConfig.Configure();
            var builder = new ContainerBuilder();
            builder.RegisterInstance(GetEmbededStorage()).As<IDocumentStore>().SingleInstance();
            builder.RegisterInstance(NSubstitute.Substitute.For<ICacheProvider>()).As<ICacheProvider>();
            builder.Update(Container);

            Repository = Container.Resolve<IRepository>();
            DocumentStore = Container.Resolve<IDocumentStore>();
            DocumentSession = DocumentStore.OpenSession();
        }

        private static EmbeddableDocumentStore GetEmbededStorage()
        {
            var embeddableDocumentStore = new EmbeddableDocumentStore { RunInMemory = true };

            embeddableDocumentStore.Conventions.DefaultQueryingConsistency =
                ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite;

            embeddableDocumentStore.Initialize();

            return embeddableDocumentStore;
        }


        public void Dispose()
        {
            DocumentSession.Dispose();
            Container.Dispose();
        }
    }
}