using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;


namespace AmvReporting.Domain.Reports.Commands
{
    public class UpdateReportCodeCommand : ICommand
    {
        public Guid AggregateId { get; set; }

        public Guid? TemplateId { get; set; }

        [AllowHtml]
        [Required, DataType(DataType.MultilineText)]
        public String Sql { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public String JavaScript { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public String HtmlOverride { get; set; }
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
            report.UpdateCode(command.TemplateId, command.Sql, command.JavaScript, command.HtmlOverride);

            var commitId = Guid.NewGuid();

            repository.Save(report, commitId);
        }
    }
}