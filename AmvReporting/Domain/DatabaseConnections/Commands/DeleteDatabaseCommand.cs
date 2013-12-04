using System;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.DatabaseConnections.Commands
{
    public class DeleteDatabaseCommand : ICommand
    {
        public String Id { get; set; }
    }


    public class DeleteDatabaseCommandHandler : ICommandHandler<DeleteDatabaseCommand>
    {
        private readonly IDocumentSession ravenSession;

        public DeleteDatabaseCommandHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public void Handle(DeleteDatabaseCommand command)
        {
            var toBeDeleted = ravenSession.Load<DatabaseConnection>(command.Id);

            ravenSession.Delete(toBeDeleted);
            ravenSession.SaveChanges();
        }
    }
}