﻿using System;
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
            aggregate.UpdateCode(Guid.NewGuid(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());


            // Act
            Repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var viewModel = DocumentSession.Query<ReportViewModel>().First(r => r.AggregateId == aggregate.Id);
            Assert.Equal(aggregate.Sql, viewModel.Sql);
            Assert.Equal(aggregate.JavaScript, viewModel.JavaScript);
            Assert.Equal(aggregate.HtmlOverride, viewModel.HtmlOverride);
        }


        [Fact]
        public void UpdateMetadata_DenormalisedModel_RepeatsAggregate()
        {
            // Arrange
            var aggregate = Fixture.Create<ReportAggregate>();
            aggregate.UpdateMetadata(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), true);


            // Act
            Repository.Save(aggregate, Guid.NewGuid());

            // Assert
            var viewModel = DocumentSession.Query<ReportViewModel>().FirstOrDefault(r => r.AggregateId == aggregate.Id);
            AssertionHelpers.PropertiesAreEqual(aggregate, viewModel, "Id");
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
    }
}
