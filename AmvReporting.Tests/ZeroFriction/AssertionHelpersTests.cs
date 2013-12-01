using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace AmvReporting.Tests.ZeroFriction
{
    public class AssertionHelpersTests
    {
        [Fact]
        public void PropertiesAreEqual_EqualObjects_Match()
        {
            var firstObject = new FirstObject() { String = "Hello world", MatchesAgain = 3, DoesNotMatch = 42 };
            var secondObject = new SecondObject()
                                   {
                                       String = firstObject.String,
                                       MatchesAgain = firstObject.MatchesAgain,
                                       SomeObjectThatDoesNotMatch = new char[3] { 'a', 'b', 'c' }
                                   };
            AssertionHelpers.PropertiesAreEqual(firstObject, secondObject);
        }


        [Fact]
        public void PropertiesAreEqual_ObjectsAreNotEqual_Throws()
        {
            var firstObject = new FirstObject() { String = "Hello world", MatchesAgain = 3, DoesNotMatch = 42 };
            var secondObject = new SecondObject()
                                   {
                                       String = "Something New",
                                       MatchesAgain = firstObject.MatchesAgain,
                                       SomeObjectThatDoesNotMatch = new char[3] { 'a', 'b', 'c' }
                                   };
            Assert.Throws<TrueException>(() => AssertionHelpers.PropertiesAreEqual(firstObject, secondObject));
        }


        [Fact]
        public void PropertiesAreEqual_IgnoredProperty_IsIgnored()
        {
            var firstObject = new FirstObject() { String = "Hello world", MatchesAgain = 3, DoesNotMatch = 42 };
            var secondObject = new SecondObject()
                                   {
                                       String = firstObject.String,
                                       MatchesAgain = 555,
                                       SomeObjectThatDoesNotMatch = new char[] { 'a', 'b', 'c' }
                                   };
            AssertionHelpers.PropertiesAreEqual(firstObject, secondObject, new List<string>() { "MatchesAgain" });
        }

        [Fact]
        public void PropertiesAreEqual_EqualEnums_Match()
        {
            var firstObject = new FirstObject() { EnumThingie = 1 };
            var secondObject = new SecondObject() { EnumThingie = EnumThang.One };
            AssertionHelpers.PropertiesAreEqual(firstObject, secondObject);
        }

        [Fact]
        public void PropertiesAreEqual_NotEqualEnums_Thorws()
        {
            var firstObject = new FirstObject() { EnumThingie = 1 };
            var secondObject = new SecondObject() { EnumThingie = EnumThang.Two };
            Assert.Throws<TrueException>(() => AssertionHelpers.PropertiesAreEqual(firstObject, secondObject));
        }

        [Fact]
        public void PropertiesAreEqual_NullableIntegers_Match()
        {
            var firstObject = new FirstObject() { NullableInt = 3 };
            var secondObject = new SecondObject() { NullableInt = 3 };
            AssertionHelpers.PropertiesAreEqual(firstObject, secondObject);
        }

        [Fact]
        public void PropertiesAreEqual_NullableIntegersAreNull_Match()
        {
            var firstObject = new FirstObject() { NullableInt = null };
            var secondObject = new SecondObject() { NullableInt = null };
            AssertionHelpers.PropertiesAreEqual(firstObject, secondObject);
        }

        [Fact]
        public void PropertiesAreEqual_NullableIntegersWhenOneNull_NOT_Match()
        {
            var firstObject = new FirstObject() { NullableInt = 3 };
            var secondObject = new SecondObject() { NullableInt = null };
            Assert.Throws<TrueException>(() => AssertionHelpers.PropertiesAreEqual(firstObject, secondObject));
        }

        [Fact]
        public void PropertiesAreEqual_ObjectsOfSameType_PropertiesMatch()
        {
            var one = new FirstObject() { String = "Hello", MatchesAgain = 1, NullableInt = null };
            var two = new FirstObject() { String = "Hello", MatchesAgain = 1, NullableInt = null };

            AssertionHelpers.PropertiesAreEqual(one, two);
        }

        [Fact]
        public void PropertiesAreEqual_SameTypeDifferentValues_Throws()
        {
            var one = new FirstObject() { String = "Hello", NullableInt = 5 };
            var two = new FirstObject() { String = "Hello", NullableInt = null };

            Assert.Throws<TrueException>(() => AssertionHelpers.PropertiesAreEqual(one, two));
        }




        private class FirstObject
        {
            public String String { get; set; }
            public int MatchesAgain { get; set; }
            public int DoesNotMatch { get; set; }
            public int EnumThingie { get; set; }
            public int? NullableInt { get; set; }
            public Person Person { get; set; }
        }
        private class SecondObject
        {
            public String String { get; set; }
            public int MatchesAgain { get; set; }
            public char[] SomeObjectThatDoesNotMatch { get; set; }
            public EnumThang EnumThingie { get; set; }
            public int? NullableInt { get; set; }
        }
        private enum EnumThang
        {
            One = 1,
            Two = 2
        }


        [Fact]
        public void PropertiesAreEqual_ClassesMatchingNamesDifferentTypes_Throws()
        {
            var jay = new Jay() { ClassId = 1, NameMatch = true };
            var bob = new Bob() { ClassId = 1, NameMatch = "something" };

            Assert.Throws<TrueException>(() => AssertionHelpers.PropertiesAreEqual(jay, bob));
        }

        [Fact]
        public void PropertiesAreEqual_CompareStrings_ExpectedBehaviour()
        {
            var one = "hello";
            var two = "hello";

            AssertionHelpers.PropertiesAreEqual(one, two);
        }

        [Fact]
        public void PropertiesAreEqual_ObjectsWithNoMatchingProps_Throws()
        {
            var jay = new Bob() { ClassId = 1, NameMatch = "hello" };
            var first = new FirstObject() { String = "Hello", NullableInt = 5 };

            Assert.Throws<TrueException>(() => AssertionHelpers.PropertiesAreEqual(jay, first));
        }

        [Fact]
        public void ListsAreEqual_BothEmpty_Throws()
        {
            //Arrange
            var expected = new List<Jay>();
            var actual = new List<Bob>();

            // Act && Assert
            Assert.Throws<TrueException>(() => AssertionHelpers.ListsAreEqual(expected, actual));
        }

        [Fact]
        public void ListsAreEqual_DifferentNumberOfEntries_Throws()
        {
            //Arrange
            var expected = new List<Jay>() { new Jay() };
            var actual = new List<Bob>() { new Bob(), new Bob() };

            // Act && Assert
            Assert.Throws<TrueException>(() => AssertionHelpers.ListsAreEqual(expected, actual));
        }

        [Fact]
        public void ListsAreEqual_DifferentObjects_Throws()
        {
            //Arrange
            var expected = new List<Jay>() { new Jay() { ClassId = 1 }, new Jay() { ClassId = 2 } };
            var actual = new List<Bob>() { new Bob() { ClassId = 11 }, new Bob() { ClassId = 12 } };

            // Act && Assert
            Assert.Throws<TrueException>(() => AssertionHelpers.ListsAreEqual(expected, actual));
        }

        [Fact]
        public void ListsAreEqual_SameObjectsDifferentOrder_Throws()
        {
            //Arrange
            var expected = new List<Jay>() { new Jay() { ClassId = 1 }, new Jay() { ClassId = 2 } };
            var actual = new List<Bob>() { new Bob() { ClassId = 2 }, new Bob() { ClassId = 1 } };

            // Act && Assert
            Assert.Throws<TrueException>(() => AssertionHelpers.ListsAreEqual(expected, actual));
        }

        [Fact]
        public void ListsAreEqual_SameObjects_Passes()
        {
            //Arrange
            var expected = new List<Jay>() { new Jay() { ClassId = 1 }, new Jay() { ClassId = 2 } };
            var actual = new List<Bob>() { new Bob() { ClassId = 1 }, new Bob() { ClassId = 2 } };

            // Act && Assert
            Assert.DoesNotThrow(() => AssertionHelpers.ListsAreEqual(expected, actual, "NameMatch"));
        }

        private class Jay
        {
            public int ClassId { get; set; }
            public bool NameMatch { get; set; }
        }
        private class Bob
        {
            public int ClassId { get; set; }
            public String NameMatch { get; set; }
        }


        [Fact]
        public void ListContains_EmptyList_ThrowsFail()
        {
            //Arrange
            var list = new List<Jay>();
            var item = new Bob();

            // Act && Assert
            Assert.Throws<TrueException>(() => AssertionHelpers.ListContains(item, list));
        }

        [Fact]
        public void ListContains_ListWithElement_AssertsTrue()
        {
            var list = new List<Bob>() { new Bob() { ClassId = 1, NameMatch = "four" } };
            var item = new Bob() { ClassId = 1, NameMatch = "four" };

            Assert.DoesNotThrow(() => AssertionHelpers.ListContains(item, list));
        }

        [Fact]
        public void ListContains_ListWithNoMatch_AssertionFails()
        {
            var list = new List<Bob>() { new Bob() { ClassId = 1, NameMatch = "four" } };
            var item = new Bob() { ClassId = 1, NameMatch = "Seven" };

            Assert.Throws<TrueException>(() => AssertionHelpers.ListContains(item, list));
        }

        [Fact]
        public void ListContains_MatchingObjectOfDifferentType_AssertionPasses()
        {
            var list = new List<FirstObject>() { new FirstObject() { String = "hello", MatchesAgain = 1 } };
            var item = new SecondObject() { String = "hello", MatchesAgain = 1 };

            Assert.DoesNotThrow(() => AssertionHelpers.ListContains(item, list));
        }

        [Fact]
        public void ListContains_NotMatchingObjectOfDifferentType_AssertionFails()
        {
            var list = new List<FirstObject>() { new FirstObject() { String = "hello", MatchesAgain = 1 } };
            var item = new SecondObject() { String = "hello", MatchesAgain = 5 };

            Assert.Throws<TrueException>(() => AssertionHelpers.ListContains(item, list));
        }
    }

    internal class Person
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public int Age { get; set; }
        public String Nin { get; set; }
        public int PersonId { get; set; }
    }
}