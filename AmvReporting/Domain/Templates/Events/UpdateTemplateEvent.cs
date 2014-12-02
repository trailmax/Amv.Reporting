using System;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Templates.Events
{
    public class UpdateTemplateEvent : IEvent
    {
        public UpdateTemplateEvent(Guid aggregateId, string title, string javaScript, string html)
        {
            Title = title;
            Html = html;
            JavaScript = javaScript;
            AggregateId = aggregateId;
        }

        public Guid AggregateId { get; set; }

        public String Title { get; set; }

        public String JavaScript { get; set; }

        public String Html { get; set; }
    }
}