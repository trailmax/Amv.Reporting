using System.ComponentModel.DataAnnotations;
using AmvReporting.Domain.DatabaseConnections.Queries;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;


namespace AmvReporting.Domain.DatabaseConnections.Commands
{
    public class CreateDatabaseDetailsCommand : ICommand
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ConnectionString { get; set; }

        public string Description { get; set; }
    }


    public class CreateDatabaseDetailsCommandValidator : ICommandValidator<CreateDatabaseDetailsCommand>
    {
        private readonly IMediator mediator;
        public ErrorList Errors { get; private set; }

        public CreateDatabaseDetailsCommandValidator(IMediator mediator)
        {
            this.mediator = mediator;

            Errors = new ErrorList();
        }

        public bool IsValid(CreateDatabaseDetailsCommand command)
        {
            var connectionChecker = mediator.Request(new CheckDatabaseConnectivityQuery(command.ConnectionString));

            if (!connectionChecker.IsConnected)
            {
                Errors.Add("ConnectionString", connectionChecker.ExceptionMessage);
            }

            return Errors.IsValid();
        }
    }


    public class CreateDatabaseDetailsCommandHandler : ICommandHandler<CreateDatabaseDetailsCommand>
    {
        private readonly IDocumentSession ravenSession;

        public CreateDatabaseDetailsCommandHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public void Handle(CreateDatabaseDetailsCommand command)
        {
            var model = CreateModel(command);
            ravenSession.Store(model);
            ravenSession.SaveChanges();
        }


        public DatabaseConnection CreateModel(CreateDatabaseDetailsCommand command)
        {
            var model = new DatabaseConnection()
            {
                Name = command.Name,
                ConnectionString = command.ConnectionString,
                Description = command.Description,
            };

            return model;
        }
    }
}