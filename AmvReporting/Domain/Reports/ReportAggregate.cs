using System;
using System.Web.Mvc;
using AmvReporting.Domain.Reports.Events;
using CommonDomain.Core;


namespace AmvReporting.Domain.Reports
{
    public class ReportAggregate : AggregateBase
    {
        private ReportAggregate(Guid id)
        {
            Id = id;
        }


        public ReportAggregate(Guid id, String reportGroupId, String title, ReportType reportType, String description, String databaseId, bool isEnabled)
            : this(id)
        {
            RaiseEvent(new ReportCreatedEvent(Id, reportGroupId, title, reportType, description, databaseId, isEnabled));
        }
        private void Apply(ReportCreatedEvent @event)
        {
            Id = @event.AggregateId;
            ReportGroupId = @event.ReportGroupId;
            Title = @event.Title;
            ReportType = @event.ReportType;
            Description = @event.Description;
            DatabaseId = @event.DatabaseId;
            Enabled = @event.IsEnabled;
        }


        public String ReportGroupId { get; private set; }

        public String Title { get; private set; }

        public ReportType ReportType { get; private set; }

        public String Description { get; private set; }

        public String Sql { get; private set; }

        public String JavaScript { get; private set; }

        public String Css { get; private set; }

        [AllowHtml]
        public String HtmlOverride { get; private set; }

        public String DatabaseId { get; private set; }

        public bool Enabled { get; private set; }

        public int? ListOrder { get; private set; }


        public void UpdateMetadata(String reportGroupId, String title, ReportType reportType, String description, String databaseId, bool isEnabled)
        {
            RaiseEvent(new UpdateReportMetadaEvent(this.Id, reportGroupId, title, reportType, description, databaseId, isEnabled));
        }
        private void Apply(UpdateReportMetadaEvent @event)
        {
            ReportGroupId = @event.ReportGroupId;
            Title = @event.Title;
            ReportType = @event.ReportType;
            Description = @event.Description;
            DatabaseId = @event.DatabaseId;
        }


        public void UpdateCode(string sql, string javaScript, string css, string htmlOverride)
        {
            var @event = new ReportCodeUpdatedEvent(this.Id, sql, javaScript, css, htmlOverride);
            RaiseEvent(@event);
        }
        private void Apply(ReportCodeUpdatedEvent @event)
        {
            Sql = @event.Sql;
            JavaScript = @event.JavaScript;
            Css = @event.Css;
            HtmlOverride = @event.HtmlOverride;
        }


        public void SetListOrder(int listOrder)
        {
            RaiseEvent(new ChangeReportListOrderEvent(this.Id, listOrder));
        }
        private void Apply(ChangeReportListOrderEvent @event)
        {
            ListOrder = @event.ListOrder;
        }


        public void Delete()
        {
            RaiseEvent(new DeleteReportEvent(Id));
        }


        private void Apply(DeleteReportEvent @event)
        {
            // do nothing here
        }
    }
}