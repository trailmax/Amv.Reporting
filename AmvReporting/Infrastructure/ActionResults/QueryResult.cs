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
        private bool doJson;
        private bool jsonAllowGet;
        private String viewName;


        public QueryResult(IQuery<TResult> query, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            this.query = query;
            ViewData = viewData;
            TempData = tempData;
        }


        public QueryResult<TResult> MapTo<TDest>()
        {
            destinationType = typeof(TDest);
            return this;
        }

        public QueryResult<TResult> ShowView(String view)
        {
            this.ViewName = view;
            return this;
        }

        public QueryResult<TResult> DoJson(bool allowGet = false)
        {
            this.doJson = true;
            this.jsonAllowGet = allowGet;
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

            if (doJson)
            {
                var jsonResult = new JsonResult()
                                     {
                                         Data = ViewData.Model,
                                     };
                if (jsonAllowGet)
                {
                    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                }

                jsonResult.ExecuteResult(context);
            }
            else
            {
                base.ExecuteResult(context);
            }
        }
    }
}