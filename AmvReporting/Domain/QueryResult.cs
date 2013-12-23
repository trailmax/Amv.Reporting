using System;

namespace AmvReporting.Domain
{
    public class QueryResult
    {
        public bool IsSuccess { get; set; }
        public String Payload { get; set; }
        public String ErrorMessage { get; set; }
    }
}