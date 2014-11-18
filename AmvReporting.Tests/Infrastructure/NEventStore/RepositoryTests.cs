using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.Autofac;
using AmvReporting.Infrastructure.Automappings;
using AmvReporting.Infrastructure.Configuration;
using AmvReporting.Infrastructure.NEventStore;
using AmvReporting.Tests.ZeroFriction;
using Autofac;
using CommonDomain.Persistence;
using NEventStore;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Xunit;


namespace AmvReporting.Tests.Infrastructure.NEventStore
{
    public class RepositoryTests
    {
        private readonly IContainer container;

        public RepositoryTests()
        {
            ConfigurationContext.Current = new StubDomainConfiguration();
            container = AutofacConfig.Configure();
            var builder = new ContainerBuilder();
            var embeddableDocumentStore = new EmbeddableDocumentStore { RunInMemory = true };
            embeddableDocumentStore.Conventions.DefaultQueryingConsistency = ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite;
            embeddableDocumentStore.Initialize();
            builder.RegisterInstance(embeddableDocumentStore).As<IDocumentStore>().SingleInstance();
            builder.Update(container);

            AutoMapperBootstrapper.Initialize();
        }


        [Fact]
        public void Report_Is_Stored()
        {
            var repository = container.Resolve<IRepository>();

            var id = Guid.NewGuid();
            var databaseId = Guid.NewGuid().ToString();
            var reportGroupId = Guid.NewGuid().ToString();
            var title = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var aggregate = new ReportAggregate(id, reportGroupId, title, ReportType.Table, description, databaseId);

            // Act
            var commitId = Guid.NewGuid();
            repository.Save(aggregate, commitId);

            // Assert
            var newReport = repository.GetById<ReportAggregate>(id);
            AssertionHelpers.PropertiesAreEqual(aggregate, newReport);
        }


        [Fact]
        public void Denormalised_Model_IsCreated()
        {
            var repository = container.Resolve<IRepository>();
            var documentSession = container.Resolve<IDocumentSession>();

            var id = Guid.NewGuid();
            var databaseId = Guid.NewGuid().ToString();
            var aggregate = new ReportAggregate(id, "reportGroup", "Title", ReportType.Table, "Description", databaseId);

            var commitId = Guid.NewGuid();
            repository.Save(aggregate, commitId);

            var viewModel = documentSession.Query<ReportViewModel>().First(r => r.AggregateId == id);

            //Assert.Equal(databaseId, viewModel.DatabaseId);
            AssertionHelpers.PropertiesAreEqual(aggregate, viewModel);
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}
