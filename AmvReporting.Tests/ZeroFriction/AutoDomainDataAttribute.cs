using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace AmvReporting.Tests.ZeroFriction
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(new Fixture().Customize(new AmvReportingCustomisation()))
        {
        }
    }
}
