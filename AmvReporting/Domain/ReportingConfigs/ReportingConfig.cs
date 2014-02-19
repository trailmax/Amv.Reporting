using System;

namespace AmvReporting.Domain.ReportingConfigs
{
    public class ReportingConfig
    {
        public string Id { get; set; }

        public String GlobalJavascript { get; set; }

        public String GlobalCss { get; set; }


        public static ReportingConfig New()
        {
            return new ReportingConfig
            {
                Id = IdString,
            };
        }

        public const String IdString = "Reporting/Config";
    }
}