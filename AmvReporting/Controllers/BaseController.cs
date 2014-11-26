using System;
using System.Web.Mvc;
using AmvReporting.Infrastructure.ActionResults;
using AmvReporting.Infrastructure.CQRS;
using AutoMapper;


namespace AmvReporting.Controllers
{
    public abstract partial class BaseController : Controller
    {
        //protected ProcessCommandResult<TForm> ProcessCommand<TForm>(TForm form, ActionResult both) where TForm : ICommand
        //{
        //    return new ProcessCommandResult<TForm>(form, both);
        //}
        protected ProcessCommandResult<TForm> ProcessCommand<TForm>(TForm form, ActionResult failure, ActionResult success) where TForm : ICommand
        {
            return new ProcessCommandResult<TForm>(form, failure, success);
        }
        //protected ProcessCommandResult<TForm> ProcessCommand<TForm>(TForm form, ActionResult both, Func<Guid, ActionResult> redirector) where TForm : ICommand
        //{
        //    return new ProcessCommandResult<TForm>(form, both, redirector);
        //}


        protected JsonFormActionResult<T> ProcessJsonForm<T>(T command, String successMessage) where T : ICommand
        {
            return new JsonFormActionResult<T>(command, successMessage);
        }


        public ActionResult MappedView<T>(object model) where T : class
        {
            var mappeddata = Mapper.Map<T>(model);

            return View(mappeddata);
        }


        /// <summary>
        /// Add errors from ErrorList to ModelState
        /// </summary>
        /// <param name="errors"></param>
        protected void AddErrorsToModelState(ErrorList errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.FieldName, error.ToString());
            }
        }


        protected QueryResult<TResult> View<TResult>(IQuery<TResult> query)
        {
            return new QueryResult<TResult>(query);
        }
    }
}