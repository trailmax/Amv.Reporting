using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AmvReporting.Tests.ZeroFriction
{
    public class AssertionHelpers
    {
        /// <summary>
        /// Compare two lists of objects. Order does matter - if lists are the same, but order is different, assertion fails.
        /// Comparison is done property by property, independent of the types of objects
        /// </summary>
        /// <typeparam name="TExpected"></typeparam>
        /// <typeparam name="TActual"></typeparam>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="ignored"></param>
        public static void ListsAreEqual<TExpected, TActual>(IEnumerable<TExpected> expected,
                                                                  IEnumerable<TActual> actual,
                                                                  params string[] ignored)
        {
            var expecteds = expected as IList<TExpected> ?? expected.ToList();
            var actuals = actual as IList<TActual> ?? actual.ToList();

            if (!expecteds.Any() && !actuals.Any())
            {
                Assert.True(false, "Both lists are empty. Can not compare");
            }

            if (expecteds.Count() != actuals.Count())
            {
                Assert.True(false, "Lists contain different number of objects");
            }

            for (int i = 0; i < expecteds.Count; i++)
            {
                PropertiesAreEqual(expecteds[i], actuals[i], ignored);
            }
        }


        public static void ListContains(object expected, IEnumerable<object> list, params string[] ignored)
        {
            var items = list as IList<object> ?? list.ToList();
            if (!items.Any())
            {
                Assert.True(false, "List is empty");
            }

            foreach (var item in items)
            {
                var assertionFails = PropertiesAreEqualHelper(expected, item, ignored.AsEnumerable());
                if (!assertionFails.Any())
                {
                    return;
                }
            }
            Assert.True(false, "The list does not contain expected object");
        }


        public static void PropertiesAreEqual(object expected, object actual, params string[] ignored)
        {
            PropertiesAreEqual(expected, actual, ignored.AsEnumerable());
        }


        /// <summary>
        /// Compare different objects for properties that match names.
        /// You can ignore properties with given names. Just supply names of properties (as string) to be ignored
        /// in the ignoredProperties parameter
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="ignoredProperties"></param>
        public static void PropertiesAreEqual(object expected, object actual, IEnumerable<string> ignoredProperties = null)
        {
            var assertionList = PropertiesAreEqualHelper(expected, actual, ignoredProperties);

            if (assertionList.Any())
            {
                Assert.True(false, String.Join(Environment.NewLine, assertionList));
            }
        }

        private static List<String> PropertiesAreEqualHelper(object expected, object actual, IEnumerable<string> ignoredProperties = null)
        {
            var assertionList = new List<string>(); // list of error messages

            if (expected == null && actual == null)
            {
                assertionList.Add("Both objects are null. Can't compare");
                return assertionList;
            }

            if (expected == null)
            {
                assertionList.Add("Expected object is null. Can't compare");
                return assertionList;
            }

            if (actual == null)
            {
                assertionList.Add("Actual object is null. Can't compare");
                return assertionList;
            }


            // if we try to compare objects that have equals defined on them, just use that!
            if (expected.Equals(actual))
            {
                return assertionList;
            }

            var expectedProperties = expected.GetType().GetProperties().Select(p => p.Name).ToList();

            var actualProperties = actual.GetType().GetProperties().Select(p => p.Name).ToList();

            // here is the list of all properties that are common to both objects
            var commonProperties = expectedProperties.Intersect(actualProperties).ToList();
            if (!commonProperties.Any())
            {
                assertionList.Add("Objects don't have any properties with similar names");
                return assertionList;
            }

            // if we need to ignore some of the properties, we handle this here
            if (ignoredProperties != null)
            {
                commonProperties = commonProperties.Except(ignoredProperties).ToList();
            }

            foreach (var propertyName in commonProperties)
            {
                var result = CompareProperty(expected, actual, propertyName);
                if (!String.IsNullOrEmpty(result))
                {
                    assertionList.Add(result);
                }
            }

            return assertionList;
        }


        private static String CompareProperty(object expectedObject, object actualObject, string propertyName)
        {
            var expectedProperty = expectedObject.GetType().GetProperty(propertyName);
            var expectedValue = expectedProperty.GetValue(expectedObject);

            var actualProperty = actualObject.GetType().GetProperty(propertyName);
            var actualValue = actualProperty.GetValue(actualObject, null);

            // if both values are null - they are equal
            if (expectedValue == null && actualValue == null)
            {
                return null;
            }

            // only one of the values are null, the other one is not
            if (expectedValue == null || actualValue == null)
            {
                return String.Format(
                        "Property {0}.{1} does not match. Expected: {2} but was: {3}",
                        actualProperty.DeclaringType != null ? actualProperty.DeclaringType.Name : "Unknown",
                        actualProperty.Name,
                        expectedValue ?? "NULL",
                        actualValue ?? "NULL");
            }

            // if one of the values is enum
            if (expectedValue.GetType().IsEnum || actualValue.GetType().IsEnum)
            {
                expectedValue = (int)expectedValue;
                actualValue = (int)actualValue;
            }

            if (!Equals(expectedValue, actualValue))
            {
                return String.Format(
                        "Property {0}.{1} does not match. Expected: {2} but was: {3}",
                        actualProperty.DeclaringType != null ? actualProperty.DeclaringType.Name : "Unknown",
                        actualProperty.Name,
                        expectedValue,
                        actualValue);
            }
            return null;
        }
    }
}
