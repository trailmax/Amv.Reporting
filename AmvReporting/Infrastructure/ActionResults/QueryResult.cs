using System;
using System.Web.Mvc;
using AmvReporting.Infrastructure.CQRS;
using AutoMapper;


namespace AmvReporting.Infrastructure.ActionResults
{
    public class QueryResult<TResult> : ViewResult
    {
        private readonly IQuery<TResult> query;
        private Type destinationType;

        public QueryResult(IQuery<TResult> query)
        {
            this.query = query;
        }


        public QueryResult<TResult> MapTo<TDest>()
        {
            destinationType = typeof(TDest);
            return this;
        }


        public override void ExecuteResult(ControllerContext context)
        {
            var mediator = DependencyResolver.Current.GetService<IMediator>();

            var result = mediator.Request(query);
            ViewData.Model = result;

            if (destinationType != null)
            {
                var mappedResult = Mapper.Map(result, typeof(TResult), destinationType);
                ViewData.Model = mappedResult;
            }



            base.ExecuteResult(context);
        }
    }
}