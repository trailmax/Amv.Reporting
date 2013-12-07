using System;
using AmvReporting.Domain.Reports;

namespace AmvReporting.Domain.Preview.ViewModels
{
    public class PreviewReportModel
    {
        public String DatabaseId { get; set; }
        
        public ReportType ReportType { get; set; }

        public String Sql { get; set; }

        public String JavaScript { get; set; }

        public String Css { get; set; }
    }
}