using System;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
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