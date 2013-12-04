using System;
using System.Linq.Expressions;

namespace AmvReporting.Infrastructure.Helpers
{
    public static class ExpressionHelper
    {
        public static TResult PropertyValue<T, TResult>(T @object, Expression<Func<T, TResult>> selector)
        {
            var result = selector.Compile().Invoke(@object);

            return result;
        }

        public static String PropertyName<T>(Expression<Func<T, String>> selector)
        {
            var name = ((MemberExpression)selector.Body).Member.Name;
            return name;
        }
    }
}