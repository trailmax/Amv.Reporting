using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Raven.Client.Embedded;

namespace AmvReporting.Tests.ZeroFriction
{
    public class AmvReportingCustomisation : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new AutoNSubstituteCustomization());

            var documentStore = new EmbeddableDocumentStore { RunInMemory = true };
            documentStore.Initialize();
            var session = documentStore.OpenSession();

            fixture.Inject(session);
        }
    }
}
