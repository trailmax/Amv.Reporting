using System;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    public class DeleteReportEvent : IEvent
    {
        public DeleteReportEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }


        public Guid AggregateId { get; private set; }
    }
}