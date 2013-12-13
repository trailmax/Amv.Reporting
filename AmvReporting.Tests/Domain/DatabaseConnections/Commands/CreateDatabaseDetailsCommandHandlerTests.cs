using System.Linq;
using AmvReporting.Domain.DatabaseConnections;
using AmvReporting.Domain.DatabaseConnections.Commands;
using AmvReporting.Tests.ZeroFriction;
using Raven.Client;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.DatabaseConnections.Commands
{
    public class CreateDatabaseDetailsCommandHandlerTests
    {
        [Theory, AutoDomainData]
        public void CreateModel_Always_MapsAllProperties(CreateDatabaseDetailsCommand command, CreateDatabaseDetailsCommandHandler sut)
        {
            var result = sut.CreateModel(command);

            AssertionHelpers.PropertiesAreEqual(command, result);
        }

        [Theory, AutoDomainData]
        public void Handle_Always_CreatesRecord(CreateDatabaseDetailsCommandHandler sut, 
            CreateDatabaseDetailsCommand command, 
            IDocumentSession raveSession)
        {
            // Act
            sut.Handle(command);

            // Assert
            var result = raveSession.Query<DatabaseConnection>().First();

            AssertionHelpers.PropertiesAreEqual(command, result, "Id");
        }
    }
}
