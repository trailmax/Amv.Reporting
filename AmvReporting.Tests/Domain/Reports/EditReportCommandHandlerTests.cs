using AmvReporting.Domain.ReportDetails;
using AmvReporting.Domain.ReportDetails.Commands;
using AmvReporting.Tests.ZeroFriction;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.Reports
{
    public class EditReportCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public void UpdateReport_Always_MatchesFieldNames(EditReportCommandHandler sut, EditReportCommand command, Report report)
        {
            var result = sut.UpdateReport(command, report);

            AssertionHelpers.PropertiesAreEqual(command, result, "Id");
        }
    }
}
