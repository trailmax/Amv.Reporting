using System.Collections.Generic;
using System.Web.Mvc;
using AmvReporting.Commands;

namespace AmvReporting.ViewModels
{
    public class ReportDetailsViewModel : CreateReportCommand
    {
        public List<SelectListItem> PossibleDatabases { get; set; }
    }
}