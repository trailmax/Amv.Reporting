namespace AmvReporting.Infrastructure.CQRS
{
    public interface IRedirectingCommand : ICommand
    {
        int RedirectingId { get; set; }
    }
}