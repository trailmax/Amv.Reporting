namespace AmvReporting.Infrastructure.CQRS
{
    public class StringErrorMessageFormatter : IErrorMessageFormatter<StringErrorMessage>
    {
        public string Format(StringErrorMessage entity)
        {
            return entity.Message;
        }
    }
}