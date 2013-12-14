using System;
using System.ComponentModel.DataAnnotations;

namespace AmvReporting.Domain.Reports.ViewModels
{
    public class ReportIndexViewModel
    {
        public String Id { get; set; }
        public String ReportGroupId { get; set; }

        [Display(Name = "Group")]
        public String GroupFullName { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
    }
}