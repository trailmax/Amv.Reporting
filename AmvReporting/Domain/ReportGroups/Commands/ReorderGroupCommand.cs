using System;
using System.Linq;
using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Reports.Events;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.ReportGroups.Commands
{
    public class ReorderGroupCommand : ICommand
    {
        public String ParentGroupId { get; set; }

        public String[] ReportIds { get; set; }
        public String[] GroupIds { get; set; }
    }



    public class ReorderGroupCommandHandler : ICommandHandler<ReorderGroupCommand>
    {
        private readonly IDocumentSession ravenSession;

        public ReorderGroupCommandHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public void Handle(ReorderGroupCommand command)
        {
            if (command.ReportIds == null)
            {
                command.ReportIds = new string[0];
            }
            for (var i = 0; i < command.ReportIds.Count(); i++)
            {
                var report = ravenSession.Load<Report>(command.ReportIds[i]);
                if (report != null)
                {
                    var @event = new ChangeReportListOrderEvent(i);
                    report.ListOrder = i;
                }
            }

            for (var i = 0; i < (command.GroupIds ?? new string[0]).Count(); i++)
            {
                var group = ravenSession.Load<ReportGroup>(command.GroupIds[i]);
                if (group != null)
                {
                    group.ListOrder = i;
                }
            }


            ravenSession.SaveChanges();
        }
    }
}