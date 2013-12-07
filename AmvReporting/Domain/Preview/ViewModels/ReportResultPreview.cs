using System;

namespace AmvReporting.Domain.Preview.ViewModels
{
    public class ReportResultPreview
    {
        public String Data { get; set; }
        public String Columns { get; set; }
        public String JavaScript { get; set; }
        public String Css { get; set; }
    }
}