using System;
using System.Collections.Generic;
using System.Linq;
using AmvReporting.Domain.ReportGroups.Queries;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using AmvReporting.Infrastructure.ModelEnrichers;

namespace AmvReporting.Domain.ReportGroups.ViewModels
{
    public class ReportGroupViewModelEnricher : IModelEnricher<ReportGroupViewModel>
    {
        private readonly IMediator mediator;

        public ReportGroupViewModelEnricher(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ReportGroupViewModel Enrich(ReportGroupViewModel model)
        {
            var allGroups = mediator.Request(new AllReportGroupsQuery()).ToList();

            var visibleGroups = allGroups.Where(g => g.Id != model.Id).ToList();

            model.PossibleParents = visibleGroups.ToSelectListItems(t => GetParentPath(allGroups, t), v => v.Id).OrderBy(t => t.Text).ToList();

            return model;
        }

        private String GetParentPath(IEnumerable<ReportGroup> allGroups, ReportGroup reportGroup)
        {
            if (reportGroup == null)
            {
                return String.Empty;
            }

            if (String.IsNullOrEmpty(reportGroup.ParentReportGroupId))
            {
                return reportGroup.Title;
            }

            var parent = allGroups.FirstOrDefault(g => g.Id == reportGroup.ParentReportGroupId);

            return GetParentPath(allGroups, parent) + " => " + reportGroup.Title;
        }
    }
}