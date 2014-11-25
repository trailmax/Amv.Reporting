using System;
using System.ComponentModel.DataAnnotations;


namespace AmvReporting.Domain.ReportGroups.ViewModels
{
    public class ReportGroupViewModel
    {
        public ReportGroupViewModel()
        {
            Enabled = true;
        }

        public String Id { get; set; }

        [Required]
        public String Title { get; set; }

        [Display(Name = "Parent Group")]
        public String ParentReportGroupId { get; set; }

        public bool Enabled { get; set; }
    }
}