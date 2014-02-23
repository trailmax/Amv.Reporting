using System;
using System.Web.Mvc;
using AmvReporting.Domain.Reports;

namespace AmvReporting.Domain.Preview.ViewModels
{
    public class PreviewReportModel
    {
        public String DatabaseId { get; set; }
        
        public ReportType ReportType { get; set; }

        public String Sql { get; set; }

        [AllowHtml]
        public String JavaScript { get; set; }

        public String Css { get; set; }

        [AllowHtml]
        public String HtmlOverride { get; set; }
    }
}