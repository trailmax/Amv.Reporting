using AmvReporting.Commands;
using AmvReporting.Models;
using AmvReporting.Tests.ZeroFriction;
using Xunit.Extensions;

namespace AmvReporting.Tests.Commands
{
    public class EditDatabaseDetailsCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public void Update_Always_MatchesAllDetails(EditDatabaseDetailsCommand command, DatabaseDetails databaseDetail, EditDatabaseDetailsCommandHandler sut)
        {
            sut.Update(databaseDetail, command);

            AssertionHelpers.PropertiesAreEqual(command, databaseDetail, "Id");
        }
    }
}
