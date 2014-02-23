using System;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;

namespace AmvReporting.Domain.ReportingConfigs.Commands
{
    public class UpdateConfigurationCommand : ICommand
    {
        public String GlobalCss { get; set; }
        public String GlobalJavascript { get; set; }
    }

    public class UpdateConfigurationCommandHandler : ICommandHandler<UpdateConfigurationCommand>
    {
        private readonly IDocumentSession documentSession;

        public UpdateConfigurationCommandHandler(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public void Handle(UpdateConfigurationCommand command)
        {
            var config = documentSession.Load<ReportingConfig>(ReportingConfig.IdString);

            config = config ?? ReportingConfig.New();
            config.GlobalJavascript = command.GlobalJavascript;
            config.GlobalCss = command.GlobalCss;

            documentSession.Store(config);
            documentSession.SaveChanges();
        }
    }
}