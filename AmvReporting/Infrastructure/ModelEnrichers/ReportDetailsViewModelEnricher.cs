﻿using AmvReporting.Domain.DatabaseConnections.Queries;
using AmvReporting.Domain.ReportDetails.ViewModels;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using AmvReporting.Queries;

namespace AmvReporting.Infrastructure.ModelEnrichers
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
            PopulateDatabases(model);

            return model;
        }

        public EditReportDetailsViewModel Enrich(EditReportDetailsViewModel model)
        {
            PopulateDatabases(model);

            return model;
        }

        private void PopulateDatabases(ReportDetailsViewModel model)
        {
            var databases = mediator.Request(new AllDatabasesQuery());

            model.PossibleDatabases = databases.ToSelectListItems(t => t.Name, v => v.Id);
        }
    }
}