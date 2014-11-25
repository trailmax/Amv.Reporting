using System;
using System.Web.Mvc;


namespace AmvReporting.Domain.Preview.ViewModels
{
    public class PreviewReportModel
    {
        public Guid AggregateId { get; set; }

        public String Sql { get; set; }

        [AllowHtml]
        public String JavaScript { get; set; }

        public String Css { get; set; }

        [AllowHtml]
        public String HtmlOverride { get; set; }
    }
}