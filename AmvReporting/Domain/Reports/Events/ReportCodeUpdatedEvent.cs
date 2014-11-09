using System;
using System.Web;
using System.Web.Mvc;
using AmvReporting.Domain.Reports.Queries;
using AmvReporting.Infrastructure.Events;


namespace AmvReporting.Domain.Reports.Events
{
    [Serializable]
    public class ReportCodeUpdatedEvent : IDomainEvent
    {
        public ReportCodeUpdatedEvent(string sql, string javaScript, string css, string htmlOverride)
        {
            Sql = sql;
            JavaScript = javaScript;
            Css = css;
            HtmlOverride = htmlOverride;
        }


        public String Sql { get; private set; }

        public String JavaScript { get; private set; }

        public String Css { get; private set; }

        [AllowHtml]
        public String HtmlOverride { get; private set; }
    }


    //TODO this is broken
    //public class ReportCodeUpdatedEventHandler : IDomainEventHandler<ReportCodeUpdatedEvent>
    //{
    //    public void Handle(ReportCodeUpdatedEvent raisedEvent)
    //    {
    //        var query = new ReportResultQuery(raisedEvent.Id);
    //        HttpContext.Current.Cache.Remove(query.CacheKey);
    //    }
    //}
}