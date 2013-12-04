using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AmvReporting.Infrastructure.Configuration;
using AmvReporting.Infrastructure.CQRS;
using Newtonsoft.Json;
using Raven.Client;

namespace AmvReporting.Domain.ReportDetails.Queries
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
            var report = ravenSession.Query<Report>().FirstOrDefault(r => r.LinkName == query.LinkName);

            var result = new ReportResult()
                         {
                             Css = report.Css,
                             Description = report.Description,
                             JavaScript = report.JavaScript,
                             Title = report.Title,
                             ReportType = report.ReportType,
                         };

            var connectionString = ConfigurationContext.Current.GetDatabaseConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    var command = new SqlCommand(report.Sql, connection);

                    var reader = command.ExecuteReader();

                    var dataDictionary = GetDataDictionary(reader);
                    result.Data = JsonConvert.SerializeObject(dataDictionary, Formatting.Indented);

                    var columnsDictionary = GetColumnsDictionary(reader);
                    result.Columns = JsonConvert.SerializeObject(columnsDictionary, Formatting.Indented);
                }
                catch (SqlException exception)
                {
                    Console.WriteLine("Unable to query database. See exception");
                    Console.WriteLine(exception);
                }
            }

            return result;
        }


        public class ColumnsDefinition
        {
            // ReSharper disable InconsistentNaming
            public String mData { get; set; }
            // ReSharper restore InconsistentNaming
        }

        public List<ColumnsDefinition> GetColumnsDictionary(SqlDataReader dataReader)
        {
            var cols = new List<ColumnsDefinition>();
            for (var i = 0; i < dataReader.FieldCount; i++)
            {
                var pair = new ColumnsDefinition()
                           {
                               mData = dataReader.GetName(i),
                           };
                cols.Add(pair); ;
            }
            return cols;
        }

        public IEnumerable<Dictionary<string, object>> GetDataDictionary(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                cols.Add(reader.GetName(i));
            }

            while (reader.Read())
            {
                results.Add(SerializeRow(cols, reader));
            }

            return results;
        }

        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols, SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();

            foreach (var col in cols)
            {
                result.Add(col, reader[col]);
            }

            return result;
        }
    }
}