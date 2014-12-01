using System;
using System.ComponentModel;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Templates.Events
{
    [Serializable]
    [Description("Created template")]
    public class CreateTemplateEvent : IEvent
    {
        public CreateTemplateEvent(Guid aggregateId, String title, string javaScript, string html)
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