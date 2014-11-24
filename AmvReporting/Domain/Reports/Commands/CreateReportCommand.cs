using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;


namespace AmvReporting.Domain.Reports.Commands
{
    public class CreateReportCommand : IRedirectingCommand
    {
        [Required]
        public String Title { get; set; }

        [Required]
        public ReportType ReportType { get; set; }

        public String ReportGroupId { get; set; }

        [DataType(DataType.MultilineText)]
        public String Description { get; set; }

        [AllowHtml]
        [Required, DataType(DataType.MultilineText)]
        public String Sql { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public String JavaScript { get; set; }

        [DataType(DataType.MultilineText)]
        public String Css { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public String HtmlOverride { get; set; }


        [Required]
        public String DatabaseId { get; set; }

        public bool Enabled { get; set; }

        public Guid RedirectingId { get; set; }
    }


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
            command.RedirectingId = newId;

            var report = new ReportAggregate(newId,
                                    command.ReportGroupId,
                                    command.Title,
                                    command.ReportType,
                                    command.Description,
                                    command.DatabaseId,
                                    command.Enabled);

            report.UpdateCode(command.Sql, command.JavaScript, command.Css, command.HtmlOverride);
            report.SetListOrder(0);

            var commitId = Guid.NewGuid();
            repository.Save(report, commitId);
        }
    }
}