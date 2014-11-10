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
        public bool ShowDisabledReports { get; set; }

        public MenuModelQuery(bool showDisabledReports = false)
        {
            ShowDisabledReports = showDisabledReports;
        }
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
            List<ReportGroup> allGroups;

            List<ReportViewModel> allReports;

            if (query.ShowDisabledReports)
            {
                allReports = ravenSession.Query<ReportViewModel>().ToList();
                allGroups = ravenSession.Query<ReportGroup>().ToList();
            }
            else
            {
                allReports = ravenSession.Query<ReportViewModel>().Where(r => r.Enabled).ToList();
                allGroups = ravenSession.Query<ReportGroup>().Where(g => g.Enabled).ToList();
            }

            var menuModel = new MenuModel
                            {
                                TopLevelReports = allReports.Where(r => String.IsNullOrEmpty(r.ReportGroupId)).OrderBy(r => r.ListOrder).ToList(),
                                MenuNodes = new List<MenuNode>(),
                            };

            foreach (var @group in allGroups.Where(g => String.IsNullOrEmpty(g.ParentReportGroupId)).OrderBy(g => g.ListOrder))
            {
                menuModel.MenuNodes.Add(BuildTree(@group, allGroups, allReports));
            }


            return menuModel;
        }


        private MenuNode BuildTree(ReportGroup @group, List<ReportGroup> allGroups, List<ReportViewModel> allReports)
        {
            var menuNode = new MenuNode()
                           {
                               ReportGroupParentId = @group.ParentReportGroupId,
                               ReportGroupId = @group.Id,
                               ReportGroupTitle = @group.Title,
                               Enabled = @group.Enabled,
                               Reports = allReports.Where(r => r.ReportGroupId == @group.Id).OrderBy(r => r.ListOrder).ToList(),
                               MenuNodes = new List<MenuNode>(),
                           };
            foreach (var childGroup in allGroups.Where(g => g.ParentReportGroupId == @group.Id).OrderBy(r => r.ListOrder))
            {
                menuNode.MenuNodes.Add(BuildTree(childGroup, allGroups, allReports));
            }

            return menuNode;
        }
    }
}