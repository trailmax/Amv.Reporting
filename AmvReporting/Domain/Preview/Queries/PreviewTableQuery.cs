using System;
using AmvReporting.Domain.DatabaseConnections;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using Raven.Client;

namespace AmvReporting.Domain.Preview.Queries
{
    public class PreviewTableQuery : IQuery<PreviewTableResult>
    {
        public string Sql { get; set; }
        public String DatabaseId { get; set; }

        public PreviewTableQuery(String sql, string databaseId)
        {
            Sql = sql;
            DatabaseId = databaseId;
        }
    }

    public class PreviewTableResult
    {
        public bool IsSuccess { get; set; }
        public String ExceptionMessage { get; set; }
        public String Data { get; set; }
        public String Columns { get; set; }
    }


    public class PreviewDataQueryHandler : IQueryHandler<PreviewTableQuery, PreviewTableResult>
    {
        private readonly IDocumentSession ravenSession;

        public PreviewDataQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public PreviewTableResult Handle(PreviewTableQuery query)
        {
            var database = ravenSession.Load<DatabaseConnection>(query.DatabaseId);

            var result = new PreviewTableResult()
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