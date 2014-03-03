using System;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Domain.DatabaseConnections.Commands
{
    public class EditDatabaseDetailsCommand : CreateDatabaseDetailsCommand, ICommand
    {
        public String Id { get; set; }
    }
}