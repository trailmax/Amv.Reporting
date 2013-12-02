using System;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Models;
using Raven.Client;

namespace AmvReporting.Commands
{
    public class DeleteReportCommand : ICommand
    {
        public String Id { get; set; }
    }

    public class DeleteReportCommandHandler : ICommandHandler<DeleteReportCommand>
    {
        private readonly IDocumentSession session;

        public DeleteReportCommandHandler(IDocumentSession session)
        {
            this.session = session;
        }

        public void Handle(DeleteReportCommand command)
        {
            var toBeDeleted = session.Load<ReportDetails>(command.Id);

            session.Delete(toBeDeleted);
            session.SaveChanges();
        }
    }
}