using System;
using System.Linq;
using AmvReporting.Domain.Templates;
using AmvReporting.Domain.Templates.Commands;
using AmvReporting.Domain.Templates.Events;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Tests.ZeroFriction;
using Autofac;
using Ploeh.AutoFixture;
using CommonDomain.Persistence;
using Xunit;


namespace AmvReporting.Tests.Domain.Templates.Commands
{
    public class UpdateTemplateCommandHandlerTests : IntegrationTestsBase
    {
        private readonly TemplateAggregate templateAggregate;

        public UpdateTemplateCommandHandlerTests()
        {
            templateAggregate = Fixture.Create<TemplateAggregate>();
            Repository.Save(templateAggregate, Guid.NewGuid());
        }


        [Fact]
        public void UpdatedAggregated_Matches_Command()
        {
            var command = Fixture.Build<UpdateTemplateCommand>().With(c => c.AggregateID, templateAggregate.Id).Create();
            var sut = Container.Resolve<ICommandHandler<UpdateTemplateCommand>>();

            // Act
            sut.Handle(command);

            var template = Repository.GetById<TemplateAggregate>(templateAggregate.Id);
            AssertionHelpers.PropertiesAreEqual(command, template);
        }


        [Fact]
        public void ViewModel_Matches_Command()
        {
            // Arrange
            var command = Fixture.Build<UpdateTemplateCommand>().With(c => c.AggregateID, templateAggregate.Id).Create();
            var sut = Container.Resolve<ICommandHandler<UpdateTemplateCommand>>();

            // Act
            sut.Handle(command);

            // Assert
            var viewModel = DocumentSession.Query<TemplateViewModel>().First(r => r.AggregateId == templateAggregate.Id);
            AssertionHelpers.PropertiesAreEqual(command, viewModel, "Id");
        }
    }
}
