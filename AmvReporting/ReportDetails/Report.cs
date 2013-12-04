using System;

namespace AmvReporting.Models
{
    public class Report
    {
        public String Id { get; set; }

        public String Title { get; set; }

        public String LinkName { get; set; }

        public ReportType ReportType { get; set; }

        public String Description { get; set; }

        public String Sql { get; set; }

        public String JavaScript { get; set; }

        public String Css { get; set; }

        public String DatabaseId { get; set; }
    }
}