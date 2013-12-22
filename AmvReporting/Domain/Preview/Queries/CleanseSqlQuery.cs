using System;
using System.Text.RegularExpressions;
using System.Web;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Domain.Preview.Queries
{
    public class CleanseSqlQuery : IQuery<String>
    {
        public string Sql { get; set; }

        public CleanseSqlQuery(string sql)
        {
            Sql = sql;
        }
    }

    public class CleanseSqlQueryHandler : IQueryHandler<CleanseSqlQuery, String>
    {
        public String Handle(CleanseSqlQuery query)
        {
            var firstPass = HttpUtility.HtmlDecode(query.Sql);
            var secondPass = HttpUtility.HtmlDecode(firstPass);

            var thirdPass = Regex.Replace(secondPass, @"\s+", " ");

            return thirdPass;
        }
    }
}