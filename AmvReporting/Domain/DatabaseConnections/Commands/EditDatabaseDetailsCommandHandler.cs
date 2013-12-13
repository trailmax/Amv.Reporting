using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.DatabaseConnections.Commands
{
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