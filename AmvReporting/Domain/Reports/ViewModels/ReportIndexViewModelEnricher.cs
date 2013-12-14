using System.Collections.Generic;
using System.Linq;
using AmvReporting.Domain.ReportGroups;
using AmvReporting.Domain.ReportGroups.Queries;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.ModelEnrichers;

namespace AmvReporting.Domain.Reports.ViewModels
{
    public class ReportIndexViewModelEnricher : IModelEnricher<IEnumerable<ReportIndexViewModel>>
    {
        private readonly IMediator mediator;

        public ReportIndexViewModelEnricher(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public IEnumerable<ReportIndexViewModel> Enrich(IEnumerable<ReportIndexViewModel> model)
        {
            var allReportGroups = mediator.Request(new AllReportGroupsQuery()).ToList();

            foreach (var viewModel in model)
            {
                var group = allReportGroups.FirstOrDefault(g => g.Id == viewModel.ReportGroupId);

                viewModel.GroupFullName = ReportGroupHelpers.GetParentPath(allReportGroups, group);
            }

            model = model.OrderBy(m => m.GroupFullName)
                .ThenBy(m => m.Title);

            return model;
        }
    }
}