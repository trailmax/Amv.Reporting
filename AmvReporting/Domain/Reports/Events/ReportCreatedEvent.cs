using System;
using System.ComponentModel;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    [Serializable]
    [Description("Created report")]
    public class ReportCreatedEvent : IEvent
    {
        public ReportCreatedEvent(Guid aggregateId, string reportGroupId, string title, string description, string databaseId, bool enabled)
        {
            AggregateId = aggregateId;
            ReportGroupId = reportGroupId;
            Title = title;
            Description = description;
            DatabaseId = databaseId;
            Enabled = enabled;
        }

        public String ReportGroupId { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        public String DatabaseId { get; private set; }

        public bool Enabled { get; private set; }

        public Guid AggregateId { get; private set; }
    }
}