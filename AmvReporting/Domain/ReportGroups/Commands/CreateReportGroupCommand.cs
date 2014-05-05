using System;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.ReportGroups.Commands
{
    public class CreateReportGroupCommand : ICommand
    {
        public String Title { get; set; }
        public String ParentReportGroupId { get; set; }
        public bool Enabled { get; set; }
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
                                  ParentReportGroupId = command.ParentReportGroupId,
                                  Enabled = command.Enabled,
                              };

            ravenSession.Store(reportGroup);
            ravenSession.SaveChanges();
        }
    }
}