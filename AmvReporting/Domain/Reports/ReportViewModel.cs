using System;
using System.Web.Mvc;


namespace AmvReporting.Domain.Reports
{
    public class ReportViewModel
    {
        public String ReportGroupId { get; set; }

        public String Title { get; set; }

        public ReportType ReportType { get; set; }

        public String Description { get; set; }

        public String Sql { get; set; }

        public String JavaScript { get; set; }

        public String Css { get; set; }

        [AllowHtml]
        public String HtmlOverride { get; set; }

        public String DatabaseId { get; set; }

        public bool Enabled { get; set; }

        public int? ListOrder { get; set; }

        public Guid AggregateId { get; set; }
    }


    [Obsolete("This is the old model required only to do a migration")]
    public class Report
    {
        public String Id { get; set; }

        public String ReportGroupId { get; set; }

        public String Title { get; set; }

        public ReportType ReportType { get; set; }

        public String Description { get; set; }

        public String Sql { get; set; }

        public String JavaScript { get; set; }

        public String Css { get; set; }

        [AllowHtml]
        public String HtmlOverride { get; set; }

        public String DatabaseId { get; set; }

        public bool Enabled { get; set; }

        public int? ListOrder { get; set; }
    }
}