using System;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Domain.Reports.Commands
{
    public class DeleteReportCommand : ICommand
    {
        public String AggregateId { get; set; }
    }
}