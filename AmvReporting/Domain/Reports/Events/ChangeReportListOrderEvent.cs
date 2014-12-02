using System;
using System.ComponentModel;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    [Serializable]
    [Description("Changed report list order")]
    public class ChangeReportListOrderEvent : IEvent
    {
        public ChangeReportListOrderEvent(Guid id, int listOrder)
        {
            AggregateId = id;
            ListOrder = listOrder;
        }


        public int ListOrder { get; set; }

        public Guid AggregateId { get; private set; }
    }
}