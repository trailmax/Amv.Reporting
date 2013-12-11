using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.Reports.Commands
{
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

        public Report CreateReportDetails(CreateReportCommand command)
        {
            var result = new Report()
                         {
                             Title = command.Title,
                             ReportType = command.ReportType,
                             Description = command.Description,
                             Sql = command.Sql,
                             JavaScript = command.JavaScript,
                             Css = command.Css,
                             DatabaseId = command.DatabaseId
                         };

            return result;
        }
    }
}