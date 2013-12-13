using AmvReporting.Domain.ReportGroups;
using AmvReporting.Domain.ReportGroups.Commands;
using AmvReporting.Tests.ZeroFriction;
using Raven.Client;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.ReportGroups.Commands
{
    public class EditReportGroupCommandHandlerTests
    {
        [Theory, AutoDomainData]
        public void Handle_Always_UpdatesAllFields(
                ReportGroup existingGroup,
                EditReportGroupCommandHandler sut,
                EditReportGroupCommand command,
                IDocumentSession ravenSession
            )
        {
            //Arrange
            ravenSession.Store(existingGroup);
            ravenSession.SaveChanges();

            command.Id = existingGroup.Id;

            // Act
            sut.Handle(command);

            // Assert
            var updatedGroup = ravenSession.Load<ReportGroup>(command.Id);
            AssertionHelpers.PropertiesAreEqual(command, updatedGroup);
        }
    }
}
