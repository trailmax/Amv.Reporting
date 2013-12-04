using AmvReporting.Domain.ReportDetails.Commands;
using AmvReporting.Tests.ZeroFriction;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.Reports
{
    public class CreateReportCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public void CreateReportModel_Always_MatchesProperties(CreateReportCommandHandler sut, CreateReportCommand command)
        {
            var result = sut.CreateReportDetails(command);

            AssertionHelpers.PropertiesAreEqual(command, result);
        }
    }
}
