using System.ComponentModel.DataAnnotations;
using AmvReporting.Infrastructure.CQRS;

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
}