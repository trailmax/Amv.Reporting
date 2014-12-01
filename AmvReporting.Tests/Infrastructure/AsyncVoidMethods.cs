using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;


namespace AmvReporting.Tests.Infrastructure
{
    public class AsyncVoidMethods
    {
        [Fact]
        public void EnsureNoAsyncVoidInProduction()
        {
            AssertNoAsyncVoidMethods(typeof(MvcApplication).Assembly);
        }

        [Fact]
        public void EnsureNoAsyncVoidInTests()
        {
            AssertNoAsyncVoidMethods(typeof(AsyncVoidMethods).Assembly);
        }



        public static void AssertNoAsyncVoidMethods(Assembly assembly)
        {
            var messages = assembly
                .GetAsyncVoidMethods()
                .Select(method =>
                    String.Format("'{0}.{1}' is an async void method.",
                        method.DeclaringType.Name,
                        method.Name))
                .ToList();
            Assert.False(messages.Any(), "Async void methods found!" + Environment.NewLine + String.Join(Environment.NewLine, messages));
        }
    }


    public static class AsyncVoidExtensions
    {
        public static IEnumerable<MethodInfo> GetAsyncVoidMethods(this Assembly assembly)
        {
            return assembly.GetLoadableTypes()
              .SelectMany(type => type.GetMethods(
                BindingFlags.NonPublic
                | BindingFlags.Public
                | BindingFlags.Instance
                | BindingFlags.Static
                | BindingFlags.DeclaredOnly))
              .Where(method => method.HasAttribute<AsyncStateMachineAttribute>())
              .Where(method => method.ReturnType == typeof(void));
        }

        public static bool HasAttribute<TAttribute>(this MethodInfo method) where TAttribute : Attribute
        {
            return method.GetCustomAttributes(typeof(TAttribute), false).Any();
        }

        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}
