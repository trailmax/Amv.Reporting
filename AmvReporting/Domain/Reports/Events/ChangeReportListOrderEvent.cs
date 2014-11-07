namespace AmvReporting.Domain.Reports.Events
{
    public class ChangeReportListOrderEvent
    {
        public ChangeReportListOrderEvent(int listOrder)
        {
            ListOrder = listOrder;
        }


        public int ListOrder { get; set; }
    }
}