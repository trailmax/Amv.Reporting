using AmvReporting.Domain.DatabaseConnections.Queries;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Domain.DatabaseConnections.Commands
{
    public class EditDatabaseDetailsCommandValidator : ICommandValidator<EditDatabaseDetailsCommand>
    {
        private readonly IMediator mediator;
        public ErrorList Errors { get; private set; }

        public EditDatabaseDetailsCommandValidator(IMediator mediator)
        {
            this.mediator = mediator;

            Errors = new ErrorList();
        }

        public bool IsValid(EditDatabaseDetailsCommand command)
        {
            var connectionChecker = mediator.Request(new CheckDatabaseConnectivityQuery(command.ConnectionString));

            if (!connectionChecker.IsConnected)
            {
                Errors.Add("ConnectionString", connectionChecker.ExceptionMessage);
            }

            return Errors.IsValid();
        }
    }
}