using System.Collections.Generic;
using System.Linq;
using AmvReporting.Domain.Menus;
using AmvReporting.Domain.ReportGroups;
using AmvReporting.Domain.Reports;
using AmvReporting.Tests.ZeroFriction;
using Ploeh.AutoFixture;
using Raven.Client;
using Xunit;

namespace AmvReporting.Tests.Domain.Menus
{
    public class MenuModelQueryHandlerTests
    {
        private readonly MenuModel expected;
        private readonly MenuModel result;
        private readonly IFixture fixture;

        public MenuModelQueryHandlerTests()
        {
            fixture = new Fixture().Customize(new DomainCustomisation());

            // done
            var groupLessReports = fixture.Build<Report>()
                .Without(r => r.ReportGroupId).CreateMany().ToList();

            // done
            var topLevelGroups = fixture.Build<ReportGroup>()
                .Without(r => r.ParentReportGroupId)
                .CreateMany(2).ToList();

            // done
            var topLevelReports = fixture.Build<Report>()
                .With(r => r.ReportGroupId, topLevelGroups.First().Id)
                .CreateMany().ToList();

            var secondLevelGroups = fixture.Build<ReportGroup>()
                .With(g => g.ParentReportGroupId, topLevelGroups.First().Id)
                .CreateMany(2).ToList();

            var secondLevelReports = fixture.Build<Report>()
                .With(r => r.ReportGroupId, secondLevelGroups.First().Id)
                .CreateMany().ToList();


            expected = new MenuModel()
                       {
                           TopLevelReports = groupLessReports,
                           MenuNodes = new List<MenuNode>()
                                       {
                                           new MenuNode()
                                           {
                                               ReportGroupId = topLevelGroups.First().Id,
                                               ReportGroupTitle = topLevelGroups.First().Title,
                                               Reports = topLevelReports,
                                               MenuNodes = new List<MenuNode>()
                                                           {
                                                               new MenuNode()
                                                               {
                                                                   ReportGroupId = secondLevelGroups.First().Id,
                                                                   ReportGroupTitle = secondLevelGroups.First().Title,
                                                                   Reports = secondLevelReports,
                                                               },
                                                               new MenuNode()
                                                               {
                                                                   ReportGroupId = secondLevelGroups.Skip(1).First().Id,
                                                                   ReportGroupTitle = secondLevelGroups.Skip(1).First().Title,
                                                               }
                                                           }
                                           },
                                           new MenuNode()
                                           {
                                               ReportGroupId = topLevelGroups.Skip(1).First().Id,
                                               ReportGroupTitle = topLevelGroups.Skip(1).First().Title,
                                           }
                                       }
                       };

            SaveDataToRaven(topLevelGroups, secondLevelGroups, groupLessReports, topLevelReports, secondLevelReports);


            var sut = fixture.Create<MenuModelQueryHandler>();

            result = sut.Handle(new MenuModelQuery(true));
        }

        private void SaveDataToRaven(List<ReportGroup> topLevelGroups, List<ReportGroup> secondLevelGroups, List<Report> groupLessReports,
            List<Report> topLevelReports, List<Report> secondLevelReports)
        {
            var allGroups = new List<ReportGroup>();
            allGroups.AddRange(topLevelGroups);
            allGroups.AddRange(secondLevelGroups);

            var allReports = new List<Report>();
            allReports.AddRange(groupLessReports);
            allReports.AddRange(topLevelReports);
            allReports.AddRange(secondLevelReports);

            var ravenSession = fixture.Create<IDocumentSession>();
            allReports.ForEach(ravenSession.Store);
            allGroups.ForEach(ravenSession.Store);
            ravenSession.SaveChanges();
        }


        [Fact]
        public void Handle_GroupLessReports_AreInTopLevel()
        {
            AssertionHelpers.ListsAreEqual(expected.TopLevelReports.OrderBy(r => r.Id), result.TopLevelReports.OrderBy(r => r.Id));
        }

        [Fact]
        public void Handle_TopLevelGroups_AreInTopLevel()
        {
            var expectedGroupIds = expected.MenuNodes.SelectMany(r => r.ReportGroupId).OrderBy(r => r);
            var expectedGroupTitles = expected.MenuNodes.SelectMany(r => r.ReportGroupTitle).OrderBy(t => t);

            var resultingGroupId = result.MenuNodes.SelectMany(r => r.ReportGroupId).OrderBy(r => r);
            var resultingGroupTitles = result.MenuNodes.SelectMany(r => r.ReportGroupTitle).OrderBy(t => t);

            Assert.Equal(expectedGroupIds, resultingGroupId);
            Assert.Equal(expectedGroupTitles, resultingGroupTitles);
        }

        [Fact]
        public void Handle_FirstLevelReports_Match()
        {
            //Arrange
            var expectedSecondLevelReports = expected.MenuNodes.First().Reports.OrderBy(r => r.Id).ToList();
            var resultingSecondLevelreports = result.MenuNodes.First().Reports.OrderBy(r => r.Id).ToList();

            // Assert
            AssertionHelpers.ListsAreEqual(expectedSecondLevelReports, resultingSecondLevelreports);
        }

        [Fact]
        public void Handle_SecondLevelGroups_Match()
        {
            //Arrange
            var expectedSecondLevelIds = expected.MenuNodes.First().MenuNodes.Select(r => r.ReportGroupId).OrderBy(r => r);
            var expectedSecondLevelTitles = expected.MenuNodes.First().MenuNodes.Select(r => r.ReportGroupTitle).OrderBy(r => r);

            var resultingSecondLevelIds = result.MenuNodes.First().MenuNodes.Select(r => r.ReportGroupId).OrderBy(r => r);
            var resultingSecondLevelTitles = result.MenuNodes.First().MenuNodes.Select(r => r.ReportGroupTitle).OrderBy(r => r);


            // Assert
            Assert.Equal(expectedSecondLevelIds, resultingSecondLevelIds);
            Assert.Equal(expectedSecondLevelTitles, resultingSecondLevelTitles);
        }


        [Fact]
        public void Handle_SecondLevelReports_Match()
        {
            //Arrange
            var expectedSecondLevel = expected.MenuNodes.First().MenuNodes.First().Reports.OrderBy(r => r.Id);
            var resultingSecondLevel = result.MenuNodes.First().MenuNodes.First().Reports.OrderBy(r => r.Id);

            // Assert
            AssertionHelpers.ListsAreEqual(expectedSecondLevel, resultingSecondLevel);
        }
    }
}
