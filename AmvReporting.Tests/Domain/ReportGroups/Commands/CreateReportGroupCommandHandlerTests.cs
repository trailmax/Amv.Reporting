using System.Linq;
using AmvReporting.Domain.ReportGroups;
using AmvReporting.Domain.ReportGroups.Commands;
using AmvReporting.Tests.ZeroFriction;
using Raven.Client;
using Xunit;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.ReportGroups.Commands
{
    public class CreateReportGroupCommandHandlerTests
    {
        [Theory, AutoDomainData]
        public void Handle_Always_CreatesCorrectGroup(CreateReportGroupCommandHandler sut, CreateReportGroupCommand command, IDocumentSession ravenSession)
        {
            //Act
            sut.Handle(command);

            // Assert
            var result = ravenSession.Query<ReportGroup>().First();
            AssertionHelpers.PropertiesAreEqual(result, command, "Id");
        }
    }
}
