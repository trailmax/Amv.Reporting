using System;
using AmvReporting.Domain.DatabaseConnections;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using CommonDomain.Persistence;
using Raven.Client;

namespace AmvReporting.Domain.Reports.Queries
{
    public class ReportResult
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Data { get; set; }
        public String JavaScript { get; set; }
        public String HtmlOverride { get; set; }
        public String GlobalJs { get; set; }
        public String Css { get; set; }
        public String GlobalCss { get; set; }
        public ReportType ReportType { get; set; }
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
        private readonly IRepository repository;

        public ReportResultQueryHandler(IDocumentSession ravenSession, IRepository repository)
        {
            this.ravenSession = ravenSession;
            this.repository = repository;
        }


        public ReportResult Handle(ReportResultQuery query)
        {
            var report = repository.GetById<ReportAggregate>(query.Id);

            if (report == null)
            {
                throw new DomainException("Unable to find report on this path");
            }

            var dbConnection = ravenSession.Load<DatabaseConnection>(report.DatabaseId);

            var result = new ReportResult()
                         {
                             Id = report.Id,
                             Title = report.Title,
                             Description = report.Description,
                             Css = report.Css,
                             JavaScript = report.JavaScript,
                             HtmlOverride = report.HtmlOverride,
                             ReportType = report.ReportType,
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