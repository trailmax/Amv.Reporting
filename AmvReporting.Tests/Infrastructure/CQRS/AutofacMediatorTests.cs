using AmvReporting.Infrastructure.CQRS;
using Autofac;
using Autofac.Core.Registration;
using Xunit;

namespace AmvReporting.Tests.Infrastructure.CQRS
{
    public class AutofacMediatorTests
    {
        private readonly IContainer container;

        public AutofacMediatorTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<RegisteredQueryHandler>().As<IQueryHandler<RegisteredQuery, int>>();
            container = builder.Build();
        }

        [Fact]
        public void Handle_RequestWithNoHandler_Throws()
        {
            var sut = new AutofacMediator(container);
            var query = new NonRegisteredQuery();
            Assert.Throws<ComponentNotRegisteredException>(() => sut.Request(query));
        }

        [Fact]
        public void Handle_RequestWithRegisteredHandler_ReturnsExpectedResult()
        {
            var sut = new AutofacMediator(container);
            var query = new RegisteredQuery();
            var actual = sut.Request(query);
            Assert.Equal(1, actual);
        }


        private class NonRegisteredQuery : IQuery<int>
        {
        }


        private class RegisteredQuery : IQuery<int>
        {
        }


        private class RegisteredQueryHandler : IQueryHandler<RegisteredQuery, int>
        {
            public int Handle(RegisteredQuery query)
            {
                return 1;
            }
        }
    }
}
