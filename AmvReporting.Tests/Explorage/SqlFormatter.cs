using System;
using PoorMansTSqlFormatterLib.Formatters;
using PoorMansTSqlFormatterLib.Interfaces;
using PoorMansTSqlFormatterLib.Parsers;
using PoorMansTSqlFormatterLib.Tokenizers;
using Xunit;

namespace AmvReporting.Tests.Explorage
{
    public class SqlFormatterTests
    {
        [Fact]
        public void SqlFormatter_Works()
        {
            var sqlString = 
@"SELECT Area
	,COUNT(FULLNAME) AS Expr1
FROM (
	SELECT TOP (100) PERCENT dbo.TABLESEVEN.NAME AS Area
		,dbo.TABLEEIGHT.FULLNAME
	FROM dbo.TABLENINE
	INNER JOIN dbo.TABLEEIGHT ON dbo.TABLENINE.SERVICEUSERS_ID = dbo.TABLEEIGHT.YETANOTHER_ID
	INNER JOIN dbo.TABLETHREE ON dbo.TABLENINE.ASSOCIATEDSERVICE = dbo.TABLETHREE.ORGLEVEL3_ID
	INNER JOIN dbo.TABLESEVEN ON dbo.TABLETHREE.ORGLEVEL2 = dbo.TABLESEVEN.SOMETABLE_ID
	WHERE (dbo.TABLEEIGHT.CURRENTSTATUS = 'ACTIVE')
	GROUP BY dbo.TABLESEVEN.NAME
		,dbo.TABLEEIGHT.FULLNAME
	ORDER BY Area
	) AS derivedtbl_1
GROUP BY Area
";
            
            ISqlTokenizer tokenizer = new TSqlStandardTokenizer();
            ISqlTokenParser parser = new TSqlStandardParser();
            ISqlTreeFormatter formatter = new TSqlStandardFormatter()
                                          {
                                              BreakJoinOnSections = true,
                                              ExpandCommaLists = false,
                                          };

            var tokenizedSql = tokenizer.TokenizeSQL(sqlString);

            var parsedSql = parser.ParseSQL(tokenizedSql);

            var result = formatter.FormatSQLTree(parsedSql);

            Console.WriteLine(result);
        }
    }
}
