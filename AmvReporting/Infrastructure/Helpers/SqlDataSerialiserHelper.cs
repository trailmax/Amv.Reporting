using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace AmvReporting.Infrastructure.Helpers
{
    public static class SqlDataSerialiserHelper
    {
        public static String GetColumnsJson(SqlDataReader dataReader)
        {
            var cols = new List<ColumnsDefinition>();
            for (var i = 0; i < dataReader.FieldCount; i++)
            {
                var pair = new ColumnsDefinition()
                           {
                               mData = dataReader.GetName(i),
                               sTitle = dataReader.GetName(i),
                           };
                cols.Add(pair); ;
            }

            var jsonColumns = JsonConvert.SerializeObject(cols, Formatting.Indented);

            return jsonColumns;
        }

        private class ColumnsDefinition
        {
            // ReSharper disable InconsistentNaming
            public String mData { get; set; }
            public String sTitle { get; set; }
            // ReSharper restore InconsistentNaming
        }


        public static String GetDataJson(SqlDataReader reader)
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

            var dataJson = JsonConvert.SerializeObject(results, Formatting.Indented);
            return dataJson;
        }


        private static Dictionary<string, object> SerializeRow(IEnumerable<string> cols, SqlDataReader reader)
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