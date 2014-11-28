using System;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;


namespace AmvReporting.Domain.Reports.Commands
{
    public class CloneReportCommand : ICommand
    {
        public CloneReportCommand(Guid aggregateId, Guid newId)
        {
            AggregateId = aggregateId;
            NewAggregateId = newId;
        }


        public Guid AggregateId { get; private set; }
        public Guid NewAggregateId { get; private set; }
    }

    public class CloneReportCommandHandler : ICommandHandler<CloneReportCommand>
    {
        private readonly IRepository repository;


        public CloneReportCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }


        public void Handle(CloneReportCommand command)
        {
            var report = repository.GetById<ReportAggregate>(command.AggregateId);

            var newReport = new ReportAggregate(command.NewAggregateId,
                                                report.ReportGroupId,
                                                report.Title,
                                                report.ReportType,
                                                report.Description,
                                                report.DatabaseId,
                                                report.Enabled);
            newReport.UpdateCode(report.Sql, report.JavaScript, report.Css, report.HtmlOverride);
            newReport.SetListOrder(report.ListOrder ?? 0);

            var commitId = Guid.NewGuid();
            repository.Save(newReport, commitId);
        }
    }
}