using System.Web.Mvc;

namespace AmvReporting.Infrastructure
{
    // TODO GET rid of this one
    public class RestoreModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext.Controller.TempData.ContainsKey("ModelState"))
            {
                var modelState = (ModelStateDictionary)filterContext.Controller.TempData["ModelState"];
                if (!modelState.IsValid)
                {
                    filterContext.Controller.ViewData.ModelState.Merge(modelState);
                }
            }
        }
    }
}