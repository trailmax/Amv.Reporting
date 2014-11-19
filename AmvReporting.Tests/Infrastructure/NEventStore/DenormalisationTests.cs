using System;
using System.Linq;
using AmvReporting.Domain.Reports;
using AmvReporting.Tests.ZeroFriction;
using CommonDomain.Persistence;
using Ploeh.AutoFixture;
using Xunit;


namespace AmvReporting.Tests.Infrastructure.NEventStore
{
    public class DenormalisationTests : IntegrationTestsBase
    {
        [Fact]
        public void Report_Is_Stored()
        {
            // Arrange
            var aggregate = Fixture.Create<ReportAggregate>();

            // Act
            Repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var newReport = Repository.GetById<ReportAggregate>(aggregate.Id);
            AssertionHelpers.PropertiesAreEqual(aggregate, newReport);
        }


        [Fact]
        public void Denormalised_Model_IsCreated()
        {
            // Arrange
            var aggregate = Fixture.Create<ReportAggregate>();

            // Act
            Repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var viewModel = DocumentSession.Query<ReportViewModel>().FirstOrDefault(r => r.AggregateId == aggregate.Id);
            AssertionHelpers.PropertiesAreEqual(aggregate, viewModel, "Id");
        }


        [Fact]
        public void UpdateCode_DenormalisedModel_RepeatsAggregate()
        {
            // Arrange
            var aggregate = Fixture.Create<ReportAggregate>();
            aggregate.UpdateCode(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());


            // Act
            Repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var viewModel = DocumentSession.Query<ReportViewModel>().First(r => r.AggregateId == aggregate.Id);
            Assert.Equal(aggregate.Sql, viewModel.Sql);
            Assert.Equal(aggregate.JavaScript, viewModel.JavaScript);
            Assert.Equal(aggregate.Css, viewModel.Css);
            Assert.Equal(aggregate.HtmlOverride, viewModel.HtmlOverride);
        }


        [Fact]
        public void UpdateMetadata_DenormalisedModel_RepeatsAggregate()
        {
            // Arrange
            var aggregate = Fixture.Create<ReportAggregate>();
            aggregate.UpdateMetadata(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), ReportType.LineChartWithSelection, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());


            // Act
            Repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var viewModel = DocumentSession.Query<ReportViewModel>().FirstOrDefault(r => r.AggregateId == aggregate.Id);
            AssertionHelpers.PropertiesAreEqual(aggregate, viewModel, "Id");
        }


        [Fact]
        public void EnableReport_DenormalisedModel_IsEnabled()
        {
            // Arrange
            var aggregate = Fixture.Create<ReportAggregate>();


            // Act
            aggregate.SetReportEnabled(true);
            Repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var viewModel = DocumentSession.Query<ReportViewModel>().First(r => r.AggregateId == aggregate.Id);
            Assert.True(viewModel.Enabled);
        }


        [Fact]
        public void DisableReport_DenormalisedModel_IsDisabled()
        {
            // Arrange
            var aggregate = Fixture.Create<ReportAggregate>();


            // Act
            aggregate.SetReportEnabled(false);
            Repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var viewModel = DocumentSession.Query<ReportViewModel>().First(r => r.AggregateId == aggregate.Id);
            Assert.False(viewModel.Enabled);
        }



        [Fact]
        public void SetListOrder_DenormalisedModel_HasSameListOrder()
        {
            // Arrange
            var aggregate = Fixture.Create<ReportAggregate>();


            // Act
            aggregate.SetListOrder(45);
            Repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var viewModel = DocumentSession.Query<ReportViewModel>().First(r => r.AggregateId == aggregate.Id);
            Assert.Equal(aggregate.ListOrder, viewModel.ListOrder);
        }


        [Fact]
        public void CreationFromMigration_DenormalisedModel_Matches()
        {
            // Arrange
            var id = Guid.NewGuid();
            var migrationModel = Fixture.Create<ReportViewModel>();
            var aggregate = new ReportAggregate(id, migrationModel);


            // Act
            Repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var viewModel = DocumentSession.Query<ReportViewModel>().First(r => r.AggregateId == aggregate.Id);
            AssertionHelpers.PropertiesAreEqual(aggregate, viewModel, "Id");
        }


        [Fact]
        public void CreationFromMigration_Report_IsStored()
        {
            // Arrange
            var migrationModel = Fixture.Create<ReportViewModel>();
            var aggregate = new ReportAggregate(Guid.NewGuid(), migrationModel);

            // Act
            Repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var newReport = Repository.GetById<ReportAggregate>(aggregate.Id);
            AssertionHelpers.PropertiesAreEqual(aggregate, newReport);
            AssertionHelpers.PropertiesAreEqual(aggregate, migrationModel, "Id");
        }
    }
}
