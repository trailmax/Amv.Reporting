using System;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    public class SetReportEnabledEvent : IEvent
    {
        public SetReportEnabledEvent(Guid aggregateId, bool isEnabled)
        {
            AggregateId = aggregateId;
            IsEnabled = isEnabled;
        }


        public Guid AggregateId { get; private set; }
        public bool IsEnabled { get; private set; }
    }
}