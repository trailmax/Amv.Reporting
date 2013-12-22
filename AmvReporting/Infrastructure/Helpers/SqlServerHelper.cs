using System;
using System.Data.SqlClient;
using AmvReporting.Domain;

namespace AmvReporting.Infrastructure.Helpers
{
    public class SqlServerHelper : IDisposable
    {
        private SqlConnection connection;

        public SqlDataReader ExecuteQuery(String sql, String connectionString)
        {
            try
            {
                connection = new SqlConnection(connectionString);

                connection.Open();

                var command = new SqlCommand(sql, connection);

                var reader = command.ExecuteReader();

                return reader;

            }
            catch (Exception exception)
            {
                connection.Close();
                throw new DomainException(String.Format("Unable to query database: {0}", exception.Message));
            }
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}