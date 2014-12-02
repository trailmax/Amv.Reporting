using System;
using System.IO;
using AmvReporting.Infrastructure.Configuration;
using AmvReporting.Infrastructure.CQRS;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Embedded;


namespace AmvReporting.Domain.Backup.Commands
{
    public class CreateBackupCommand : ICommand
    {
    }

    public class CreateBackupCommandHandler : ICommandHandler<CreateBackupCommand>
    {
        private readonly IDocumentStore documentStore;


        public CreateBackupCommandHandler(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }


        public void Handle(CreateBackupCommand command)
        {
            var embeddedStore = documentStore as EmbeddableDocumentStore;

            if (embeddedStore == null)
            {
                throw new NotSupportedException("Document Storage is not embedded storage");
            }

            var path = Path.Combine(ConfigurationContext.Current.GetBackupPath(), String.Format("{0:yyyy-MM-dd-HHmm}", DateTime.Now));
            Directory.CreateDirectory(path);

            embeddedStore.DocumentDatabase.StartBackup(path, false, new DatabaseDocument());
        }
    }
}