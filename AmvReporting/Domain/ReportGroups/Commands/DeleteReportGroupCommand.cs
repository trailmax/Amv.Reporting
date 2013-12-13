using System;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.ReportGroups.Commands
{
    public class DeleteReportGroupCommand : ICommand
    {
        public String Id { get; set; }
    }

    public class DeleteReportGroupCommandHandler : ICommandHandler<DeleteReportGroupCommand>
    {
        private readonly IDocumentSession ravenSession;

        public DeleteReportGroupCommandHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public void Handle(DeleteReportGroupCommand command)
        {
            var reportGroup = ravenSession.Load<ReportGroup>(command.Id);
            if (reportGroup == null)
            {
                return;
            }

            ravenSession.Delete(reportGroup);
            ravenSession.SaveChanges();
        }
    }
}