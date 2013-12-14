using System;

namespace AmvReporting.Domain.ReportGroups.ViewModels
{
    public class ReportGroupIndexViewModel
    {
        public String Id { get; set; }
        public String ParentReportGroupId { get; set; }
        public String ParentFullName { get; set; }
        public String Title { get; set; }
    }
}