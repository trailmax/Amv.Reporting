using AmvReporting.Domain.DatabaseConnections.Commands;
using AmvReporting.Tests.ZeroFriction;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.DatabaseConnections.Commands
{
    public class CreateDatabaseDetailsCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public void CreateModel_Always_MapsAllProperties(CreateDatabaseDetailsCommand command, CreateDatabaseDetailsCommandHandler sut)
        {
            var result = sut.CreateModel(command);

            AssertionHelpers.PropertiesAreEqual(command, result);
        }
    }
}
