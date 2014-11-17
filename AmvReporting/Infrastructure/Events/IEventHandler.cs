namespace AmvReporting.Infrastructure.Events
{
    public interface IEventHandler<in TEvent>
    {
        void Handle(TEvent raisedEvent);
    }
}
