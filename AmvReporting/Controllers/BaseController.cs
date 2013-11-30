using System;
using System.Web.Mvc;
using AmvReporting.Infrastructure.ActionResults;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Controllers
{
    public abstract class BaseController : Controller
    {
        protected FormActionResult<TForm> ProcessForm<TForm>(TForm form, ActionResult both) where TForm : ICommand
        {
            return new FormActionResult<TForm>(form, both);
        }

        protected FormActionResult<TForm> ProcessForm<TForm>(TForm form, ActionResult failure, ActionResult success) where TForm : ICommand
        {
            return new FormActionResult<TForm>(form, failure, success);
        }

        protected FormActionResult<TForm> ProcessForm<TForm>(TForm form, ActionResult both, Func<int, ActionResult> redirector) where TForm : ICommand
        {
            return new FormActionResult<TForm>(form, both, redirector);
        }

        protected JsonFormActionResult<T> ProcessJsonForm<T>(T command, String successMessage) where T : ICommand
        {
            return new JsonFormActionResult<T>(command, successMessage);
        }
    }
}