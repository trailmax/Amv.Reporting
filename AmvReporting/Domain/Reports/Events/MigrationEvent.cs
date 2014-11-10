using System;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    [Serializable]
    public class MigrationEvent : IDomainEvent
    {
        public MigrationEvent(ReportViewModel migratedReport)
        {
            MigratedReport = migratedReport;
        }


        public ReportViewModel MigratedReport { get; set; }
    }
}