namespace AmvReporting.Infrastructure.CQRS
{
    public interface ICommandHandler<in TCommand>
    {
        void Handle(TCommand command);
    }
}