using System;
using System.Linq;
using System.Web.Mvc;
using AmvReporting.Domain.ReportingConfigs.Queries;
using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Templates.Events;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using Raven.Client;


namespace AmvReporting.Domain.Preview.Queries
{
    public class ReportResultPreview
    {
        public String Data { get; set; }
        public String ReportJavaScript { get; set; }
        public String ReportHtml { get; set; }
        public String GlobalJs { get; set; }
        public String GlobalCss { get; set; }
        public String TemplateJavascript { get; set; }
        public String TemplateHtml { get; set; }

        public bool IsSuccess { get; set; }
        public String ExceptionMessage { get; set; }
    }


    public class PreviewReportQuery : IQuery<ReportResultPreview>
    {
        public Guid AggregateId { get; set; }

        public String Sql { get; set; }

        [AllowHtml]
        public String JavaScript { get; set; }

        [AllowHtml]
        public String HtmlOverride { get; set; }

        public Guid TemplateId { get; set; }
    }


    public class PreviewReportQueryHandler : IQueryHandler<PreviewReportQuery, ReportResultPreview>
    {
        private readonly IMediator mediator;
        private readonly IDocumentSession ravenSession;

        public PreviewReportQueryHandler(IMediator mediator, IDocumentSession ravenSession)
        {
            this.mediator = mediator;
            this.ravenSession = ravenSession;
        }


        public ReportResultPreview Handle(PreviewReportQuery query)
        {
            var report = ravenSession.Query<ReportViewModel>()
                                     .FirstOrDefault(r => r.AggregateId == query.AggregateId);
            if (report == null)
            {
                return new ReportResultPreview()
                {
                    IsSuccess = false,
                    ExceptionMessage = "Unable to find report with this ID",
                };
            }

            var template = ravenSession.Query<TemplateViewModel>()
                                       .FirstOrDefault(t => t.AggregateId == query.TemplateId);

            var previewData = mediator.Request(new PreviewDataQuery(query.AggregateId, query.Sql));
            var config = mediator.Request(new ReportingConfigQuery());


            var outModel = new ReportResultPreview()
                           {
                               Data = previewData.Data,
                               ReportJavaScript = query.JavaScript,
                               ReportHtml = query.HtmlOverride,
                               TemplateJavascript = template.CheckForNull(t => t.JavaScript),
                               TemplateHtml = template.CheckForNull(t => t.Html),
                               GlobalJs = config.GlobalJavascript,
                               GlobalCss = config.GlobalCss,
                           };
            return outModel;
        }
    }
}