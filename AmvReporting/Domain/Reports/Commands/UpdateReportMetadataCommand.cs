using System;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;


namespace AmvReporting.Domain.Reports.Commands
{
    public class UpdateReportMetadataCommand : CreateReportCommand
    {
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

            report.UpdateMetadata(command.ReportGroupId, command.Title, command.Description, command.DatabaseId, command.Enabled);

            var commitId = Guid.NewGuid();
            repository.Save(report, commitId);
        }
    }
}