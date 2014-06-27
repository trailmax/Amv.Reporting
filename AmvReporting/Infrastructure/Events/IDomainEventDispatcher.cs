namespace AmvReporting.Infrastructure.Events
{
    public interface IDomainEventDispatcher
    {
        void Dispatch<TEvent>(TEvent eventToDispatch) where TEvent : IDomainEvent;
    }
}
