using System;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;


namespace AmvReporting.Domain.Templates.Commands
{
    public class UpdateTemplateCommand : CreateTemplateCommand
    {
    }

    public class UpdateTemplateCommandHandler : ICommandHandler<UpdateTemplateCommand>
    {
        private readonly IRepository repository;


        public UpdateTemplateCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }


        public void Handle(UpdateTemplateCommand command)
        {
            var template = repository.GetById<TemplateAggregate>(command.AggregateId);
            template.UpdateTemplate(command.Title, command.JavaScript, command.Html);

            var commitId = Guid.NewGuid();
            repository.Save(template, commitId);
        }
    }
}