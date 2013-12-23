using System;
using System.Text.RegularExpressions;
using System.Web;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;

namespace AmvReporting.Domain.Preview.Queries
{
    public class RawHtmlInputQuery : IQuery<QueryResult>
    {
        public string RawData { get; set; }

        public RawHtmlInputQuery(String rawData)
        {
            RawData = rawData;
        }
    }

    public class RawHtmlInputQueryHandler : IQueryHandler<RawHtmlInputQuery, QueryResult>
    {
        public QueryResult Handle(RawHtmlInputQuery query)
        {
            var decodedHtml = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(query.RawData));

            var pattern = new Regex("<[cx]:CommandText>(.*)</[cx]:CommandText>");

            var match = pattern.Match(decodedHtml);

            var sql = match.Groups[1].CheckForNull(g => g.Value);

            if (String.IsNullOrEmpty(sql))
            {
                var result = new QueryResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Provided text does not look like a accepted HTML format: no 'CommandText' tag in xml",
                };
                return result;
            }

            var successResult = new QueryResult()
                                {
                                    IsSuccess = true,
                                    Payload = Regex.Replace(sql, @"\s+", " "),
                                };

            return successResult;
        }
    }
}