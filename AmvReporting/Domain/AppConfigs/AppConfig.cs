using System;

namespace AmvReporting.Domain.AppConfigs
{
    public class AppConfig
    {
        public string Id { get; set; }

        public String GlobalJavascript { get; set; }

        public String GlobalCss { get; set; }


        public static AppConfig New()
        {
            return new AppConfig
            {
                Id = IdString,
            };
        }

        public const String IdString = "App/Config";
    }
}