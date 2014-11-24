using System.Collections.Generic;
using System.Web.Mvc;
using AmvReporting.Domain.Reports.Commands;

namespace AmvReporting.Domain.Reports.ViewModels
{
    //TODO REMOVE
    public class ReportDetailsViewModel : CreateReportCommand
    {
        public List<SelectListItem> PossibleDatabases { get; set; }
        public List<SelectListItem> PossibleGroups { get; set; }
    }
}