using System;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;
using Raven.Client;


namespace AmvReporting.Domain.Reports.Commands
{
    public class DeleteReportCommand : ICommand
    {
        public Guid AggregateId { get; set; }
    }

    public class DeleteReportCommandHandler : ICommandHandler<DeleteReportCommand>
    {
        private readonly IRepository repository;

        public DeleteReportCommandHandler(IDocumentSession session, IRepository repository)
        {
            this.repository = repository;
        }


        public void Handle(DeleteReportCommand command)
        {
            var report = repository.GetById<ReportAggregate>(command.AggregateId);
            report.Delete();
            repository.Save(report, Guid.NewGuid());
        }
    }
}