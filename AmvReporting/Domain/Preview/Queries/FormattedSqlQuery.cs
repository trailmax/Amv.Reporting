using System;
using AmvReporting.Infrastructure.CQRS;
using PoorMansTSqlFormatterLib.Formatters;
using PoorMansTSqlFormatterLib.Interfaces;
using PoorMansTSqlFormatterLib.Parsers;
using PoorMansTSqlFormatterLib.Tokenizers;

namespace AmvReporting.Domain.Preview.Queries
{
    public class FormattedSqlQuery : IQuery<string>
    {
        public string CleanedSql { get; set; }

        public FormattedSqlQuery(string cleanedSql)
        {
            CleanedSql = cleanedSql;
        }
    }

    public class FormattedSqlQueryHandler : IQueryHandler<FormattedSqlQuery, String>
    {
        public string Handle(FormattedSqlQuery query)
        {
            ISqlTokenizer tokenizer = new TSqlStandardTokenizer();
            ISqlTokenParser parser = new TSqlStandardParser();
            ISqlTreeFormatter formatter = new TSqlStandardFormatter()
            {
                BreakJoinOnSections = true,
                ExpandCommaLists = false,
            };


            var tokenizedSql = tokenizer.TokenizeSQL(query.CleanedSql);

            var parsedSql = parser.ParseSQL(tokenizedSql);

            var result = formatter.FormatSQLTree(parsedSql);

            return result;
        }
    }
}