using AmvReporting.Commands;
using AmvReporting.Models;
using AmvReporting.Tests.ZeroFriction;
using Xunit.Extensions;

namespace AmvReporting.Tests.Commands
{
    public class EditReportCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public void UpdateReport_Always_MatchesFieldNames(EditReportCommandHandler sut, EditReportCommand command, ReportDetails report)
        {
            var result = sut.UpdateReport(command, report);

            AssertionHelpers.PropertiesAreEqual(command, result, "Id");
        }
    }
}
