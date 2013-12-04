using System;
using System.ComponentModel.DataAnnotations;
using AmvReporting.Domain.DatabaseConnections.Queries;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.DatabaseConnections.Commands
{
    public class EditDatabaseDetailsCommand : ICommand
    {
        [Required]
        public String Id { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public String ConnectionString { get; set; }

        public String Description { get; set; }
    }


    public class EditDatabaseDetailsCommandValidator : ICommandValidator<EditDatabaseDetailsCommand>
    {
        private readonly IMediator mediator;
        public ErrorList Errors { get; private set; }

        public EditDatabaseDetailsCommandValidator(IMediator mediator)
        {
            this.mediator = mediator;

            Errors = new ErrorList();
        }

        public bool IsValid(EditDatabaseDetailsCommand command)
        {
            var connectionChecker = mediator.Request(new CheckDatabaseConnectivityQuery(command.ConnectionString));

            if (!connectionChecker.IsConnected)
            {
                Errors.Add("ConnectionString", connectionChecker.ExceptionMessage);
            }

            return Errors.IsValid();
        }
    }


    public class EditDatabaseDetailsCommandHandler : ICommandHandler<EditDatabaseDetailsCommand>
    {
        private readonly IDocumentSession ravenSession;

        public EditDatabaseDetailsCommandHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public void Handle(EditDatabaseDetailsCommand command)
        {
            var databaseDetail = ravenSession.Load<DatabaseConnection>(command.Id);
            Update(databaseDetail, command);
            ravenSession.SaveChanges();
        }

        public void Update(DatabaseConnection databaseConnection, EditDatabaseDetailsCommand command)
        {
            databaseConnection.Name = command.Name;
            databaseConnection.ConnectionString = command.ConnectionString;
            databaseConnection.Description = command.Description;
        }
    }
}