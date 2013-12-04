using AmvReporting.Domain.DatabaseConnections;
using AmvReporting.Domain.DatabaseConnections.Commands;
using AmvReporting.Tests.ZeroFriction;
using Xunit.Extensions;

namespace AmvReporting.Tests.Commands
{
    public class EditDatabaseDetailsCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public void Update_Always_MatchesAllDetails(EditDatabaseDetailsCommand command, DatabaseConnection databaseConnection, EditDatabaseDetailsCommandHandler sut)
        {
            sut.Update(databaseConnection, command);

            AssertionHelpers.PropertiesAreEqual(command, databaseConnection, "Id");
        }
    }
}
