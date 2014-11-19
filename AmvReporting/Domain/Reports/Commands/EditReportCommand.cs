using System;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;


namespace AmvReporting.Domain.Reports.Commands
{
    public class EditReportCommand : CreateReportCommand, ICommand
    {
        public Guid Id { get; set; }
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
            var report = repository.GetById<ReportAggregate>(command.Id);
            report.UpdateMetadata(command.ReportGroupId, command.Title, command.ReportType, command.Description, command.DatabaseId);
            report.UpdateCode(command.Sql, command.JavaScript, command.Css, command.HtmlOverride);

            var commitId = Guid.NewGuid();
            repository.Save(report, commitId);
        }
    }
}