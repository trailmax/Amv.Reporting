namespace AmvReporting.Infrastructure.CQRS
{
    public interface IErrorMessageFormatter<in T> where T : IErrorMessage
    {
        string Format(T entity);
    }
}