using System;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.ReportGroups.Commands
{
    public class CreateReportGroupCommand : ICommand
    {
        public String Title { get; set; }
    }

    public class CreateReportGroupCommandHandler : ICommandHandler<CreateReportGroupCommand>
    {
        private readonly IDocumentSession ravenSession;

        public CreateReportGroupCommandHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public void Handle(CreateReportGroupCommand command)
        {
            var reportGroup = new ReportGroup()
                              {
                                  Title = command.Title,
                              };

            ravenSession.Store(reportGroup);
            ravenSession.SaveChanges();
        }
    }
}