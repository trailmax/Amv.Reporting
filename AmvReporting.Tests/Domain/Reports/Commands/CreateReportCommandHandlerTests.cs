using System.Linq;
using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Reports.Commands;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Tests.ZeroFriction;
using Autofac;
using Ploeh.AutoFixture;
using Xunit;


namespace AmvReporting.Tests.Domain.Reports.Commands
{
    public class CreateReportCommandHandlerTests : IntegrationTestsBase
    {
        [Fact]
        public void CreatedAggregate_Matches_Command()
        {
            // Arrange
            var command = Fixture.Create<CreateReportCommand>();
            var sut = Container.Resolve<ICommandHandler<CreateReportCommand>>();

            // Assert
            sut.Handle(command);

            // Assert
            var aggregateReport = Repository.GetById<ReportAggregate>(command.RedirectingId);

            AssertionHelpers.PropertiesAreEqual(command, aggregateReport);
        }


        [Fact]
        public void CreatedViewModel_Matches_Command()
        {
            // Arrange
            var command = Fixture.Create<CreateReportCommand>();
            var sut = Container.Resolve<ICommandHandler<CreateReportCommand>>();

            // Assert
            sut.Handle(command);

            // Assert
            var viewModel = DocumentSession.Query<ReportViewModel>().First(r => r.AggregateId == command.RedirectingId);
            AssertionHelpers.PropertiesAreEqual(command, viewModel);
        }
    }
}
