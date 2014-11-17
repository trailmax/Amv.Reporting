using System;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    public class EnableReportEvent : IEvent
    {
        public EnableReportEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }


        public Guid AggregateId { get; private set; }
    }
}