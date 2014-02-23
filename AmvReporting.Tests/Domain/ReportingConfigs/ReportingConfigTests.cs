using AmvReporting.Domain.ReportingConfigs;
using Xunit;

namespace AmvReporting.Tests.Domain.AppConfig
{
    public class ReportingConfigTests
    {
        [Fact]
        public void NewInstance_Always_GivesSameId()
        {
            var sut = ReportingConfig.New();

            Assert.Equal("Reporting/Config", sut.Id);
        }
    }
}
