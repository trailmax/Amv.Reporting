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


        public TemplateAggregate(Guid id, String title, String javascript, String html) : this(id)
        {
            RaiseEvent(new CreateTemplateEvent(id, title, javascript, html));
        }
        private void Apply(CreateTemplateEvent @event)
        {
            this.Title = @event.Title;
            this.JavaScript = @event.JavaScript;
            this.Html = @event.Html;
        }


        public String Title { get; set; }

        public String JavaScript { get; private set; }

        public String Html { get; private set; }

        public void UpdateTemplate(String title, String javascript, String html)
        {
            RaiseEvent(new UpdateTemplateEvent(Id, title, javascript, html));
        }
        private void Apply(UpdateTemplateEvent @event)
        {
            this.Title = @event.Title;
            this.JavaScript = @event.JavaScript;
            this.Html = @event.Html;
        }
    }
}