using System;
using System.Diagnostics;
using System.Web.Mvc;


namespace AmvReporting.Domain.Reports
{
    [DebuggerDisplay("{Title}")]
    public class ReportViewModel
    {
        public Guid AggregateId { get; set; }

        public String ReportGroupId { get; set; }

        public String Title { get; set; }

        public Guid? TemplateId { get; set; }

        public String Description { get; set; }

        public String Sql { get; set; }

        public String JavaScript { get; set; }

        [AllowHtml]
        public String HtmlOverride { get; set; }

        public String DatabaseId { get; set; }

        public bool Enabled { get; set; }

        public int? ListOrder { get; set; }
    }
}