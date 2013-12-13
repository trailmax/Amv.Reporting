using System;
using System.ComponentModel.DataAnnotations;
using AmvReporting.Infrastructure.CQRS;

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
}