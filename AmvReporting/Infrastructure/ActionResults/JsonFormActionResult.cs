using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AmvReporting.Domain;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Infrastructure.ActionResults
{
    public class JsonFormActionResult<T> : ActionResult where T : ICommand
    {
        private readonly T command;
        private readonly string successMessage;
        private readonly ICommandHandler<T> handler;
        private readonly ICommandValidator<T> validator;


        public JsonFormActionResult(T command, string successMessage)
        {
            this.command = command;
            this.successMessage = successMessage;

            validator = DependencyResolver.Current.GetService<ICommandValidator<T>>();
            handler = DependencyResolver.Current.GetService<ICommandHandler<T>>();
        }


        public override void ExecuteResult(ControllerContext context)
        {
            var returnModel = new JsonReturnModel();
            var jsonResult = new JsonResult();

            if (!context.Controller.ViewData.ModelState.IsValid)
            {
                var errors = new List<String>();
                foreach (var modelState in context.Controller.ViewData.ModelState.Values)
                {
                    var fieldErrors = modelState
                        .Errors
                        .Where(error => !String.IsNullOrEmpty(error.ErrorMessage))
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    errors.AddRange(fieldErrors);

                }
                returnModel.FailureMessage += String.Join("; ", errors);
                returnModel.Success = false;
            } 
            else if (validator.IsValid(command))
            {
                try
                {
                    handler.Handle(command);
                    returnModel.Success = true;
                    returnModel.SuccessMessage = successMessage;
                }
                catch (DomainException domainException)
                {
                    returnModel.Success = false;
                    returnModel.FailureMessage = domainException.Message;
                }
                catch (Exception)
                {
                    returnModel.Success = false;
                    returnModel.FailureMessage = "Unable to process command";
                }
            }
            else
            {
                var errors = validator.Errors.ToString();
                returnModel.FailureMessage = errors;
                returnModel.Success = false;
            }

            jsonResult.Data = returnModel;
            jsonResult.ExecuteResult(context);
        }
    }


    public class JsonReturnModel
    {
        public String SuccessMessage { get; set; }
        public String FailureMessage { get; set; }
        public bool Success { get; set; }
    }
}