using System;
using System.Linq;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.ReportGroups.Commands
{
    public class DeleteReportGroupCommand : ICommand
    {
        public String Id { get; set; }
    }


    public class DeleteReportGroupCommandValidator : ICommandValidator<DeleteReportGroupCommand>
    {
        private readonly IDocumentSession ravenSession;

        public DeleteReportGroupCommandValidator(IDocumentSession ravenSession)
        {
            Errors = new ErrorList();
            this.ravenSession = ravenSession;
        }

        public ErrorList Errors { get; private set; }
        public bool IsValid(DeleteReportGroupCommand command)
        {
            var childReports = ravenSession.Query<Report>().Where(r => r.ReportGroupId == command.Id).ToList();
            var childGroups = ravenSession.Query<ReportGroup>().Where(r => r.ParentReportGroupId == command.Id).ToList();

            if (childGroups.Any() || childReports.Any())
            {
                Errors.Add("Group contains child items. Remove all child items before deleting the parent");
            }

            return Errors.IsValid();
        }
    }


    public class DeleteReportGroupCommandHandler : ICommandHandler<DeleteReportGroupCommand>
    {
        private readonly IDocumentSession ravenSession;

        public DeleteReportGroupCommandHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public void Handle(DeleteReportGroupCommand command)
        {
            var reportGroup = ravenSession.Load<ReportGroup>(command.Id);
            if (reportGroup == null)
            {
                return;
            }

            ravenSession.Delete(reportGroup);
            ravenSession.SaveChanges();
        }
    }
}