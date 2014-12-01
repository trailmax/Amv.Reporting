using System;
using System.ComponentModel;
using System.Web.Mvc;
using AmvReporting.Domain.Reports.Queries;
using AmvReporting.Infrastructure.Caching;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    [Serializable]
    [Description("Updated source codes")]
    public class ReportCodeUpdatedEvent : IEvent
    {
        public ReportCodeUpdatedEvent(Guid aggregateId, Guid? templateId, string sql, string javaScript, string htmlOverride)
        {
            AggregateId = aggregateId;
            TemplateId = templateId;
            Sql = sql;
            JavaScript = javaScript;
            HtmlOverride = htmlOverride;
        }


        public Guid? TemplateId { get; set; }

        public String Sql { get; private set; }

        public String JavaScript { get; private set; }

        [AllowHtml]
        public String HtmlOverride { get; private set; }

        public Guid AggregateId { get; private set; }
    }


    public class ReportCodeUpdatedEventHandler : IEventHandler<ReportCodeUpdatedEvent>
    {
        private readonly ICacheProvider cacheProvider;


        public ReportCodeUpdatedEventHandler(ICacheProvider cacheProvider)
        {
            this.cacheProvider = cacheProvider;
        }


        public void Handle(ReportCodeUpdatedEvent raisedEvent)
        {
            var query = new ReportResultQuery(raisedEvent.AggregateId);
            cacheProvider.Invalidate(query.CacheKey);
        }
    }
}