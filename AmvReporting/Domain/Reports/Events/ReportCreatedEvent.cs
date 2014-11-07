using System;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    [Serializable]
    public class ReportCreatedEvent : IDomainEvent
    {
        public ReportCreatedEvent(Guid id, String reportGroupId, String title, ReportType reportType, String description, String databaseId)
        {
            Id = id;
            ReportGroupId = reportGroupId;
            Title = title;
            ReportType = reportType;
            Description = description;
            DatabaseId = databaseId;
        }
        public Guid Id { get; set; }

        public String ReportGroupId { get; set; }

        public String Title { get; set; }

        public ReportType ReportType { get; set; }

        public String Description { get; set; }

        public String DatabaseId { get; private set; }
    }
}