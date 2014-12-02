using System;
using System.Collections.Generic;


namespace AmvReporting.Domain
{
    public class EventStoreMigrationDictonary : Dictionary<string, Guid>
    {
        public const string DefaultId = "migration/dictionary";

        public String Id 
        {
            get
            {
                return DefaultId;
            } 
        }
    }
}