using AmvReporting.Domain.ReportingConfigs;
using AmvReporting.Domain.ReportingConfigs.Queries;
using AmvReporting.Tests.ZeroFriction;
using Raven.Client;
using Xunit;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.AppConfig.Queries
{
    public class ReportingConfigQueryHandlerTests
    {
        [Theory, AutoDomainData]
        public void Handle_NoConfigSaved_ReturnsNewObject(ReportingConfigQueryHandler sut)
        {
            var result = sut.Handle(new ReportingConfigQuery());

            Assert.Null(result.GlobalJavascript);
            Assert.Null(result.GlobalCss);
        }

        [Theory, AutoDomainData]
        public void Handle_ConfiguredObjectSaved_ReturnsTheOnlyObject(ReportingConfigQueryHandler sut, string globalJs, string globalCss, IDocumentSession documentSession)
        {
            //Arrange
            var reportingConfig = ReportingConfig.New();
            reportingConfig.GlobalJavascript = globalJs;
            reportingConfig.GlobalCss = globalCss;
            documentSession.Store(reportingConfig);
            documentSession.SaveChanges();

            // Act
            var result = sut.Handle(new ReportingConfigQuery());

            // Assert
            AssertionHelpers.PropertiesAreEqual(reportingConfig, result);
        }
    }
}
