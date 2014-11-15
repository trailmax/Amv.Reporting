using System;
using System.Collections.Generic;


namespace AmvReporting.Domain
{
    public class MigrationDictonary : Dictionary<string, Guid>
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