using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;

namespace AmvReporting.Infrastructure.Helpers
{
    public static class SqlDataSerialiserHelper
    {
        public static String GetDataWithColumnsJson(SqlDataReader dataReader)
        {
            var dataBuilder = new StringBuilder();
            dataBuilder.AppendFormat("var columns = {0};", GetColumnsJson(dataReader));
            dataBuilder.AppendLine();
            dataBuilder.AppendLine();
            dataBuilder.AppendFormat("var data = {0};", GetDataJson(dataReader));
            var result = dataBuilder.ToString();

            return result;
        }

        private static String GetColumnsJson(SqlDataReader dataReader)
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


        private static String GetDataJson(SqlDataReader reader)
        {
            var results = new List<Dictionary<String, String>>();
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


        private static Dictionary<String, String> SerializeRow(IEnumerable<String> cols, SqlDataReader reader)
        {
            var result = new Dictionary<String, String>();

            foreach (var col in cols)
            {
                result.Add(col, reader[col].ToString().Trim());
            }

            return result;
        }
    }
}