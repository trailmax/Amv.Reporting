using System;
using System.Web.Mvc;
using AmvReporting.Domain;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Infrastructure.ActionResults
{
    public class FormActionResult<T> : ActionResult where T : ICommand
    {
        private readonly T form;
        private readonly ActionResult failure;
        private readonly ICommandHandler<T> handler;
        private readonly ICommandValidator<T> validator;
        private readonly Func<int, ActionResult> redirector;
        private ActionResult success;


        public FormActionResult(T form, ActionResult failure, ActionResult success)
        {
            this.form = form;
            this.success = success;
            this.failure = failure;

            validator = DependencyResolver.Current.GetService<ICommandValidator<T>>();
            handler = DependencyResolver.Current.GetService<ICommandHandler<T>>();
        }



        public FormActionResult(T form, ActionResult both)
        {
            this.form = form;
            success = both;
            failure = both;

            handler = DependencyResolver.Current.GetService<ICommandHandler<T>>();
            validator = DependencyResolver.Current.GetService<ICommandValidator<T>>();
        }


        public FormActionResult(T form, ActionResult failure, Func<int, ActionResult> redirector)
        {
            this.form = form;
            this.failure = failure;
            this.redirector = redirector;

            validator = DependencyResolver.Current.GetService<ICommandValidator<T>>();
            handler = DependencyResolver.Current.GetService<ICommandHandler<T>>();
        }


        public override void ExecuteResult(ControllerContext context)
        {
            if (success == null && redirector == null)
            {
                throw new DomainException("Can not process form. Please supply either success action or redirection action");
            }

            //Validator never null as we are always providing NullObject validator
            if (!validator.IsValid(form))
            {
                foreach (var error in validator.Errors)
                {
                    var errorType = error.GetType();
                    var formatterType = typeof(IErrorMessageFormatter<>).MakeGenericType(errorType);
                    var formatter = DependencyResolver.Current.GetService(formatterType);
                    if (formatter != null)
                    {
                        var method = formatterType.GetMethod("Format");
                        var result = method.Invoke(formatter, new object[] { error });

                        context.Controller.ViewData.ModelState.AddModelError(error.FieldName, (String)result);
                    }
                }
            }

            // validation is failed
            if (!context.Controller.ViewData.ModelState.IsValid)
            {
                context.Controller.TempData["ModelState"] = context.Controller.ViewData.ModelState;
                failure.ExecuteResult(context);

                return;
            }

            // if we got here, validation if fine and we go ahead with success route
            handler.Handle(form);


            var redirectingCommand = form as IRedirectingCommand;
            if (redirector != null && redirectingCommand != null)
            {
                success = redirector.Invoke(redirectingCommand.RedirectingId);
            }

            success.ExecuteResult(context);
        }
    }
}