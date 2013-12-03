using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmvReporting.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace AmvReporting.Infrastructure.Raven
{
    public class DatabaseDetailsByIdIndex : AbstractIndexCreationTask<DatabaseDetails>
    {
        public DatabaseDetailsByIdIndex()
        {
            Index(x => x.Id, FieldIndexing.Default);
        }
    }
}