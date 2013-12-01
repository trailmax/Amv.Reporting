using System;
using System.Linq;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Models;
using Raven.Client;

namespace AmvReporting.Commands
{
    public class EditReportCommand : CreateReportCommand, ICommand
    {
        public String Id { get; set; }
    }

    public class EditReportCommandHandler : ICommandHandler<EditReportCommand>
    {
        private readonly IDocumentSession ravenSession;

        public EditReportCommandHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public void Handle(EditReportCommand command)
        {
            var report = ravenSession.Load<ReportDetails>(command.Id);

            UpdateReport(command, report);

            ravenSession.SaveChanges();
        }

        public ReportDetails UpdateReport(EditReportCommand command, ReportDetails reportDetails)
        {
            reportDetails.Title = command.Title;
            reportDetails.LinkName = command.LinkName;
            reportDetails.ReportType = command.ReportType;
            reportDetails.Description = command.Description;
            reportDetails.Sql = command.Sql;
            reportDetails.JavaScript = command.JavaScript;
            reportDetails.Css = command.Css;

            return reportDetails;
        }
    }
}