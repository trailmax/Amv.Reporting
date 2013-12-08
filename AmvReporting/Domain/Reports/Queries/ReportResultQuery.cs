﻿using System;
using System.Linq;
using AmvReporting.Domain.DatabaseConnections;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using Raven.Client;

namespace AmvReporting.Domain.Reports.Queries
{
    public class ReportResult
    {
        public String Title { get; set; }
        public String Description { get; set; }
        public String Data { get; set; }
        public String Columns { get; set; }

        public String JavaScript { get; set; }
        public String Css { get; set; }
        public ReportType ReportType { get; set; }
    }


    public class ReportResultQuery : IQuery<ReportResult>
    {
        public String LinkName { get; set; }

        public ReportResultQuery(String linkName)
        {
            this.LinkName = linkName;
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
            var report = ravenSession.Query<Report>()
                .Customize(x => x.Include<Report>(r => r.DatabaseId)) // load Database 
                .FirstOrDefault(r => r.LinkName == query.LinkName);

            if (report == null)
            {
                throw new DomainException("Unable to find report on this path");
            }

            var dbConnection = ravenSession.Load<DatabaseConnection>(report.DatabaseId);

            var result = new ReportResult()
                         {
                             Css = report.Css,
                             Description = report.Description,
                             JavaScript = report.JavaScript,
                             Title = report.Title,
                             ReportType = report.ReportType,
                         };


            using (var sqlServerHelper = new SqlServerHelper())
            {
                var dataReader = sqlServerHelper.ExecuteQuery(report.Sql, dbConnection.ConnectionString);
                result.Data = SqlDataSerialiserHelper.GetDataJson(dataReader);
                result.Columns = SqlDataSerialiserHelper.GetColumnsJson(dataReader);
            }

            return result;
        }
    }
}