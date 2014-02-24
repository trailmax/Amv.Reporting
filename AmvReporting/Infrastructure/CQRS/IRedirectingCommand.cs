namespace AmvReporting.Infrastructure.CQRS
{
    public interface IRedirectingCommand : ICommand
    {
        string RedirectingId { get; set; }
    }
}