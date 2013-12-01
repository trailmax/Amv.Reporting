using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AmvReporting.Infrastructure.ModelEnrichers;


namespace AmvReporting.Infrastructure.ActionResults
{
    public class EnrichViewResult<TDestination> : ViewResult
    {
        public EnrichViewResult(String viewName, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            ViewName = viewName;
            ViewData = viewData;
            TempData = tempData;
        }

        public EnrichViewResult(ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            ViewData = viewData;
            TempData = tempData;
        }


        public override void ExecuteResult(ControllerContext context)
        {
            // From DI container get an instance of enricher that works with our particular class
            var enricher = DependencyResolver.Current.GetService<IModelEnricher<TDestination>>();

            //If we have an enricher run and it and update the model data with it.
            if (enricher != null)
            {
                var model = (TDestination)ViewData.Model;
                ReassignModel(model, ViewData.ModelState);

                ViewData.Model = enricher.Enrich(model);
            }

            // and pass on the controller back to Microsoft code.
            base.ExecuteResult(context);
        }


        private static void ReassignModel(TDestination model, ModelStateDictionary modelState)
        {
            if (modelState.IsValid || modelState.Count == 0)
            {
                return;
            }

            var props = model.GetType().GetProperties();
            foreach (var pi in props.Where(pi => modelState.IsValidField(pi.Name)))
            {
                ModelState ms;
                if (!modelState.TryGetValue(pi.Name, out ms))
                {
                    continue;
                }

                var value = pi.GetValue(model);
                var rawValue = ms.Value.RawValue;
                if ((value is IEnumerable<int>) && (rawValue is string[]))
                {
                    var rv = (string[])rawValue;
                    var converted = rv.Select(s => Convert.ToInt32(s)).ToList();
                    pi.SetValue(model, converted);
                }
                else
                {
                    pi.SetValue(model, ms.Value.ConvertTo(pi.PropertyType));
                }
            }
        }
    }
}