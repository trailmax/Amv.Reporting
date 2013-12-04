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
            var report = ravenSession.Load<Report>(command.Id);

            UpdateReport(command, report);

            ravenSession.SaveChanges();
        }

        public Report UpdateReport(EditReportCommand command, Report report)
        {
            report.Title = command.Title;
            report.LinkName = command.LinkName;
            report.ReportType = command.ReportType;
            report.Description = command.Description;
            report.Sql = command.Sql;
            report.JavaScript = command.JavaScript;
            report.Css = command.Css;
            report.DatabaseId = command.DatabaseId;

            return report;
        }
    }
}