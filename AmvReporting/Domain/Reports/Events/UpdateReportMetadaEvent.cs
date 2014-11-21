using System;
using System.ComponentModel;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    [Serializable]
    [Description("Updated report metadata")]
    public class UpdateReportMetadaEvent : IEvent
    {
        public String ReportGroupId { get; set; }

        public String Title { get; set; }

        public ReportType ReportType { get; set; }

        public String Description { get; set; }

        public String DatabaseId { get; private set; }


        public UpdateReportMetadaEvent(Guid aggregateId, String reportGroupId, String title, ReportType reportType, String description, String databaseId)
        {
            AggregateId = aggregateId;
            ReportGroupId = reportGroupId;
            Title = title;
            ReportType = reportType;
            Description = description;
            DatabaseId = databaseId;
        }


        public Guid AggregateId { get; private set; }
    }
}