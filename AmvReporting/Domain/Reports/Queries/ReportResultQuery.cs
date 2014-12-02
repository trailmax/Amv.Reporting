using System;
using System.Linq;
using AmvReporting.Domain.DatabaseConnections;
using AmvReporting.Domain.ReportGroups;
using AmvReporting.Domain.Templates.Events;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using Raven.Client;


namespace AmvReporting.Domain.Reports.Queries
{
    public class ReportResult
    {
        public Guid AggregateId { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Data { get; set; }
        public String ReportJavaScript { get; set; }
        public String ReportHtml { get; set; }
        public String TemplateJavascript { get; set; }
        public String TemplateHtml { get; set; }
        public String ReportGroupName { get; set; }
    }


    public class ReportResultQuery : IQuery<ReportResult>, ICachedQuery
    {
        public Guid Id { get; set; }

        public ReportResultQuery(Guid id)
        {
            this.Id = id;
        }


        public string CacheKey
        {
            get { return "ReportsResultQuery_" + Id.ToString(); } 
        }

        public int CacheDurationMinutes
        {
            get { return 60; }
        }
    }

    public class ReportResultQueryHandler : IQueryHandler<ReportResultQuery, ReportResult>
    {
        private readonly IDocumentSession ravenSession;

        public ReportResultQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }


        public ReportResult Handle(ReportResultQuery query)
        {
            var report = ravenSession.Query<ReportViewModel>()
                .Include<ReportViewModel>(r => r.DatabaseId)
                .FirstOrDefault(r => r.AggregateId == query.Id);

            if (report == null)
            {
                throw new DomainException("Unable to find report with this id");
            }

            var dbConnection = ravenSession.Load<DatabaseConnection>(report.DatabaseId);
            var reportGroup = ravenSession.Load<ReportGroup>(report.ReportGroupId);
            var template = ravenSession.Query<TemplateViewModel>()
                                       .SingleOrDefault(t => t.AggregateId == report.TemplateId);

            var result = new ReportResult()
                         {
                             AggregateId = report.AggregateId,
                             Title = report.Title,
                             Description = report.Description,
                             ReportJavaScript = report.JavaScript,
                             ReportHtml = report.HtmlOverride,
                             TemplateJavascript = template.CheckForNull(t => t.JavaScript),
                             TemplateHtml = template.CheckForNull(t => t.Html),
                             ReportGroupName = reportGroup.CheckForNull(g => g.Title),
                         };

            using (var sqlServerHelper = new SqlServerHelper())
            {
                var dataReader = sqlServerHelper.ExecuteQuery(report.Sql, dbConnection.ConnectionString);
                result.Data = SqlDataSerialiserHelper.GetDataWithColumnsJson(dataReader);
            }

            return result;
        }
    }
}