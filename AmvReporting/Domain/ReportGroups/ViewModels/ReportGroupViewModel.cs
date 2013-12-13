using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AmvReporting.Domain.ReportGroups.ViewModels
{
    public class ReportGroupViewModel
    {
        public String Id { get; set; }

        [Required]
        public String Title { get; set; }

        [Display(Name = "Parent Group")]
        public String ParentReportGroupId { get; set; }

        public List<SelectListItem> PossibleParents { get; set; }
    }
}