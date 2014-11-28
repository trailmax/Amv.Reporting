using System;
using System.ComponentModel;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Templates.Events
{
    [Serializable]
    [Description("Created template")]
    public class CreateTemplateEvent : IEvent
    {
        public CreateTemplateEvent(Guid aggregateId, String title, string javaScript, string html, bool allowOverrides)
        {
            Title = title;
            AllowOverrides = allowOverrides;
            Html = html;
            JavaScript = javaScript;
            AggregateId = aggregateId;
        }


        public Guid AggregateId { get; private set; }

        public String Title { get; set; }

        public String JavaScript { get; private set; }

        public String Html { get; private set; }

        public bool AllowOverrides { get; private set; }
    }
}