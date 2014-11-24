//using System;
//using AmvReporting.Infrastructure.CQRS;
//using CommonDomain.Persistence;


//namespace AmvReporting.Domain.Reports.Commands
//{
//    [Obsolete]
//    public class EditReportCommand : CreateReportCommand, ICommand
//    {
//        public Guid AggregateId { get; set; }
//    }


//    [Obsolete]
//    public class EditReportCommandHandler : ICommandHandler<EditReportCommand>
//    {
//        private readonly IRepository repository;

//        public EditReportCommandHandler(IRepository repository)
//        {
//            this.repository = repository;
//        }


//        public void Handle(EditReportCommand command)
//        {
//            var report = repository.GetById<ReportAggregate>(command.AggregateId);
//            report.UpdateMetadata(command.ReportGroupId, command.Title, command.ReportType, command.Description, command.DatabaseId, command.Enabled);
//            report.UpdateCode(command.Sql, command.JavaScript, command.Css, command.HtmlOverride);

//            var commitId = Guid.NewGuid();

//            repository.Save(report, commitId);
//        }
//    }
//}