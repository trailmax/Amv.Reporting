//TODO REMOVE
//using System.Linq;
//using AmvReporting.Infrastructure.CQRS;
//using Raven.Client;

//namespace AmvReporting.Domain.Reports.Commands
//{
//    public class EditReportCommandValidator : ICommandValidator<EditReportCommand>
//    {
//        private readonly IDocumentSession ravenSession;
//        public ErrorList Errors { get; private set; }

//        public EditReportCommandValidator(IDocumentSession ravenSession)
//        {
//            this.ravenSession = ravenSession;
//            Errors = new ErrorList();
//        }


//        public bool IsValid(EditReportCommand command)
//        {
//            var isDuplicated = ravenSession.Query<Report>().Any(r => r.LinkName == command.LinkName && r.Id != command.Id);

//            if (isDuplicated)
//            {
//                Errors.Add("LinkName", "Report with this link already exists. Please choose other name.");
//            }

//            return Errors.IsValid();
//        }
//    }
//}