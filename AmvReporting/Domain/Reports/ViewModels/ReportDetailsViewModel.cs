using System.Collections.Generic;
using System.Web.Mvc;
using AmvReporting.Domain.ReportDetails.Commands;

namespace AmvReporting.Domain.ReportDetails.ViewModels
{
    public class ReportDetailsViewModel : CreateReportCommand
    {
        public List<SelectListItem> PossibleDatabases { get; set; }
    }
}