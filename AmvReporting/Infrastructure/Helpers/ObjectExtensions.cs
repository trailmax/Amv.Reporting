using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmvReporting.Infrastructure.Helpers
{
    public static class ObjectExtensions
    {
        public static String CheckForNull<T>(this T @object, Func<T, String> func)
        {
            if (@object == null)
            {
                // default(String) returns null, but we need empty string
                return String.Empty;
            }

            return func.Invoke(@object);
        }

        public static TResult CheckForNull<T, TResult>(this T @object, Func<T, TResult> func)
        {
            if (@object == null)
            {
                return default(TResult);
            }

            return func.Invoke(@object);
        }
    }
}