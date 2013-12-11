//TODO REMOVE
//using AmvReporting.Domain.Reports;
//using AmvReporting.Domain.Reports.Commands;
//using AmvReporting.Tests.ZeroFriction;
//using Raven.Client;
//using Xunit;
//using Xunit.Extensions;

//namespace AmvReporting.Tests.Domain.Reports.Commands
//{
//    public class EditReportCommandValidatorTests
//    {
//        [Theory, AutoRavenData]
//        public void EditCommandIsValid_DuplicateExists_NotValidated(EditReportCommandValidator sut, EditReportCommand command, IDocumentSession ravenSession)
//        {
//            //Arrange
//            var report = new Report() { LinkName = command.LinkName };
//            ravenSession.Store(report);
//            ravenSession.SaveChanges();

//            // Act
//            var result = sut.IsValid(command);

//            // Assert
//            Assert.False(result);
//        }

//        [Theory, AutoRavenData]
//        public void EditCommandIsValid_UpdatingTheSameRecord_IsValid(EditReportCommandValidator sut, EditReportCommand command, IDocumentSession ravenSession)
//        {
//            //Arrange
//            var report = new Report() { LinkName = command.LinkName };
//            ravenSession.Store(report);
//            ravenSession.SaveChanges();
//            command.Id = report.Id;

//            // Act
//            var result = sut.IsValid(command);

//            // Assert
//            Assert.True(result);
//        }

//        [Theory, AutoRavenData]
//        public void EditCommandIsValid_NoDuplicateExists_IsValidated(EditReportCommandValidator sut, EditReportCommand command, IDocumentSession ravenSession)
//        {
//            //Arrange
//            var report = new Report() { LinkName = command.LinkName + "random" };
//            ravenSession.Store(report);
//            ravenSession.SaveChanges();

//            // Act
//            var result = sut.IsValid(command);

//            // Assert
//            Assert.True(result);
//        }
//    }
//}
