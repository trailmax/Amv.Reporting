using AmvReporting.Domain.DatabaseConnections;
using AmvReporting.Domain.DatabaseConnections.Commands;
using AmvReporting.Tests.ZeroFriction;
using Raven.Client;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.DatabaseConnections.Commands
{
    public class EditDatabaseDetailsCommandHandlerTests
    {
        [Theory, AutoDomainData]
        public void Update_Always_MatchesAllDetails(EditDatabaseDetailsCommand command, DatabaseConnection databaseConnection, EditDatabaseDetailsCommandHandler sut)
        {
            sut.Update(databaseConnection, command);

            AssertionHelpers.PropertiesAreEqual(command, databaseConnection, "Id");
        }

        [Theory, AutoDomainData]
        public void Handle_Always_UpdatesExisting(
            EditDatabaseDetailsCommandHandler sut, 
            EditDatabaseDetailsCommand command, 
            DatabaseConnection existingConnection,
            IDocumentSession ravenSession)
        {
            //Arrange
            ravenSession.Store(existingConnection);
            ravenSession.SaveChanges();
            command.Id = existingConnection.Id;

            // Act
            sut.Handle(command);

            // Assert
            var result = ravenSession.Load<DatabaseConnection>(command.Id);
            AssertionHelpers.PropertiesAreEqual(command, result);
        }
    }
}
