using System;
using System.Data.SqlClient;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Queries
{
    public class CheckDatabaseConnectivityQuery : IQuery<ConnectionCheckingData>
    {
        public string ConnectionString { get; set; }

        public CheckDatabaseConnectivityQuery(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }

    public class ConnectionCheckingData
    {
        public bool IsConnected { get; set; }
        public String ExceptionMessage { get; set; }
    }

    public class CheckDatabaseConnectivityQueryHandler : IQueryHandler<CheckDatabaseConnectivityQuery, ConnectionCheckingData>
    {
        public ConnectionCheckingData Handle(CheckDatabaseConnectivityQuery query)
        {
            var result = new ConnectionCheckingData();

            try
            {
                using (var connection = new SqlConnection(query.ConnectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("select 1", connection);
                    command.ExecuteReader();
                    result.IsConnected = true;
                }
            }
            catch (Exception exception)
            {
                result.IsConnected = false;
                result.ExceptionMessage = exception.Message;
            }
            return result;
        }
    }
}