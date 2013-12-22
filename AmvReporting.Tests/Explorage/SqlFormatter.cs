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
	SELECT TOP (100) PERCENT dbo.ORGLEVEL2.NAME AS Area
		,dbo.SERVICEUSERS.FULLNAME
	FROM dbo.SUSERVICES
	INNER JOIN dbo.SERVICEUSERS ON dbo.SUSERVICES.SERVICEUSERS_ID = dbo.SERVICEUSERS.SERVICEUSERS_ID
	INNER JOIN dbo.ORGLEVEL3 ON dbo.SUSERVICES.ASSOCIATEDSERVICE = dbo.ORGLEVEL3.ORGLEVEL3_ID
	INNER JOIN dbo.ORGLEVEL2 ON dbo.ORGLEVEL3.ORGLEVEL2 = dbo.ORGLEVEL2.ORGLEVEL2_ID
	WHERE (dbo.SERVICEUSERS.CURRENTSTATUS = 'ACTIVE')
	GROUP BY dbo.ORGLEVEL2.NAME
		,dbo.SERVICEUSERS.FULLNAME
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
