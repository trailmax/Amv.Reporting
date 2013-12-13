using AmvReporting.Domain.DatabaseConnections.Queries;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Domain.DatabaseConnections.Commands
{
    public class CreateDatabaseDetailsCommandValidator : ICommandValidator<CreateDatabaseDetailsCommand>
    {
        private readonly IMediator mediator;
        public ErrorList Errors { get; private set; }

        public CreateDatabaseDetailsCommandValidator(IMediator mediator)
        {
            this.mediator = mediator;

            Errors = new ErrorList();
        }

        public bool IsValid(CreateDatabaseDetailsCommand command)
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