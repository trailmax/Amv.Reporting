using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.Reports.Commands
{
    public class DeleteReportCommandHandler : ICommandHandler<DeleteReportCommand>
    {
        private readonly IDocumentSession session;

        public DeleteReportCommandHandler(IDocumentSession session)
        {
            this.session = session;
        }

        public void Handle(DeleteReportCommand command)
        {
            var toBeDeleted = session.Load<Report>(command.Id);

            session.Delete(toBeDeleted);
            session.SaveChanges();
        }
    }
}