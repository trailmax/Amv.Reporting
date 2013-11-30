using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmvReporting.Infrastructure.CQRS
{
    public interface IQueryHandler<in TQuery, out TResult>
        where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
}