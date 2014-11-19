﻿using System.Collections.Generic;
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
        private readonly IDocumentSession ravenSession;

        public MenuModelQueryHandlerTests()
        {
            var fixture = new Fixture().Customize(new DomainCustomisation());
            ravenSession = fixture.Create<IDocumentSession>();

            // done
            var groupLessReports = fixture.Build<ReportViewModel>()
                .Without(r => r.ReportGroupId)
                .CreateMany().ToList();

            // done
            var topLevelGroups = fixture.Build<ReportGroup>()
                .Without(r => r.ParentReportGroupId)
                .CreateMany(2).ToList();

            // done
            var topLevelReports = fixture.Build<ReportViewModel>()
                .With(r => r.ReportGroupId, topLevelGroups.First().Id)
                .CreateMany().ToList();

            var secondLevelGroups = fixture.Build<ReportGroup>()
                .With(g => g.ParentReportGroupId, topLevelGroups.First().Id)
                .CreateMany(2).ToList();

            var secondLevelReports = fixture.Build<ReportViewModel>()
                .With(r => r.ReportGroupId, secondLevelGroups.First().Id)
                .CreateMany().ToList();


            expected = new MenuModel()
                       {
                           TopLevelReports = groupLessReports,
                           MenuNodes = new List<MenuNode>()
                                       {
                                           new MenuNode()
                                           {
                                               ReportGroupId = topLevelGroups.OrderBy(r => r.ListOrder).First().Id,
                                               ReportGroupTitle = topLevelGroups.OrderBy(r => r.ListOrder).First().Title,
                                               Reports = topLevelReports,
                                               MenuNodes = new List<MenuNode>()
                                                           {
                                                               new MenuNode()
                                                               {
                                                                   ReportGroupId = secondLevelGroups.OrderBy(r => r.ListOrder).First().Id,
                                                                   ReportGroupTitle = secondLevelGroups.OrderBy(r => r.ListOrder).First().Title,
                                                                   Reports = secondLevelReports,
                                                               },
                                                               new MenuNode()
                                                               {
                                                                   ReportGroupId = secondLevelGroups.OrderBy(r => r.ListOrder).Skip(1).First().Id,
                                                                   ReportGroupTitle = secondLevelGroups.OrderBy(r => r.ListOrder).Skip(1).First().Title,
                                                               }
                                                           }
                                           },
                                           new MenuNode()
                                           {
                                               ReportGroupId = topLevelGroups.OrderBy(r => r.ListOrder).Skip(1).First().Id,
                                               ReportGroupTitle = topLevelGroups.OrderBy(r => r.ListOrder).Skip(1).First().Title,
                                           }
                                       }
                       };

            SaveDataToRaven(topLevelGroups, secondLevelGroups, groupLessReports, topLevelReports, secondLevelReports);


            var sut = fixture.Create<MenuModelQueryHandler>();

            result = sut.Handle(new MenuModelQuery(true));
        }

        private void SaveDataToRaven(List<ReportGroup> topLevelGroups, List<ReportGroup> secondLevelGroups, List<ReportViewModel> groupLessReports,
            List<ReportViewModel> topLevelReports, List<ReportViewModel> secondLevelReports)
        {
            var allGroups = new List<ReportGroup>();
            allGroups.AddRange(topLevelGroups);
            allGroups.AddRange(secondLevelGroups);

            var allReports = new List<ReportViewModel>();
            allReports.AddRange(groupLessReports);
            allReports.AddRange(topLevelReports);
            allReports.AddRange(secondLevelReports);

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
            var expectedGroupIds = expected.MenuNodes.Select(r => r.ReportGroupId).OrderBy(r => r).ToList();
            var expectedGroupTitles = expected.MenuNodes.Select(r => r.ReportGroupTitle).OrderBy(t => t).ToList();

            var resultingGroupId = result.MenuNodes.Select(r => r.ReportGroupId).OrderBy(r => r).ToList();
            var resultingGroupTitles = result.MenuNodes.Select(r => r.ReportGroupTitle).OrderBy(t => t).ToList();

            Assert.Equal(expectedGroupIds, resultingGroupId);
            Assert.Equal(expectedGroupTitles, resultingGroupTitles);
        }

        [Fact(Skip = "random failings")]
        public void Handle_FirstLevelReports_Match()
        {
            //Arrange
            var expectedSecondLevelReports = expected.MenuNodes.First().Reports.OrderBy(r => r.Id).ToList();
            var resultingSecondLevelReports = result.MenuNodes.First().Reports.OrderBy(r => r.Id).ToList();

            // Assert
            AssertionHelpers.ListsAreEqual(expectedSecondLevelReports, resultingSecondLevelReports);
        }

        [Fact(Skip = "random failings")]
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


        [Fact(Skip = "random failings")]
        public void Handle_SecondLevelReports_Match()
        {
            //Arrange
            var expectedSecondLevel = expected.MenuNodes.First().MenuNodes.First().Reports.OrderBy(r => r.Id);
            var resultingSecondLevel = result.MenuNodes.First().MenuNodes.First().Reports.OrderBy(r => r.Id);

            // Assert
            AssertionHelpers.ListsAreEqual(expectedSecondLevel, resultingSecondLevel);
        }

        //public void Dispose()
        //{
        //    var allReports = ravenSession.Query<Report>().ToList();
        //    allReports.ForEach(ravenSession.Delete);

        //    var allGroups = ravenSession.Query<ReportGroup>().ToList();
        //    allGroups.ForEach(ravenSession.Delete);

        //    ravenSession.SaveChanges();

        //    ravenSession.Dispose();
        //}
    }
}
