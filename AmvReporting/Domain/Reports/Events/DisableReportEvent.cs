using System;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    public class DisableReportEvent : IEvent
    {
        public DisableReportEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }


        public Guid AggregateId { get; private set; }
    }
}