using AmvReporting.Infrastructure.Automappings;
using AutoMapper;
using Xunit;

namespace AmvReporting.Tests.Infrastructure.Automappings
{
    public class AutoMapperBootstrapperTests
    {
        [Fact]
        public void Configure_Always_HasMappedFields()
        {
            AutoMapperBootstrapper.Initialize();

            Mapper.AssertConfigurationIsValid();
        }
    }
}
