using System;
using System.Collections.Generic;
using System.Linq;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;
using Raven.Client.Linq;

namespace AmvReporting.Domain.ReportGroups.Queries
{
    public class GroupReorderModel
    {
        public String ParentGroupTitle { get; set; }
        public String ParentGroupId { get; set; }
        public List<ReportGroup> Groups { get; set; }
        public List<ReportViewModel> Reports { get; set; }
    }

    public class GroupReorderQuery : IQuery<GroupReorderModel>
    {
        public string GroupId { get; set; }

        public GroupReorderQuery(string groupId = null)
        {
            GroupId = groupId;
        }
    }

    public class GroupReorderQueryHandler : IQueryHandler<GroupReorderQuery, GroupReorderModel>
    {
        private readonly IDocumentSession ravenSession;

        public GroupReorderQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public GroupReorderModel Handle(GroupReorderQuery query)
        {
            var childReports = ravenSession.Query<ReportViewModel>()
                .Where(r => r.ReportGroupId == query.GroupId)
                .ToList()
                .OrderBy(r => r.ListOrder)
                .ToList();

            var childGroups = ravenSession.Query<ReportGroup>()
                .Where(rg => rg.ParentReportGroupId == query.GroupId)
                .ToList()
                .OrderBy(rg => rg.ListOrder)
                .ToList();


            var model = new GroupReorderModel()
                        {
                            Reports = childReports,
                            Groups = childGroups,
                        };

            if (query.GroupId != null)
            {
                var parentGroup = ravenSession.Load<ReportGroup>(query.GroupId);
                model.ParentGroupTitle = parentGroup.Title;
                model.ParentGroupId = parentGroup.Id;
            }

            return model;
        }
    }
}