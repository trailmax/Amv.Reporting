using System;
using System.Data.SqlClient;
using AmvReporting.Domain.DatabaseConnections;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using Raven.Client;

namespace AmvReporting.Domain.Preview
{
    public class PreviewDataQuery : IQuery<SqlPreviewResult>
    {
        public string Sql { get; set; }
        public String DatabaseId { get; set; }

        public PreviewDataQuery(String sql, string databaseId)
        {
            Sql = sql;
            DatabaseId = databaseId;
        }
    }

    public class SqlPreviewResult
    {
        public bool IsSuccess { get; set; }
        public String ExceptionMessage { get; set; }
        public String Data { get; set; }
        public String Columns { get; set; }
    }


    public class PreviewDataQueryHandler : IQueryHandler<PreviewDataQuery, SqlPreviewResult>
    {
        private readonly IDocumentSession ravenSession;

        public PreviewDataQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public SqlPreviewResult Handle(PreviewDataQuery query)
        {
            var database = ravenSession.Load<DatabaseConnection>(query.DatabaseId);

            var result = new SqlPreviewResult()
                         {
                             IsSuccess = false,
                         };
            try
            {
                using (var sqlHelper = new SqlServerHelper())
                {
                    var dataReader = sqlHelper.ExecuteQuery(query.Sql, database.ConnectionString);
                    result.IsSuccess = true;
                    result.Data = SqlDataSerialiserHelper.GetDataJson(dataReader);
                    result.Columns = SqlDataSerialiserHelper.GetColumnsJson(dataReader);
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