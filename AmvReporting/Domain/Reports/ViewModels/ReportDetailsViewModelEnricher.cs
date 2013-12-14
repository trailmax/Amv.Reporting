using System.Linq;
using AmvReporting.Domain.DatabaseConnections.Queries;
using AmvReporting.Domain.ReportGroups;
using AmvReporting.Domain.ReportGroups.Queries;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using AmvReporting.Infrastructure.ModelEnrichers;

namespace AmvReporting.Domain.Reports.ViewModels
{
    public class ReportDetailsViewModelEnricher : IModelEnricher<ReportDetailsViewModel>, IModelEnricher<EditReportDetailsViewModel>
    {
        private readonly IMediator mediator;

        public ReportDetailsViewModelEnricher(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ReportDetailsViewModel Enrich(ReportDetailsViewModel model)
        {
            PopulateDropdowns(model);

            return model;
        }

        public EditReportDetailsViewModel Enrich(EditReportDetailsViewModel model)
        {
            PopulateDropdowns(model);

            return model;
        }

        private void PopulateDropdowns(ReportDetailsViewModel model)
        {
            var databases = mediator.Request(new AllDatabasesQuery());

            model.PossibleDatabases = databases.ToSelectListItems(t => t.Name, v => v.Id);

            var allGroups = mediator.Request(new AllReportGroupsQuery()).ToList();

            model.PossibleGroups = allGroups
                .ToSelectListItems(t => ReportGroupHelpers.GetParentPath(allGroups, t), v => v.Id)
                .OrderBy(t => t.Text)
                .ToList();
        }
    }
}