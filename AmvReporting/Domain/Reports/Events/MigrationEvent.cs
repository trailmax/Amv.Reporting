using System;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    [Serializable]
    public class MigrationEvent : IEvent
    {
        public MigrationEvent(Guid aggregateId, ReportViewModel migratedReport)
        {
            AggregateId = aggregateId;
            MigratedReport = migratedReport;
        }


        public ReportViewModel MigratedReport { get; set; }
        public Guid AggregateId { get; private set; }
    }
}