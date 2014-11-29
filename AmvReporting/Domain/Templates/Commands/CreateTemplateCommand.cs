using System;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;


namespace AmvReporting.Domain.Templates.Commands
{
    public class CreateTemplateCommand : ICommand
    {
        public Guid AggregateId { get; set; }

        public String Title { get; set; }

        public String JavaScript { get; set; }

        public String Html { get; set; }

        public bool AllowOverrides { get; set; }
    }

    public class CreateTemplateCommandHandler : ICommandHandler<CreateTemplateCommand>
    {
        private readonly IRepository repository;


        public CreateTemplateCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }


        public void Handle(CreateTemplateCommand command)
        {
            var template = new TemplateAggregate(command.AggregateId, command.Title, command.JavaScript, command.Html, command.AllowOverrides);
            var commitId = Guid.NewGuid();
            repository.Save(template, commitId);
        }
    }
}