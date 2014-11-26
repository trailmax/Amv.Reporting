using System.Web.Mvc;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Infrastructure.ActionResults
{
    public class ProcessCommandResult<T> : ActionResult where T : ICommand
    {
        private readonly T command;
        private readonly ActionResult failure;
        private readonly ActionResult success;
        private readonly IMediator mediator;


        public ProcessCommandResult(T command, ActionResult failure, ActionResult success)
        {
            this.command = command;
            this.success = success;
            this.failure = failure;

            mediator = DependencyResolver.Current.GetService<IMediator>();
        }



        public override void ExecuteResult(ControllerContext context)
        {
            if (!context.Controller.ViewData.ModelState.IsValid)
            {
                failure.ExecuteResult(context);
                return;
            }


            ErrorList handlingResult = mediator.ProcessCommand(command);

            if (!handlingResult.IsValid())
            {
                foreach (var error in handlingResult)
                {
                    context.Controller.ViewData.ModelState.AddModelError(error.FieldName, error.ToString());
                }
            }

            // validation is failed
            if (!context.Controller.ViewData.ModelState.IsValid)
            {
                //context.Controller.TempData["ModelState"] = context.Controller.ViewData.ModelState;
                failure.ExecuteResult(context);
                return;
            }

            success.ExecuteResult(context);
        }
    }
}