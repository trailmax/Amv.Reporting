//TODO Remove
//using System.Collections.Generic;
//using Autofac;


//namespace AmvReporting.Infrastructure.Events
//{
//    public class EventDispatcher : IDomainEventDispatcher
//    {
//        private readonly ILifetimeScope container;


//        public EventDispatcher(ILifetimeScope container)
//        {
//            this.container = container;
//        }


//        public void Dispatch<TEvent>(TEvent eventToDispatch) where TEvent : IEvent
//        {
//            var domainEventHandlers = container.Resolve<IEnumerable<IEventHandler<TEvent>>>();
//            foreach (var handler in domainEventHandlers)
//            {
//                handler.Handle(eventToDispatch);
//            }
//        }
//    }
//}