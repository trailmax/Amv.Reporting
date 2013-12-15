using System;
using System.Collections.Generic;
using System.Linq;
using AmvReporting.Domain.ReportGroups;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.Menus
{
    public class MenuModelQuery : IQuery<MenuModel>
    {
    }

    public class MenuModelQueryHandler : IQueryHandler<MenuModelQuery, MenuModel>
    {
        private readonly IDocumentSession ravenSession;

        public MenuModelQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public MenuModel Handle(MenuModelQuery query)
        {
            var allGroups = ravenSession.Query<ReportGroup>().ToList();
            var allReports = ravenSession.Query<Report>().ToList();

            var menuModel = new MenuModel
                            {
                                TopLevelReports = allReports.Where(r => String.IsNullOrEmpty(r.ReportGroupId)).ToList(),
                                MenuNodes = new List<MenuNode>(),
                            };

            foreach (var @group in allGroups.Where(g => String.IsNullOrEmpty(g.ParentReportGroupId)))
            {
                menuModel.MenuNodes.Add(BuildTree(@group, allGroups, allReports));
            }


            return menuModel;
        }


        private MenuNode BuildTree(ReportGroup @group, List<ReportGroup> allGroups, List<Report> allReports)
        {
            var menuNode = new MenuNode()
                           {
                               ReportGroupId = @group.Id,
                               ReportGroupTitle = @group.Title,
                               Reports = allReports.Where(r => r.ReportGroupId == @group.Id).ToList(),
                               MenuNodes = new List<MenuNode>(),
                           };
            foreach (var childGroup in allGroups.Where(g => g.ParentReportGroupId == @group.Id))
            {
                menuNode.MenuNodes.Add(BuildTree(childGroup, allGroups, allReports));
            }

            return menuNode;
        }
    }
}