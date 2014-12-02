using System;


namespace AmvReporting.Infrastructure.Events
{
    public interface IEvent
    {
        Guid AggregateId { get; }
    }
}
