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
    }
}