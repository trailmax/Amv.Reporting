using System;
using AmvReporting.Domain.Templates.Events;
using CommonDomain.Core;


namespace AmvReporting.Domain.Templates
{
    public class TemplateAggregate : AggregateBase
    {
        private TemplateAggregate(Guid id)
        {
            Id = id;
        }


        public TemplateAggregate(Guid id, String title, String javascript, String html, bool allowOverrides) : this(id)
        {
            RaiseEvent(new CreateTemplateEvent(id, title, javascript, html, allowOverrides));
        }
        private void Apply(CreateTemplateEvent @event)
        {
            this.Title = @event.Title;
            this.JavaScript = @event.JavaScript;
            this.Html = @event.Html;
            this.AllowOverrides = @event.AllowOverrides;
        }


        public String Title { get; set; }

        public String JavaScript { get; private set; }

        public String Html { get; private set; }

        public bool AllowOverrides { get; private set; }

        public void UpdateTemplate(String title, String javascript, String html, bool allowOverrides)
        {
            RaiseEvent(new UpdateTemplateEvent(Id, title, javascript, html, allowOverrides));
        }
        private void Apply(UpdateTemplateEvent @event)
        {
            this.Title = @event.Title;
            this.JavaScript = @event.JavaScript;
            this.Html = @event.Html;
            this.AllowOverrides = @event.AllowOverrides;
        }
    }
}