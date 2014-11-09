using System;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;


namespace AmvReporting.Domain.Reports.Commands
{
    public class CreateReportCommandHandler : ICommandHandler<CreateReportCommand>
    {
        private readonly IRepository repository;

        public CreateReportCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }


        public void Handle(CreateReportCommand command)
        {
            var newId = Guid.NewGuid();
            command.RedirectingId = newId.ToString();
            
            var report = new Report(newId,
                                    command.ReportGroupId,
                                    command.Title,
                                    command.ReportType,
                                    command.Description,
                                    command.DatabaseId);

            report.UpdateCode(command.Sql, command.JavaScript, command.Css, command.HtmlOverride);
            report.EnableReport();
            report.SetListOrder(0);

            var commitId = Guid.NewGuid();
            repository.Save(report, commitId);
        }
    }
}