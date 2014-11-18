using System;
using System.Text.RegularExpressions;

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
                return Regex.Replace(value, "([A-Z][a-z]?)", " $1").Trim();
            }
            return null;
        }


        public static Guid ToGuid(this string value)
        {
            var guid = Guid.Empty;
            Guid.TryParse(value, out guid);
            return guid;
        }
    }
}