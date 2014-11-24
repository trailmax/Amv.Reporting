using System;
using System.Web.Mvc;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;


namespace AmvReporting.Domain.Reports.Commands
{
    public class UpdateReportCodeCommand : ICommand
    {
        public Guid AggregateId { get; set; }

        public String Sql { get; private set; }

        public String JavaScript { get; private set; }

        public String Css { get; private set; }

        [AllowHtml]
        public String HtmlOverride { get; private set; }
    }


    public class UpdateReportCodeCommandHandler : ICommandHandler<UpdateReportCodeCommand>
    {
        private readonly IRepository repository;

        public UpdateReportCodeCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }


        public void Handle(UpdateReportCodeCommand command)
        {
            var report = repository.GetById<ReportAggregate>(command.AggregateId);
            report.UpdateCode(command.Sql, command.JavaScript, command.Css, command.HtmlOverride);

            var commitId = Guid.NewGuid();

            repository.Save(report, commitId);
        }
    }
}