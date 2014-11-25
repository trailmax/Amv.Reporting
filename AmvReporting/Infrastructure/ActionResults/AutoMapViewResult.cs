using System;
using System.Web.Mvc;
using AutoMapper;

namespace AmvReporting.Infrastructure.ActionResults
{
    public class AutoMapViewResult<TDestination> : ViewResult
    {
        public AutoMapViewResult(String viewName, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            ViewName = viewName;
            ViewData = viewData;
            TempData = tempData;
        }

        public AutoMapViewResult(ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            ViewData = viewData;
            TempData = tempData;
        }


        public override void ExecuteResult(ControllerContext context)
        {
            // get the model from ViewData dictionary
            var model = ViewData.Model;

            if (model != null)
            {
                ViewData.Model = Mapper.Map(model, model.GetType(), typeof(TDestination));
            }

            base.ExecuteResult(context);
        }
    }
}