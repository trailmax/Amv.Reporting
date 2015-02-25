using System;
using System.Data.SqlClient;
using System.Linq;
using AmvReporting.Domain.DatabaseConnections;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;


namespace AmvReporting.Domain.SchemaValidation
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public String Error { get; set; }
    }

    public class SqlValidationQuery : IQuery<ValidationResult>
    {
        public Guid AggregateId { get; set; }
    }

    public class SqlValidationQueryHandler : IQueryHandler<SqlValidationQuery, ValidationResult>
    {
        private readonly IDocumentSession ravenSession;

        public SqlValidationQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public ValidationResult Handle(SqlValidationQuery query)
        {
            var report = ravenSession.Query<ReportViewModel>()
                .Include<ReportViewModel>(r => r.DatabaseId)
                .FirstOrDefault(r => r.AggregateId == query.AggregateId);

            if (report == null)
            {
                throw new DomainException("Unable to find report with this id");
            }

            var database = ravenSession.Load<DatabaseConnection>(report.DatabaseId);

            var sql = String.Format("set rowcount 1;{0}{1}", Environment.NewLine, report.Sql);
            var connectionString = database.ConnectionString;

            var result = new ValidationResult()
                            {
                                IsValid = true,
                                Error = String.Empty,
                            };

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand(sql, connection);

                    command.ExecuteReader();
                }
            }
            catch (Exception exception)
            {
                result.IsValid = false;
                result.Error = exception.Message;
            }
            return result;
        }
    }
}