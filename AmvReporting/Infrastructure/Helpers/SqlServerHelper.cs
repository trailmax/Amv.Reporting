using System;
using System.Data.SqlClient;
using AmvReporting.Domain;

namespace AmvReporting.Infrastructure.Helpers
{
    public static class SqlServerHelper
    {
        public static SqlDataReader ExecuteSqlQuery(String sql, String connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand(sql, connection);

                    var reader = command.ExecuteReader();

                    return reader;
                }
            }
            catch (Exception exception)
            {
                throw new DomainException("Unable to query database", exception.Message);
            }
        }
    }
}