using System;
using System.ComponentModel.DataAnnotations;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Models;
using Raven.Client;

namespace AmvReporting.Commands
{
    public class CreateReportCommand : ICommand
    {
        [Required]
        public String Title { get; set; }

        [Required]
        public String LinkName { get; set; }

        [Required]
        public ReportType ReportType { get; set; }

        [DataType(DataType.MultilineText)]
        public String Description { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public String Sql { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public String JavaScript { get; set; }

        [DataType(DataType.MultilineText)]
        public String Css { get; set; }
    }

    public class CreateReportCommandHandler : ICommandHandler<CreateReportCommand>
    {
        private readonly IDocumentSession ravenSession;

        public CreateReportCommandHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public void Handle(CreateReportCommand command)
        {
            var report = CreateReportDetails(command);

            ravenSession.Store(report);
            ravenSession.SaveChanges();
        }

        public ReportDetails CreateReportDetails(CreateReportCommand command)
        {
            var result = new ReportDetails()
                         {
                             Title = command.Title,
                             LinkName = command.LinkName,
                             ReportType = command.ReportType,
                             Description = command.Description,
                             Sql = command.Sql,
                             JavaScript = command.JavaScript,
                             Css = command.Css,
                         };

            return result;
        }
    }
}