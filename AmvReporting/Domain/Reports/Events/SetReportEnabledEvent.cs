using System;
using System.ComponentModel;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    [Serializable]
    [Description("Changed report visibility")]
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