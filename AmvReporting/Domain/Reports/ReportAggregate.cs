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


        public ReportAggregate(Guid id, String reportGroupId, String title, String description, String databaseId, bool isEnabled)
            : this(id)
        {
            RaiseEvent(new ReportCreatedEvent(Id, reportGroupId, title, description, databaseId, isEnabled));
        }
        private void Apply(ReportCreatedEvent @event)
        {
            Id = @event.AggregateId;
            ReportGroupId = @event.ReportGroupId;
            Title = @event.Title;
            Description = @event.Description;
            DatabaseId = @event.DatabaseId;
            Enabled = @event.Enabled;
        }


        public String ReportGroupId { get; private set; }

        public String Title { get; private set; }

        public Guid? TemplateId { get; private set; }

        public String Description { get; private set; }

        public String Sql { get; private set; }

        public String JavaScript { get; private set; }

        [AllowHtml]
        public String HtmlOverride { get; private set; }

        public String DatabaseId { get; private set; }

        public bool Enabled { get; private set; }

        public int? ListOrder { get; private set; }


        public void UpdateMetadata(String reportGroupId, String title, String description, String databaseId, bool isEnabled)
        {
            RaiseEvent(new UpdateReportMetadaEvent(this.Id, reportGroupId, title, description, databaseId, isEnabled));
        }
        private void Apply(UpdateReportMetadaEvent @event)
        {
            ReportGroupId = @event.ReportGroupId;
            Title = @event.Title;
            Description = @event.Description;
            DatabaseId = @event.DatabaseId;
            Enabled = @event.Enabled;
        }


        public void UpdateCode(Guid? templateId, string sql, string javaScript, string htmlOverride)
        {
            var @event = new ReportCodeUpdatedEvent(this.Id, templateId, sql, javaScript, htmlOverride);
            RaiseEvent(@event);
        }
        private void Apply(ReportCodeUpdatedEvent @event)
        {
            TemplateId = @event.TemplateId;
            Sql = @event.Sql;
            JavaScript = @event.JavaScript;
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