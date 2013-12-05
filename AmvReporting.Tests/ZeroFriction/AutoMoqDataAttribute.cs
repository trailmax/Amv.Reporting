using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace AmvReporting.Tests.ZeroFriction
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(new Fixture().Customize(new AmvReportingCustomisation()))
        {
        }
    }

    public class AutoRavenDataAttribute : AutoDataAttribute
    {
        public AutoRavenDataAttribute() : base(new Fixture().Customize(new AutoRavenData()))
        {
        }
    }
}
