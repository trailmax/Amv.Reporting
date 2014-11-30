using System;


namespace AmvReporting.Domain.Preview.ViewModels
{
    public class ReportResultPreview
    {
        public String Data { get; set; }
        public String ReportJavaScript { get; set; }
        public String GlobalJs { get; set; }
        public String ReportCss { get; set; }
        public String GlobalCss { get; set; }
        public String ReportHtml { get; set; }
        public String TemplateJavascript { get; set; }
        public String TemplateHtml { get; set; }
    }
}