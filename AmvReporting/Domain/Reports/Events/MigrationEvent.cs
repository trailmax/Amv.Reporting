using System;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    [Serializable]
    public class MigrationEvent : IDomainEvent
    {
        public MigrationEvent(Report migratedReport)
        {
            MigratedReport = migratedReport;
        }


        public Report MigratedReport { get; set; }
    }
}