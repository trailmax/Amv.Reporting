//TODO REMOVE
//using System.Linq;
//using AmvReporting.Domain.Reports;
//using AmvReporting.Domain.Reports.Commands;
//using AmvReporting.Tests.ZeroFriction;
//using Raven.Client;
//using Xunit;
//using Xunit.Extensions;

//namespace AmvReporting.Tests.Domain.Reports.Commands
//{
//    public class ModifyReportCommandValidatorTest
//    {
//        [Theory, AutoRavenDataAttribute]
//        public void CreateCommandIsValid_DuplicateExists_NotValidated(CreateReportCommandValidator sut, CreateReportCommand command, IDocumentSession ravenSession)
//        {
//            //Arrange
//            var report = new Report() {LinkName = command.LinkName};
//            ravenSession.Store(report);
//            ravenSession.SaveChanges();

//            // Act
//            var result = sut.IsValid(command);

//            // Assert
//            Assert.False(result);
//        }

//        [Theory, AutoRavenDataAttribute]
//        public void CreateCommandIsValid_NoDuplicateExists_IsValidated(CreateReportCommandValidator sut, CreateReportCommand command, IDocumentSession ravenSession)
//        {
//            //Arrange
//            var report = new Report() { LinkName = command.LinkName + "random"};
//            ravenSession.Store(report);
//            ravenSession.SaveChanges();

//            // Act
//            var result = sut.IsValid(command);

//            // Assert
//            Assert.True(result);
//        }
//    }
//}
