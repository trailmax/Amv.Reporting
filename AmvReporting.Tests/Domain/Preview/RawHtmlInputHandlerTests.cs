using System;
using System.IO;
using System.Text.RegularExpressions;
using AmvReporting.Domain.Preview.Queries;
using Xunit;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.Preview
{
    public class RawHtmlInputHandlerTests
    {
        [Theory,
        InlineData("Reports/eight.htm", "SELECT dbo.__TABLEONE.Reason, dbo.__TABLEONE.LostTime * dbo.__TABLETWO.FieldSeventySeven AS Cost, ISNULL(dbo.TABLETHREE.SERVICEGROUP, dbo.TABLETHREE.NAME) AS Service FROM dbo.__TABLEONE INNER JOIN dbo.__TABLETWO ON dbo.__TABLEONE.YearMonth = dbo.__TABLETWO.[MONTH] INNER JOIN dbo.TABLETHREE ON dbo.__TABLEONE.Service = dbo.TABLETHREE.NAME WHERE (dbo.__TABLEONE.YearMonth = dbo.__SomeSqlFunction(GETDATE())) AND (dbo.__TABLEONE.Area = 'Some String') GROUP BY dbo.__TABLEONE.Reason, dbo.__TABLEONE.LostTime * dbo.__TABLETWO.FieldSeventySeven, ISNULL(dbo.TABLETHREE.SERVICEGROUP, dbo.TABLETHREE.NAME)"),
        InlineData("Reports/nine.htm", "SELECT TOP 100 PERCENT dbo.TABLEFIVE.REASON, ISNULL(dbo.TABLETHREE.SERVICEGROUP, dbo.TABLETHREE.NAME) AS Service, SUM(dbo.TABLEFIVE.HOURSLOST * dbo.__TABLETWO.FieldSeventySeven) AS Cost FROM dbo.TABLETHREE INNER JOIN dbo.TABLEFIVE ON dbo.TABLETHREE.ORGLEVEL3_ID = dbo.TABLEFIVE.SERVICEOFFICE INNER JOIN dbo.TABLESEVEN ON dbo.TABLEFIVE.AREA = dbo.TABLESEVEN.SOMETABLE_ID INNER JOIN dbo.__TABLETWO ON dbo.TABLEFIVE.YEARMONTH = dbo.__TABLETWO.[MONTH] WHERE (dbo.TABLEFIVE.YEARMONTH = dbo.__SomeSqlFunction(GETDATE())) AND (dbo.TABLEFIVE.CATEGORY = N'SICK') AND (dbo.TABLESEVEN.NAME = N'Some String') GROUP BY dbo.TABLEFIVE.REASON, ISNULL(dbo.TABLETHREE.SERVICEGROUP, dbo.TABLETHREE.NAME) ORDER BY SUM(dbo.TABLEFIVE.HOURSLOST * dbo.__TABLETWO.FieldSeventySeven) DESC"),
        InlineData("Reports/one.htm", "select * from _CCC_LD_WhosBookedOnTraining_IntranetReport"),
        InlineData("Reports/seven.htm", "SELECT Area, COUNT(FULLNAME) AS Expr1 FROM (SELECT TOP (100) PERCENT dbo.TABLESEVEN.NAME AS Area, dbo.TABLEEIGHT.FULLNAME FROM dbo.TABLENINE INNER JOIN dbo.TABLEEIGHT ON dbo.TABLENINE.SERVICEUSERS_ID = dbo.TABLEEIGHT.YETANOTHER_ID INNER JOIN dbo.TABLETHREE ON dbo.TABLENINE.ASSOCIATEDSERVICE = dbo.TABLETHREE.ORGLEVEL3_ID INNER JOIN dbo.TABLESEVEN ON dbo.TABLETHREE.ORGLEVEL2 = dbo.TABLESEVEN.SOMETABLE_ID WHERE (dbo.TABLEEIGHT.CURRENTSTATUS = 'ACTIVE') GROUP BY dbo.TABLESEVEN.NAME, dbo.TABLEEIGHT.FULLNAME ORDER BY Area) AS derivedtbl_1 GROUP BY Area"),
        InlineData("Reports/ten.htm", "SELECT TOP 100 PERCENT dbo.TABLEFIVE.REASON, SUM(dbo.TABLEFIVE.HOURSLOST * dbo.__TABLETWO.FieldSeventySeven) AS Cost FROM dbo.TABLEFIVE INNER JOIN dbo.TABLESEVEN ON dbo.TABLEFIVE.AREA = dbo.TABLESEVEN.SOMETABLE_ID INNER JOIN dbo.__TABLETWO ON dbo.TABLEFIVE.YEARMONTH = dbo.__TABLETWO.[MONTH] WHERE (dbo.TABLEFIVE.CATEGORY = N'sick') AND (dbo.TABLEFIVE.YEARMONTH = dbo.__SomeSqlFunction(GETDATE())) AND (dbo.TABLESEVEN.NAME = N'Some String') GROUP BY dbo.TABLEFIVE.REASON, dbo.TABLESEVEN.NAME ORDER BY SUM(dbo.TABLEFIVE.HOURSLOST * dbo.__TABLETWO.FieldSeventySeven) DESC "),
        ]
        public void Handle_GivenFile_ReturnsSqlRequest(String filename, String expected)
        {
            //Arrange
            var fileString = File.ReadAllText(filename);

            var query = new RawHtmlInputQuery(fileString);
            var sut = new RawHtmlInputQueryHandler();
            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.True(result.IsSuccess);

            var e = Regex.Replace(expected, @"\s", "").ToLower();
            var r = Regex.Replace(result.Payload, @"\s", "").ToLower();
            Assert.Equal(e, r);
        }
    }
}
