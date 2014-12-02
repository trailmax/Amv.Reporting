using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using AmvReporting.Infrastructure.Events;
using Autofac;
using NEventStore;


namespace AmvReporting.Infrastructure.NEventStore
{
    public static class MessageHeaders
    {
        public const String CommitDate = "CommitDate";
        public const String Username = "Username";
        public const String DateFormat = "dd/MM/yyyy HH:mm:ss.fffffff";
    }


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
            var principal = container.Resolve<IPrincipal>();

            foreach (var eventMessage in attempt.Events)
            {
                eventMessage.Headers.Add(MessageHeaders.CommitDate, DateTime.Now.ToString(MessageHeaders.DateFormat));
                eventMessage.Headers.Add(MessageHeaders.Username, principal.Identity.Name);
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