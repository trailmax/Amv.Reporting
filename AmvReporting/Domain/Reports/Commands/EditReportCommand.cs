using System;
using System.Collections.Generic;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;


namespace AmvReporting.Domain.Reports.Commands
{
    public class EditReportCommand : CreateReportCommand, ICommand
    {
        public Guid AggregateId { get; set; }
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
            var report = repository.GetById<ReportAggregate>(command.AggregateId);
            report.UpdateMetadata(command.ReportGroupId, command.Title, command.ReportType, command.Description, command.DatabaseId);
            report.UpdateCode(command.Sql, command.JavaScript, command.Css, command.HtmlOverride);
            report.SetReportEnabled(command.Enabled);

            var commitId = Guid.NewGuid();

            repository.Save(report, commitId);
        }
    }
}