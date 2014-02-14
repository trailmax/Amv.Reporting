using System;
using AmvReporting.Domain.DatabaseConnections;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using Raven.Client;

namespace AmvReporting.Domain.Reports.Queries
{
    public class ReportResult
    {
        public String Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Data { get; set; }
        public String JavaScript { get; set; }
        public String Css { get; set; }
        public ReportType ReportType { get; set; }
    }


    public class ReportResultQuery : IQuery<ReportResult>
    {
        public String Id { get; set; }

        public ReportResultQuery(String id)
        {
            this.Id = id;
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
            var report = ravenSession.Include<Report>(r => r.DatabaseId)
                .Load<Report>(query.Id);

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