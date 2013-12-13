using Autofac;

namespace AmvReporting.Infrastructure.CQRS
{
    public interface IMediator
    {
        TResponse Request<TResponse>(IQuery<TResponse> query);
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
    }
}