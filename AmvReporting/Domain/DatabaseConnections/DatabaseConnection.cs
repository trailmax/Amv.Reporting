using System;

namespace AmvReporting.Domain.DatabaseConnections
{
    public class DatabaseConnection
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String ConnectionString { get; set; }
    }
}