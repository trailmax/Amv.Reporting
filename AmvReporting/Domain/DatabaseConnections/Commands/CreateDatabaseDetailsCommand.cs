using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Domain.DatabaseConnections.Commands
{
    public class CreateDatabaseDetailsCommand : ICommand
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string Description { get; set; }
    }
}