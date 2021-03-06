using System;
using System.Collections.Generic;
using System.Globalization;


namespace AmvReporting.Infrastructure.NEventStore
{
    public static class EventHeaderHelpers
    {
        public static DateTime? GetCommitDate(this Dictionary<String, object> headers)
        {
            object savedObject;
            headers.TryGetValue(MessageHeaders.CommitDate, out savedObject);
            if (savedObject == null)
            {
                return null;
            }

            var dateString = savedObject.ToString();

            DateTime parsedDate;
            if (DateTime.TryParseExact(dateString, MessageHeaders.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return parsedDate;
            }
            return null;
        }


        public static String GetUsername(this Dictionary<String, object> headers)
        {
            object savedObject;
            headers.TryGetValue(MessageHeaders.Username, out savedObject);
            if (savedObject == null)
            {
                return "Anonymous";
            }

            if (String.IsNullOrWhiteSpace(savedObject.ToString()))
            {
                return "Anonymous";
            }

            return savedObject.ToString();
        }
    }
}