using System;
using System.Text.RegularExpressions;
using System.Web;


namespace AmvReporting.Infrastructure.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Insert spaces before capital letter in the string. I.e. "HelloWorld" turns into "Hello World"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSeparatedWords(this string value)
        {
            if (value != null)
            {
                var regex = @"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])";
                var r = new Regex(regex);
                return r.Replace(value, " ").Trim();
            }
            return null;
        }

        public static string LowerCasePrepositions(this string value)
        {
            const string Prepositions = "As At But By For In Of Off On Onto Per Than To Up Via With";
            var tokens = value.Split(' ');
            if (tokens.Length == 1)
            {
                return value;
            }
            // Always leave the first and last word capitalised
            for (var i = 1; i < tokens.Length - 1; i++)
            {
                if (Prepositions.Contains(tokens[i]))
                {
                    tokens[i] = tokens[i].ToLower();
                }
            }
            return string.Join(" ", tokens);
        }


        public static Guid ToGuid(this string value)
        {
            var guid = Guid.Empty;
            Guid.TryParse(value, out guid);
            return guid;
        }


        public static String Unescape(this String source)
        {
            var unescaped = Regex.Unescape(source);
            var htmlDecode = HttpUtility.HtmlDecode(unescaped);

            return htmlDecode;
        }
    }
}