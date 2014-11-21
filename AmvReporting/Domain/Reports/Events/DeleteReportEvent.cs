using System;
using System.ComponentModel;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    [Serializable]
    [Description("Deleted report")]
    public class DeleteReportEvent : IEvent
    {
        public DeleteReportEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }


        public Guid AggregateId { get; private set; }
    }
}