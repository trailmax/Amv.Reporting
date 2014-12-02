using System;
using System.Linq;
using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Reports.Commands;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Tests.ZeroFriction;
using Autofac;
using Ploeh.AutoFixture;
using Xunit;
using CommonDomain.Persistence;


namespace AmvReporting.Tests.Domain.Reports.Commands
{
    public class UpdateReportMetadataCommandHandlerTests : IntegrationTestsBase
    {
        private readonly ReportAggregate aggregateReport;

        public UpdateReportMetadataCommandHandlerTests()
            : base()
        {
            aggregateReport = Fixture.Create<ReportAggregate>();
            Repository.Save(aggregateReport, Guid.NewGuid());
        }


        [Fact]
        public void UpdatedAggregate_Matches_Command()
        {
            // Arrange
            var command = Fixture.Build<UpdateReportMetadataCommand>().With(c => c.AggregateId, aggregateReport.Id).Create();
            var sut = Container.Resolve<ICommandHandler<UpdateReportMetadataCommand>>();

            // Act
            sut.Handle(command);

            // Assert
            var report = Repository.GetById<ReportAggregate>(aggregateReport.Id);
            AssertionHelpers.PropertiesAreEqual(command, report);
        }


        [Fact]
        public void ViewModel_Matches_Command()
        {
            // Arrange
            var command = Fixture.Build<UpdateReportMetadataCommand>().With(c => c.AggregateId, aggregateReport.Id).Create();
            var sut = Container.Resolve<ICommandHandler<UpdateReportMetadataCommand>>();

            // Act
            sut.Handle(command);

            // Assert
            var viewModel = DocumentSession.Query<ReportViewModel>().First(r => r.AggregateId == aggregateReport.Id);
            AssertionHelpers.PropertiesAreEqual(command, viewModel, "Id");
        }
    }
}
