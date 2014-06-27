namespace AmvReporting.Infrastructure.Events
{
    public interface IDomainEventHandler<in TEvent>
    {
        void Handle(TEvent raisedEvent);
    }
}
