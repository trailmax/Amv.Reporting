using System;
using System.Web.Mvc;
using AmvReporting.Infrastructure.ActionResults;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Controllers
{
    public abstract partial class BaseController : Controller
    {
        protected FormActionResult<TForm> ProcessForm<TForm>(TForm form, ActionResult both) where TForm : ICommand
        {
            return new FormActionResult<TForm>(form, both);
        }
        protected FormActionResult<TForm> ProcessForm<TForm>(TForm form, ActionResult failure, ActionResult success) where TForm : ICommand
        {
            return new FormActionResult<TForm>(form, failure, success);
        }
        protected FormActionResult<TForm> ProcessForm<TForm>(TForm form, ActionResult both, Func<string, ActionResult> redirector) where TForm : ICommand
        {
            return new FormActionResult<TForm>(form, both, redirector);
        }


        protected JsonFormActionResult<T> ProcessJsonForm<T>(T command, String successMessage) where T : ICommand
        {
            return new JsonFormActionResult<T>(command, successMessage);
        }


        public EnrichViewResult<T> EnrichedView<T>(string viewName, T model) where T : class
        {
            AssignModel(model);
            return new EnrichViewResult<T>(viewName, ViewData, TempData);
        }
        public EnrichViewResult<T> EnrichedView<T>(T model) where T : class
        {
            AssignModel(model);
            return new EnrichViewResult<T>(ViewData, TempData);
        }


        public AutoMapViewResult<T> AutoMappedView<T>(string viewName, object model) where T : class
        {
            AssignModel(model);
            return new AutoMapViewResult<T>(viewName, ViewData, TempData);
        }

        public AutoMapViewResult<T> AutoMappedView<T>(object model) where T : class
        {
            AssignModel(model);
            return new AutoMapViewResult<T>(ViewData, TempData);
        }


        private void AssignModel(object model)
        {
            if (model != null)
            {
                ViewData.Model = model;
            }
        }
    }
}