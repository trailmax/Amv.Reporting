using System;
using AmvReporting.Domain.Preview.Queries;
using Xunit;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.Preview
{
    public class CleanseSqlQueryHandlerTests
    {
        [Theory, 
        InlineData("select * from _CCC_CurrentSUCountByArea", "select * from _CCC_CurrentSUCountByArea"),
        InlineData("SELECT     TOP 100 PERCENT &amp;#13;&amp;#9;dbo.PEOPLE.REFERENCENUMBER AS EmplNo, &amp;#13;&amp;#9;dbo.PEOPLE.LASTNAME AS [Last Name], &amp;#13;&amp;#9;dbo.PEOPLE.FIRSTNAME AS [First Name],&amp;#13;&amp;#9;dbo.ORGLEVEL2.NAME AS [Area/Office], &amp;#13;&amp;#9;dbo.ORGLEVEL3.NAME AS [Service/Team], &amp;#13;&amp;#9;ISNULL(CONVERT(CHAR(20), dbo.COSTCENTRES.COSTCENTRE), 'Not Yet Assigned') AS [Cost Centre]&amp;#13;FROM         dbo.PEOPLE INNER JOIN&amp;#13;                      dbo.JOBDETAIL ON dbo.PEOPLE.PEOPLE_ID = dbo.JOBDETAIL.PEOPLE_ID INNER JOIN&amp;#13;                      dbo.ORGLEVEL3 ON dbo.JOBDETAIL.ORGLEVEL3 = dbo.ORGLEVEL3.ORGLEVEL3_ID LEFT OUTER JOIN&amp;#13;                      dbo.COSTCENTRES ON dbo.ORGLEVEL3.COSTCENTRE = dbo.COSTCENTRES.COSTCENTRES_ID INNER JOIN&amp;#13;&amp;#9;&amp;#9;&amp;#9;&amp;#9;&amp;#9;  dbo.ORGLEVEL2 on dbo.JOBDETAIL.ORGLEVEL2 = dbo.ORGLEVEL2.ORGLEVEL2_ID&amp;#13;WHERE     (dbo.JOBDETAIL.EFFECTIVEDATE &amp;lt;= dbo.__CCC_LDOM(GETDATE())) AND (dbo.JOBDETAIL.ENDDATE &amp;gt;= dbo.__CCC_FDOM(GETDATE()) OR&amp;#13;                      dbo.JOBDETAIL.ENDDATE IS NULL) AND (dbo.JOBDETAIL.CURRENTRECORD = N'YES')&amp;#13;ORDER BY dbo.PEOPLE.LASTNAME, dbo.PEOPLE.FIRSTNAME",
            "SELECT TOP 100 PERCENT dbo.PEOPLE.REFERENCENUMBER AS EmplNo, dbo.PEOPLE.LASTNAME AS [Last Name], dbo.PEOPLE.FIRSTNAME AS [First Name], dbo.ORGLEVEL2.NAME AS [Area/Office], dbo.ORGLEVEL3.NAME AS [Service/Team], ISNULL(CONVERT(CHAR(20), dbo.COSTCENTRES.COSTCENTRE), 'Not Yet Assigned') AS [Cost Centre] FROM dbo.PEOPLE INNER JOIN dbo.JOBDETAIL ON dbo.PEOPLE.PEOPLE_ID = dbo.JOBDETAIL.PEOPLE_ID INNER JOIN dbo.ORGLEVEL3 ON dbo.JOBDETAIL.ORGLEVEL3 = dbo.ORGLEVEL3.ORGLEVEL3_ID LEFT OUTER JOIN dbo.COSTCENTRES ON dbo.ORGLEVEL3.COSTCENTRE = dbo.COSTCENTRES.COSTCENTRES_ID INNER JOIN dbo.ORGLEVEL2 on dbo.JOBDETAIL.ORGLEVEL2 = dbo.ORGLEVEL2.ORGLEVEL2_ID WHERE (dbo.JOBDETAIL.EFFECTIVEDATE <= dbo.__CCC_LDOM(GETDATE())) AND (dbo.JOBDETAIL.ENDDATE >= dbo.__CCC_FDOM(GETDATE()) OR dbo.JOBDETAIL.ENDDATE IS NULL) AND (dbo.JOBDETAIL.CURRENTRECORD = N'YES') ORDER BY dbo.PEOPLE.LASTNAME, dbo.PEOPLE.FIRSTNAME"),
        InlineData("SELECT     Area, COUNT(FULLNAME) AS Expr1&amp;#13;FROM         (SELECT     TOP (100) PERCENT dbo.ORGLEVEL2.NAME AS Area, dbo.SERVICEUSERS.FULLNAME&amp;#13;                       FROM          dbo.SUSERVICES INNER JOIN&amp;#13;                                              dbo.SERVICEUSERS ON dbo.SUSERVICES.SERVICEUSERS_ID = dbo.SERVICEUSERS.SERVICEUSERS_ID INNER JOIN&amp;#13;                                              dbo.ORGLEVEL3 ON dbo.SUSERVICES.ASSOCIATEDSERVICE = dbo.ORGLEVEL3.ORGLEVEL3_ID INNER JOIN&amp;#13;                                              dbo.ORGLEVEL2 ON dbo.ORGLEVEL3.ORGLEVEL2 = dbo.ORGLEVEL2.ORGLEVEL2_ID&amp;#13;                       WHERE      (dbo.SERVICEUSERS.CURRENTSTATUS = 'ACTIVE')&amp;#13;                       GROUP BY dbo.ORGLEVEL2.NAME, dbo.SERVICEUSERS.FULLNAME&amp;#13;                       ORDER BY Area) AS derivedtbl_1&amp;#13;GROUP BY Area", 
            "SELECT Area, COUNT(FULLNAME) AS Expr1 FROM (SELECT TOP (100) PERCENT dbo.ORGLEVEL2.NAME AS Area, dbo.SERVICEUSERS.FULLNAME FROM dbo.SUSERVICES INNER JOIN dbo.SERVICEUSERS ON dbo.SUSERVICES.SERVICEUSERS_ID = dbo.SERVICEUSERS.SERVICEUSERS_ID INNER JOIN dbo.ORGLEVEL3 ON dbo.SUSERVICES.ASSOCIATEDSERVICE = dbo.ORGLEVEL3.ORGLEVEL3_ID INNER JOIN dbo.ORGLEVEL2 ON dbo.ORGLEVEL3.ORGLEVEL2 = dbo.ORGLEVEL2.ORGLEVEL2_ID WHERE (dbo.SERVICEUSERS.CURRENTSTATUS = 'ACTIVE') GROUP BY dbo.ORGLEVEL2.NAME, dbo.SERVICEUSERS.FULLNAME ORDER BY Area) AS derivedtbl_1 GROUP BY Area")]
        public void Handle_ConvertsHtml_IntoPlainSql(String dirtySql, String expected)
        {
            //Arrange
            var sut = new CleanseSqlQueryHandler();

            var query = new CleanseSqlQuery(dirtySql);

            // Act
            var result = sut.Handle(query);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
