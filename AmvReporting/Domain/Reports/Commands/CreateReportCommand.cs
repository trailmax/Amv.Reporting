using System;
using System.ComponentModel.DataAnnotations;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;


namespace AmvReporting.Domain.Reports.Commands
{
    public class CreateReportCommand : ICommand
    {
        public CreateReportCommand()
        {
            AggregateId = Guid.NewGuid();
        }
        public Guid AggregateId { get; private set; }

        public String ReportGroupId { get; set; }

        [Required]
        public String Title { get; set; }

        [Required]
        public String DatabaseId { get; set; }
        
        [DataType(DataType.MultilineText)]
        public String Description { get; set; }

        public bool Enabled { get; set; }

        [Required]
        public ReportType ReportType { get; set; }

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
            var report = new ReportAggregate(command.AggregateId,
                                    command.ReportGroupId,
                                    command.Title,
                                    command.ReportType,
                                    command.Description,
                                    command.DatabaseId,
                                    command.Enabled);

            var commitId = Guid.NewGuid();
            repository.Save(report, commitId);
        }
    }
}