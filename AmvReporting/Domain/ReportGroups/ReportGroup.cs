using System;
using System.ComponentModel.DataAnnotations;

namespace AmvReporting.Domain.ReportGroups
{
    public class ReportGroup
    {
        public String Id { get; set; }

        [Required]
        public String Title { get; set; }

        public String ParentReportGroupId { get; set; }

        public int ListOrder { get; set; }

        public bool Enabled { get; set; }
    }
}