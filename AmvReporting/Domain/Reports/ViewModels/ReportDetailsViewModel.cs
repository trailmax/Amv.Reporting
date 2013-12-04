using System.Collections.Generic;
using System.Web.Mvc;
using AmvReporting.Domain.Reports.Commands;

namespace AmvReporting.Domain.Reports.ViewModels
{
    public class ReportDetailsViewModel : CreateReportCommand
    {
        public List<SelectListItem> PossibleDatabases { get; set; }
    }
}