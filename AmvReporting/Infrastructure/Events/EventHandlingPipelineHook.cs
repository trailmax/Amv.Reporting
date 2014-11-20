using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Autofac;
using NEventStore;
using NEventStore.Persistence;


namespace AmvReporting.Infrastructure.Events
{
    public class EventHandlingPipelineHook : IPipelineHook
    {
        private readonly ILifetimeScope container;


        public EventHandlingPipelineHook(ILifetimeScope container)
        {
            this.container = container;
        }




        public ICommit Select(ICommit committed)
        {
            return committed;
        }


        public bool PreCommit(CommitAttempt attempt)
        {
            foreach (var eventMessage in attempt.Events)
            {
                Thread.Sleep(10);
                eventMessage.Headers.Add("DateTime", DateTime.Now);
                eventMessage.Headers.Add("CommitId", attempt.CommitId);
                eventMessage.Headers.Add("CommitSequence", attempt.CommitSequence);
            }

            return true;
        }


        public void PostCommit(ICommit committed)
        {
            foreach (var eventMessage in committed.Events)
            {
                var @event = eventMessage.Body;
                var eventType = @event.GetType();

                Type handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
                Type allHandlerTypes = typeof(IEnumerable<>).MakeGenericType(handlerType);
                var handlers = container.Resolve(allHandlerTypes);
                foreach (var handler in (IEnumerable)handlers)
                {
                    var method = handler.GetType().GetMethod("Handle", new[] { eventType });
                    method.Invoke(handler, new [] { @event });
                }
            }
        }


        public void OnPurge(string bucketId)
        {
        }


        public void OnDeleteStream(string bucketId, string streamId)
        {
        }


        public void Dispose()
        {
            container.Dispose();
        }
    }
}