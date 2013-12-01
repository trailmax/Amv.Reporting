using System;
using System.Web.Mvc;
using AutoMapper;

namespace AmvReporting.Infrastructure.ActionResults
{
    public class AutoMapViewResult<TDestination> : EnrichViewResult<TDestination> where TDestination : class
    {
        public AutoMapViewResult(String viewName, ViewDataDictionary viewData, TempDataDictionary tempData)
            : base(viewName, viewData, tempData)
        {
            // nothing to see here, move on.
        }

        public AutoMapViewResult(ViewDataDictionary viewData, TempDataDictionary tempData)
            : base(viewData, tempData)
        {
            // and here there is whole lot of nothing
        }


        public override void ExecuteResult(ControllerContext context)
        {
            // get the model from ViewData dictionary
            var model = ViewData.Model;

            // Mapper.Map(object Source, Type SourceType, Type DestinationType)
            ViewData.Model = Mapper.Map(model, model.GetType(), typeof(TDestination));

            base.ExecuteResult(context);
        }
    }
}