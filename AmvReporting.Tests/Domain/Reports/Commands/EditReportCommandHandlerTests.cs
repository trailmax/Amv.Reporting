using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Reports.Commands;
using AmvReporting.Infrastructure.Events;
using AmvReporting.Tests.ZeroFriction;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.Reports.Commands
{
    public class EditReportCommandHandlerTests
    {
        [Theory, AutoDomainData]
        public void UpdateReport_Always_MatchesFieldNames(EditReportCommandHandler sut, EditReportCommand command, Report report)
        {
            DomainEvents.Dispatcher = NSubstitute.Substitute.For<IDomainEventDispatcher>();

            var result = sut.UpdateReport(command, report);

            AssertionHelpers.PropertiesAreEqual(command, result, "Id");
        }
    }
}
