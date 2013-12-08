using System;
using AmvReporting.Domain.DatabaseConnections;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using Raven.Client;

namespace AmvReporting.Domain.Preview.Queries
{
    public class PreviewLineQuery : IQuery<PreviewLineResult>
    {
        public string Sql { get; set; }
        public string DatabaseId { get; set; }

        public PreviewLineQuery(String sql, String databaseId)
        {
            Sql = sql;
            DatabaseId = databaseId;
        }
    }

    public class PreviewLineResult
    {
        public bool IsSuccess { get; set; }
        public String ExceptionMessage { get; set; }
    }


    public class PreviewLineQueryHandler : IQueryHandler<PreviewLineQuery, PreviewLineResult>
    {
        private readonly IDocumentSession ravenSession;

        public PreviewLineQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public PreviewLineResult Handle(PreviewLineQuery query)
        {
            var database = ravenSession.Load<DatabaseConnection>(query.DatabaseId);

            var result = new PreviewLineResult()
                         {
                             IsSuccess = false,
                         };

            try
            {
                using (var sqlHelper = new SqlServerHelper())
                {
                    var dataReader = sqlHelper.ExecuteQuery(query.Sql, database.ConnectionString);
                    result.IsSuccess = true;
                    //result.Data = SqlDataSerialiserHelper.GetDataJson(dataReader);
                    //result.Columns = SqlDataSerialiserHelper.GetColumnsJson(dataReader);
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