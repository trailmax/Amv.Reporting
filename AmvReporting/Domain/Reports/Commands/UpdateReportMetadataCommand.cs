using System;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;


namespace AmvReporting.Domain.Reports.Commands
{
    public class UpdateReportMetadataCommand : ICommand
    {
        public Guid AggregateId { get; private set; }

        public String ReportGroupId { get; set; }

        public String Title { get; set; }

        public ReportType ReportType { get; set; }

        public String Description { get; set; }

        public String DatabaseId { get; private set; }

        public bool Enabled { get; private set; }
    }


    public class UpdateMetadataCommandHandler : ICommandHandler<UpdateReportMetadataCommand>
    {
        private readonly IRepository repository;


        public UpdateMetadataCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }


        public void Handle(UpdateReportMetadataCommand command)
        {
            var report = repository.GetById<ReportAggregate>(command.AggregateId);

            report.UpdateMetadata(command.ReportGroupId, command.Title, command.ReportType, command.Description, command.DatabaseId, command.Enabled);

            var commitId = Guid.NewGuid();
            repository.Save(report, commitId);
        }
    }
}