using System.Linq;
using AmvReporting.Domain.ReportGroups;
using AmvReporting.Domain.ReportGroups.Queries;
using AmvReporting.Domain.Reports;
using AmvReporting.Tests.ZeroFriction;
using Ploeh.AutoFixture;
using Raven.Client;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.ReportGroups.Queries
{
    public class GroupReorderQueryHandlerTests
    {
        private ReportGroup topGroup;
        private ReportGroup subGroup;
        private Report subReport;
        private Report topReport;

        private void PopulateData(IDocumentSession ravenSession)
        {
            var fixture = new Fixture();

            topReport = fixture.Build<Report>().Without(r => r.ReportGroupId).Create();
            topGroup = fixture.Build<ReportGroup>().Without(g => g.ParentReportGroupId).Create();

            subGroup = fixture.Build<ReportGroup>().With(g => g.ParentReportGroupId, topGroup.Id).Create();

            subReport = fixture.Build<Report>().With(r => r.ReportGroupId, topGroup.Id).Create();

            ravenSession.Store(topReport);
            ravenSession.Store(topGroup);
            ravenSession.Store(subGroup);
            ravenSession.Store(subReport);
            ravenSession.SaveChanges();
        }


        [Theory, AutoDomainData]
        public void Handle_IdProvided_ListsReturned(IDocumentSession ravenSession, GroupReorderQueryHandler sut)
        {
            //Arrange
            PopulateData(ravenSession);

            // Act
            var result = sut.Handle(new GroupReorderQuery(topGroup.Id));

            // Assert
            AssertionHelpers.PropertiesAreEqual(subReport, result.Reports.Single());
            AssertionHelpers.PropertiesAreEqual(subGroup, result.Groups.Single());
        }

        [Theory, AutoDomainData]
        public void Handle_NoIdProvided_TopLevelItemsReturned(IDocumentSession ravenSession, GroupReorderQueryHandler sut)
        {
            //Arrange
            PopulateData(ravenSession);

            // Act
            var result = sut.Handle(new GroupReorderQuery());

            // Assert
            AssertionHelpers.PropertiesAreEqual(topReport, result.Reports.Single());
            AssertionHelpers.PropertiesAreEqual(topGroup, result.Groups.Single());
        }
    }
}
