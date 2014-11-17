using System;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    [Serializable]
    public class ReportCreatedEvent : IEvent
    {
        public ReportCreatedEvent(Guid aggregateId, String reportGroupId, String title, ReportType reportType, String description, String databaseId)
        {
            AggregateId = aggregateId;
            ReportGroupId = reportGroupId;
            Title = title;
            ReportType = reportType;
            Description = description;
            DatabaseId = databaseId;
        }

        public String ReportGroupId { get; set; }

        public String Title { get; set; }

        public ReportType ReportType { get; set; }

        public String Description { get; set; }

        public String DatabaseId { get; private set; }

        public Guid AggregateId { get; private set; }
    }
}