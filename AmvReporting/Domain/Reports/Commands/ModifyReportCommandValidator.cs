using System.Linq;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.Reports.Commands
{
    public class ModifyReportCommandValidator : ICommandValidator<CreateReportCommand>, ICommandValidator<EditReportCommand>
    {
        private readonly IDocumentSession ravenSession;
        public ErrorList Errors { get; private set; }

        public ModifyReportCommandValidator(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
            Errors = new ErrorList();
        }


        public bool IsValid(CreateReportCommand command)
        {
            return ValidateCommand(command);
        }


        public bool IsValid(EditReportCommand command)
        {
            return ValidateCommand(command);
        }


        private bool ValidateCommand(CreateReportCommand command)
        {
            var allReports = ravenSession.Query<Report>().ToList();

            var isDuplicated = ravenSession.Query<Report>().Any(r => r.LinkName == command.LinkName);

            if (isDuplicated)
            {
                Errors.Add("LinkName", "Report with this link already exists. Please choose other name.");
            }

            return Errors.IsValid();
        }
    }
}