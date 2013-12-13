using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.DatabaseConnections.Commands
{
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