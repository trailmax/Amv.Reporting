using Autofac;

namespace AmvReporting.Infrastructure.CQRS
{
    public interface IMediator
    {
        TResponse Request<TResponse>(IQuery<TResponse> query);
        ErrorList ProcessCommand<TCommand>(TCommand command) where TCommand : ICommand;
    }

    public class AutofacMediator : IMediator
    {
        readonly ILifetimeScope container;

        public AutofacMediator(ILifetimeScope container)
        {
            this.container = container;
        }

        public virtual TResponseData Request<TResponseData>(IQuery<TResponseData> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponseData));
            var handler = container.Resolve(handlerType);
            var result = (TResponseData)handler.GetType().GetMethod("Handle").Invoke(handler, new object[] { query });
            return result;
        }


        public ErrorList ProcessCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            var validator = container.Resolve<ICommandValidator<TCommand>>();

            var isValid = validator.IsValid(command);
            if (!isValid)
            {
                return validator.Errors;
            }

            var handler = container.Resolve<ICommandHandler<TCommand>>();

            handler.Handle(command);

            return new ErrorList();
        }
    }
}