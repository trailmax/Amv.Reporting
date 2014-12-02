using System.Collections.Generic;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Kernel;
using Raven.Client.Embedded;

namespace AmvReporting.Tests.ZeroFriction
{
    public class DomainCustomisation : ICustomization
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

    public class GreedyEngineParts : DefaultEngineParts
    {
        public override IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            var iter = base.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current is MethodInvoker)
                    yield return new MethodInvoker(
                        new CompositeMethodQuery(
                            new GreedyConstructorQuery(),
                            new FactoryMethodQuery()));
                else
                    yield return iter.Current;
            }
        }
    }
}
