using AmvReporting.Domain.ReportingConfigs;
using AmvReporting.Domain.ReportingConfigs.Commands;
using AmvReporting.Tests.ZeroFriction;
using Raven.Client;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.ReportingConfigs.Commands
{
    public class UpdateConfigurationCommandHandlerTests
    {
        [Theory, AutoDomainData]
        public void Handle_NoPreviousObject_Creates(UpdateConfigurationCommandHandler sut, UpdateConfigurationCommand command, IDocumentSession documentSession)
        {
            // Act
            sut.Handle(command);

            // Assert
            var config = documentSession.Load<ReportingConfig>(ReportingConfig.IdString);
            AssertionHelpers.PropertiesAreEqual(command, config);
        }


        [Theory, AutoDomainData]
        public void Handle_PreviosObjectExists_UpdatesExistingObject(UpdateConfigurationCommandHandler sut, UpdateConfigurationCommand command, 
            IDocumentSession documentSession, ReportingConfig config)
        {
            //Arrange
            documentSession.Store(config);
            documentSession.SaveChanges();

            // Act
            sut.Handle(command);

            // Assert
            var updatedConfig = documentSession.Load<ReportingConfig>(ReportingConfig.IdString);
            AssertionHelpers.PropertiesAreEqual(command, updatedConfig);
        }
    }
}
