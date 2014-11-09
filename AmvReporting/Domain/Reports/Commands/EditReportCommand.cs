using System;
using AmvReporting.Domain.Reports.Events;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Events;
using CommonDomain.Persistence;
using Raven.Client;

namespace AmvReporting.Domain.Reports.Commands
{
    public class EditReportCommand : CreateReportCommand, ICommand
    {
        public String Id { get; set; }
    }


    public class EditReportCommandHandler : ICommandHandler<EditReportCommand>
    {
        private readonly IRepository repository;

        public EditReportCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }


        public void Handle(EditReportCommand command)
        {
            //TODO complete 
            //var account = repository.GetById<Account>(command.AccountId);
            report.UpdateMetadata();
            report.CodeUpdate();
        }

        //public void Handle(EditReportCommand command)
        //{
        //    var report = ravenSession.Load<Report>(command.Id);

        //    UpdateReport(command, report);

        //    ravenSession.SaveChanges();
        //}


        //public Report UpdateReport(EditReportCommand command, Report report)
        //{
        //    DomainEvents.Raise(new ReportUpdatedEvent(command.Id));

        //    report.Title = command.Title;
        //    report.ReportType = command.ReportType;
        //    report.Description = command.Description;
        //    report.Sql = command.Sql;
        //    report.JavaScript = command.JavaScript;
        //    report.Css = command.Css;
        //    report.DatabaseId = command.DatabaseId;
        //    report.ReportGroupId = command.ReportGroupId;
        //    report.Enabled = command.Enabled;
        //    report.HtmlOverride = command.HtmlOverride;

        //    return report;
        //}
    }
}