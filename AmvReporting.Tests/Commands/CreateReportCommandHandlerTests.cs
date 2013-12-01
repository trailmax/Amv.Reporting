using AmvReporting.Commands;
using AmvReporting.Tests.ZeroFriction;
using Xunit.Extensions;

namespace AmvReporting.Tests.Commands
{
    public class CreateReportCommandHandlerTests
    {
        [Theory, AutoMoqDataAttribute]
        public void CreateReportModel_Always_MatchesProperties(CreateReportCommandHandler sut, CreateReportCommand command)
        {
            var result = sut.CreateReportDetails(command);

            AssertionHelpers.PropertiesAreEqual(command, result);
        }
    }
}
