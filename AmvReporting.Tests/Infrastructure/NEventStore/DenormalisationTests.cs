using System;
using System.Linq;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.Autofac;
using AmvReporting.Infrastructure.Caching;
using AmvReporting.Infrastructure.Configuration;
using AmvReporting.Tests.ZeroFriction;
using Autofac;
using CommonDomain.Persistence;
using Ploeh.AutoFixture;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Xunit;


namespace AmvReporting.Tests.Infrastructure.NEventStore
{
    public class DenormalisationTests
    {
        private readonly IContainer container;
        private readonly IFixture fixture;

        public DenormalisationTests()
        {
            ConfigurationContext.Current = new StubDomainConfiguration();

            fixture = new Fixture(new GreedyEngineParts());

            container = AutofacConfig.Configure();
            var builder = new ContainerBuilder();
            builder.RegisterInstance(GetEmbededStorage()).As<IDocumentStore>().SingleInstance();
            builder.RegisterInstance(NSubstitute.Substitute.For<ICacheProvider>()).As<ICacheProvider>();
            builder.Update(container);
        }


        [Fact]
        public void Report_Is_Stored()
        {
            // Arrange
            var repository = container.Resolve<IRepository>();
            var aggregate = fixture.Create<ReportAggregate>();

            // Act
            var commitId = Guid.NewGuid();
            repository.Save(aggregate, commitId);

            // Assert
            var newReport = repository.GetById<ReportAggregate>(aggregate.Id);
            AssertionHelpers.PropertiesAreEqual(aggregate, newReport);
        }


        [Fact]
        public void Denormalised_Model_IsCreated()
        {
            // Arrange
            var repository = container.Resolve<IRepository>();
            var documentSession = container.Resolve<IDocumentSession>();
            var aggregate = fixture.Create<ReportAggregate>();

            // Act
            repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var viewModel = documentSession.Query<ReportViewModel>().FirstOrDefault(r => r.AggregateId == aggregate.Id);
            AssertionHelpers.PropertiesAreEqual(aggregate, viewModel, "Id");
        }


        [Fact]
        public void UpdateCode_DenormalisedModel_RepeatsAggregate()
        {
            // Arrange
            var repository = container.Resolve<IRepository>();
            var documentSession = container.Resolve<IDocumentSession>();
            var aggregate = fixture.Create<ReportAggregate>();
            aggregate.UpdateCode(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());


            // Act
            repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var viewModel = documentSession.Query<ReportViewModel>().First(r => r.AggregateId == aggregate.Id);
            Assert.Equal(aggregate.Sql, viewModel.Sql);
            Assert.Equal(aggregate.JavaScript, viewModel.JavaScript);
            Assert.Equal(aggregate.Css, viewModel.Css);
            Assert.Equal(aggregate.HtmlOverride, viewModel.HtmlOverride);
        }


        [Fact]
        public void UpdateMetadata_DenormalisedModel_RepeatsAggregate()
        {
            // Arrange
            var repository = container.Resolve<IRepository>();
            var documentSession = container.Resolve<IDocumentSession>();
            var aggregate = fixture.Create<ReportAggregate>();
            aggregate.UpdateMetadata(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), ReportType.LineChartWithSelection, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());


            // Act
            repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var viewModel = documentSession.Query<ReportViewModel>().FirstOrDefault(r => r.AggregateId == aggregate.Id);
            AssertionHelpers.PropertiesAreEqual(aggregate, viewModel, "Id");
        }


        [Fact]
        public void EnableReport_DenormalisedModel_IsEnabled()
        {
            // Arrange
            var repository = container.Resolve<IRepository>();
            var documentSession = container.Resolve<IDocumentSession>();
            var aggregate = fixture.Create<ReportAggregate>();


            // Act
            aggregate.EnableReport();
            repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var viewModel = documentSession.Query<ReportViewModel>().First(r => r.AggregateId == aggregate.Id);
            Assert.True(viewModel.Enabled);
        }


        [Fact]
        public void DisableReport_DenormalisedModel_IsDisabled()
        {
            // Arrange
            var repository = container.Resolve<IRepository>();
            var documentSession = container.Resolve<IDocumentSession>();
            var aggregate = fixture.Create<ReportAggregate>();


            // Act
            aggregate.DisableReport();
            repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var viewModel = documentSession.Query<ReportViewModel>().First(r => r.AggregateId == aggregate.Id);
            Assert.False(viewModel.Enabled);
        }



        [Fact]
        public void SetListOrder_DenormalisedModel_HasSameListOrder()
        {
            // Arrange
            var repository = container.Resolve<IRepository>();
            var documentSession = container.Resolve<IDocumentSession>();
            var aggregate = fixture.Create<ReportAggregate>();


            // Act
            aggregate.SetListOrder(45);
            repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var viewModel = documentSession.Query<ReportViewModel>().First(r => r.AggregateId == aggregate.Id);
            Assert.Equal(aggregate.ListOrder, viewModel.ListOrder);
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
            container.Dispose();
        }
    }
}
