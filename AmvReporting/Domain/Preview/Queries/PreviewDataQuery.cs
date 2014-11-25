using System;
using System.Linq;
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
        public Guid AggregateId { get; set; }

        public PreviewDataQuery(Guid aggregateId, string sql)
        {
            AggregateId = aggregateId;
            Sql = sql;
        }
    }

    public class PreviewDataResult
    {
        public bool IsSuccess { get; set; }
        public String ExceptionMessage { get; set; }
        public String Data { get; set; }
        public ReportType ReportType { get; set; }
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
            var report = ravenSession.Query<ReportViewModel>().Include<ReportViewModel>(r => r.DatabaseId)
                .FirstOrDefault(r => r.AggregateId == query.AggregateId);

            if (report == null)
            {
                throw new DomainException("Unable to find report with this id");
            }

            var database = ravenSession.Load<DatabaseConnection>(report.DatabaseId);

            var result = new PreviewDataResult()
            {
                ReportType = report.ReportType,
            };

            try
            {
                using (var sqlHelper = new SqlServerHelper())
                {
                    var dataReader = sqlHelper.ExecuteQuery(query.Sql, database.ConnectionString);
                    result.IsSuccess = true;
                    result.Data = SqlDataSerialiserHelper.GetDataWithColumnsJson(dataReader);
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