using System;
using Omu.ValueInjecter;
using Xunit;


namespace AmvReporting.Tests.Explorage
{
    public class ValueInjectorTests
    {

        [Fact]
        public void FactMethodName()
        {
            var anotherClass = new MyAnotherClass()
            {
                Value1 = Guid.NewGuid().ToString(),
                Value2 = Guid.NewGuid().ToString(),
                Value3 = Guid.NewGuid().ToString()
            };

            var myClass = new MyClass(anotherClass);

            Assert.Equal(anotherClass.Value1, myClass.Value1);
            Assert.Equal(anotherClass.Value2, myClass.Value2);
        }

        public class MyClass
        {
            public String Value1 { get; set; }
            public String Value2 { get; set; }


            public MyClass(MyAnotherClass anotherClass)
            {
                this.InjectFrom(anotherClass);
            }
        }

        public class MyAnotherClass
        {
            public String Value1 { get; set; }
            public String Value2 { get; set; }
            public String Value3 { get; set; }
        }
    }
}
