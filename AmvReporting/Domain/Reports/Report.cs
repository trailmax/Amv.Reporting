using System;
using System.Web.Mvc;
using AmvReporting.Domain.Reports.Events;
using CommonDomain.Core;


namespace AmvReporting.Domain.Reports
{
    public class Report : AggregateBase
    {
        public Report(Guid id, String reportGroupId, String title, ReportType reportType, String description, String databaseId) : this(id)
        {
            RaiseEvent(new ReportCreatedEvent(Id, reportGroupId, title, reportType, description, databaseId));
        }
        private void Apply(ReportCreatedEvent @event)
        {
            Id = @event.Id;
            ReportGroupId = @event.ReportGroupId;
            Title = @event.Title;
            ReportType = @event.ReportType;
            Description = @event.Description;
            DatabaseId = @event.DatabaseId;
        }


        private Report(Guid id)
        {
            Id = id;
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


        public void UpdateMetadata(String reportGroupId, String title, ReportType reportType, String description, String databaseId)
        {
            RaiseEvent(new UpdateReportMetadaEvent(reportGroupId, title, reportType, description, databaseId));
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
            var @event = new ReportCodeUpdatedEvent(sql, javaScript, css, htmlOverride);
            RaiseEvent(@event);
        }
        private void Apply(ReportCodeUpdatedEvent @event)
        {
            Sql = @event.Sql;
            JavaScript = @event.JavaScript;
            Css = @event.Css;
            HtmlOverride = @event.HtmlOverride;
        }


        public void EnableReport()
        {
            RaiseEvent(new EnableReportEvent());
        }
        private void Apply(EnableReportEvent @event)
        {
            Enabled = true;
        }


        public void DisableReport()
        {
            RaiseEvent(new DisableReportEvent());
        }
        private void Apply(DisableReportEvent @event)
        {
            Enabled = false;
        }


        public void SetListOrder(int listOrder)
        {
            RaiseEvent(new ChangeReportListOrderEvent(listOrder));
        }
        private void Apply(ChangeReportListOrderEvent @event)
        {
            ListOrder = @event.ListOrder;
        }
    }
}