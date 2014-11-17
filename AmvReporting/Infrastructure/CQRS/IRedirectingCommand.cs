using System;


namespace AmvReporting.Infrastructure.CQRS
{
    public interface IRedirectingCommand : ICommand
    {
        Guid RedirectingId { get; set; }
    }
}