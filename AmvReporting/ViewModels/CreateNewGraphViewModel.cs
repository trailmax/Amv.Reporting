using System;
using System.ComponentModel.DataAnnotations;
using AmvReporting.Models;

namespace AmvReporting.ViewModels
{
    public class CreateNewGraphViewModel
    {
        public String Title { get; set; }
        public String LinkName { get; set; }

        [DataType(DataType.MultilineText)]
        public String Description { get; set; }

        [DataType(DataType.MultilineText)]
        public String Sql { get; set; }

        [DataType(DataType.MultilineText)]
        public String JavaScript { get; set; }

        [DataType(DataType.MultilineText)]
        public String Css { get; set; }

        public ReportType ReportType { get; set; }
    }
}