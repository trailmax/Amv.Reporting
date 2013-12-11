//using System.Linq;
//using AmvReporting.Infrastructure.CQRS;
//using Raven.Client;

//namespace AmvReporting.Domain.Reports.Commands
//{
//    public class CreateReportCommandValidator : ICommandValidator<CreateReportCommand>
//    {
//        private readonly IDocumentSession ravenSession;
//        public ErrorList Errors { get; private set; }

//        public CreateReportCommandValidator(IDocumentSession ravenSession)
//        {
//            this.ravenSession = ravenSession;
//            Errors = new ErrorList();
//        }


//        public bool IsValid(CreateReportCommand command)
//        {
//            var isDuplicated = ravenSession.Query<Report>().Any(r => r.LinkName == command.LinkName);

//            if (isDuplicated)
//            {
//                Errors.Add("LinkName", "Report with this link already exists. Please choose other name.");
//            }

//            return Errors.IsValid();
//        }
//    }
//}