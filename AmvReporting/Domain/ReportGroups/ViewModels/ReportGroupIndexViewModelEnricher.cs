using System.Collections.Generic;
using System.Linq;
using AmvReporting.Domain.ReportGroups.Queries;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.ModelEnrichers;

namespace AmvReporting.Domain.ReportGroups.ViewModels
{
    public class ReportGroupIndexViewModelEnricher : IModelEnricher<IEnumerable<ReportGroupIndexViewModel>>
    {
        private readonly IMediator mediator;

        public ReportGroupIndexViewModelEnricher(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public IEnumerable<ReportGroupIndexViewModel> Enrich(IEnumerable<ReportGroupIndexViewModel> model)
        {
            var allGroups = mediator.Request(new AllReportGroupsQuery()).ToList();

            foreach (var viewModel in model)
            {
                viewModel.ParentFullName = ReportGroupHelpers.GetParentPath(allGroups, viewModel);
            }

            model = model.OrderBy(r => r.ParentFullName).ToList();

            return model;
        }
    }
}