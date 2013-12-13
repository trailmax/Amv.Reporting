using AmvReporting.Domain.ReportGroups;
using AmvReporting.Domain.ReportGroups.Commands;
using AmvReporting.Tests.ZeroFriction;
using Raven.Client;
using Xunit;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.ReportGroups.Commands
{
    public class DeleteReportGroupCommandHandlerTests
    {
        [Theory, AutoDomainData]
        public void Handle_Always_Deletes(IDocumentSession ravenSession, DeleteReportGroupCommandHandler sut, DeleteReportGroupCommand command)
        {
            //Arrange
            var reportGroup = new ReportGroup();
            ravenSession.Store(reportGroup);
            ravenSession.SaveChanges();
            command.Id = reportGroup.Id;


            // Act
            sut.Handle(command);

            // Assert
            var result = ravenSession.Load<ReportGroup>(command.Id);
            Assert.Null(result);
        }


        [Theory, AutoDomainData]
        public void Handle_NothingToDelete_DoesNotThrow(DeleteReportGroupCommandHandler sut, DeleteReportGroupCommand command)
        {
            Assert.DoesNotThrow(() => sut.Handle(command));
        }
    }
}
