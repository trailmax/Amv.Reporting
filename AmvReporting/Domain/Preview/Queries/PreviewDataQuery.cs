using System;
using System.Text;
using AmvReporting.Domain.DatabaseConnections;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using Raven.Client;

namespace AmvReporting.Domain.Preview.Queries
{
    public class PreviewDataQuery : IQuery<PreviewDataResult>
    {
        public string Sql { get; set; }
        public String DatabaseId { get; set; }
        public ReportType ReportType { get; set; }

        public PreviewDataQuery(string sql, string databaseId, ReportType reportType)
        {
            Sql = sql;
            DatabaseId = databaseId;
            ReportType = reportType;
        }
    }

    public class PreviewDataResult
    {
        public bool IsSuccess { get; set; }
        public String ExceptionMessage { get; set; }
        public String Data { get; set; }
    }


    public class PreviewDataQueryHandler : IQueryHandler<PreviewDataQuery, PreviewDataResult>
    {
        private readonly IDocumentSession ravenSession;

        public PreviewDataQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public PreviewDataResult Handle(PreviewDataQuery query)
        {
            var database = ravenSession.Load<DatabaseConnection>(query.DatabaseId);

            var result = new PreviewDataResult()
                         {
                             IsSuccess = false,
                         };
            try
            {
                using (var sqlHelper = new SqlServerHelper())
                {
                    var dataReader = sqlHelper.ExecuteQuery(query.Sql, database.ConnectionString);
                    result.IsSuccess = true;

                    var dataBuilder = new StringBuilder();
                    dataBuilder.AppendFormat("var columns = {0};", SqlDataSerialiserHelper.GetColumnsJson(dataReader));
                    dataBuilder.AppendLine();
                    dataBuilder.AppendLine();
                    dataBuilder.AppendFormat("var data = {0};",SqlDataSerialiserHelper.GetDataJson(dataReader));
                    result.Data = dataBuilder.ToString();
                }
            }
            catch (Exception exception)
            {
                result.IsSuccess = false;
                result.ExceptionMessage = exception.Message;
            }

            return result;
        }
    }
}