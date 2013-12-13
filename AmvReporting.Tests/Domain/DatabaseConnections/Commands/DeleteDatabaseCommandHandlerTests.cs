using System.Linq;
using AmvReporting.Domain.DatabaseConnections;
using AmvReporting.Domain.DatabaseConnections.Commands;
using AmvReporting.Tests.ZeroFriction;
using Raven.Client;
using Xunit;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.DatabaseConnections.Commands
{
    public class DeleteDatabaseCommandHandlerTests
    {
        [Theory, AutoDomainData]
        public void Handle_Exists_RemoveRecord(DeleteDatabaseCommandHandler sut, DeleteDatabaseCommand command, IDocumentSession ravenSession)
        {
            //Arrange
            var existingRecord = new DatabaseConnection();
            ravenSession.Store(existingRecord);
            ravenSession.SaveChanges();
            command.Id = existingRecord.Id;

            // Act
            sut.Handle(command);

            // Assert
            var result = ravenSession.Query<DatabaseConnection>().FirstOrDefault();
            Assert.Null(result);
        }


        [Theory, AutoDomainData]
        public void Handle_NoExistingRecord_DoesNotThrow(DeleteDatabaseCommandHandler sut, DeleteDatabaseCommand command)
        {
            Assert.DoesNotThrow(() => sut.Handle(command));
        }
    }
}
