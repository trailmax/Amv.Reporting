using System.Linq;
using AmvReporting.Domain.Templates;
using AmvReporting.Domain.Templates.Commands;
using AmvReporting.Domain.Templates.Events;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Tests.ZeroFriction;
using Autofac;
using Ploeh.AutoFixture;
using Xunit;


namespace AmvReporting.Tests.Domain.Templates.Commands
{
    public class CreateTemplateCommandHandlerTests : IntegrationTestsBase
    {
        [Fact]
        public void CreatedTemplate_Matches_Command()
        {
            var command = Fixture.Create<CreateTemplateCommand>();
            var sut = Container.Resolve<ICommandHandler<CreateTemplateCommand>>();

            // Assert
            sut.Handle(command);

            // Assert
            var templateAggregate = Repository.GetById<TemplateAggregate>(command.AggregateID);

            AssertionHelpers.PropertiesAreEqual(command, templateAggregate);
        }


        [Fact]
        public void CreatedViewModel_Matches_Command()
        {
            var command = Fixture.Create<CreateTemplateCommand>();
            var sut = Container.Resolve<ICommandHandler<CreateTemplateCommand>>();

            // Assert
            sut.Handle(command);

            // Assert
            var veiwModel = DocumentSession.Query<TemplateViewModel>().First(t => t.AggregateId == command.AggregateID);
            AssertionHelpers.PropertiesAreEqual(command, veiwModel);
        }
    }
}
