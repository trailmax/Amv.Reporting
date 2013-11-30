using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.Sterling.Database;
using Wintellect.Sterling.Serialization;
using Xunit;

namespace AmvReporting.Tests
{
    public class SterlingExplorationTests
    {
        [Fact]
        public void TrySterling()
        {
            
        }
    }

    public class TestDatabaseInstance : BaseDatabaseInstance
    {
        protected override List<ITableDefinition> RegisterTables()
        {
            return new List<ITableDefinition>
                       {
                           CreateTableDefinition<TestModel, int>(testModel => testModel.Key)
                               .WithIndex<TestModel, string, int>("DataIndex", t => t.Data)
                               .WithIndex<TestModel, DateTime, string, int>("IndexDateData", t => Tuple.Create(t.Date, t.Data))                                                     
                       };
            throw new NotImplementedException();
        }
    }

    public class TestModel
    {
        public TestModel()
        {
            SubClass = new TestSubclass();
        }
        
        public int Key { get; set; }

        private static int Idx;
         
        public const int SampleConstant = 2;


        public string Data { get; set; }

        [SterlingIgnore]
        public string Data2 { get; set; }

        public TestSubclass SubClass { get; set; }

        public TestSubStruct SubStruct { get; set; }

        public DateTime Date { get; set; }
    }

    public class TestSubclass
    {
        public string NestedText { get; set; }
        public TestSubStruct SubStruct { get; set; }
    }

    public struct TestSubStruct
    {
        public int NestedId;
        public string NestedString;
    }
}
