using System;
using System.IO;
using System.Threading;
using AmvReporting.Infrastructure.Configuration;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database;


namespace AmvReporting.Domain.Backup.Commands
{
    public class RestoreBackupCommand : ICommand
    {
        public String Name { get; private set; }

        public RestoreBackupCommand(string name)
        {
            Name = name;
        }
    }


    public class RestoreBackupCommandHandler : ICommandHandler<RestoreBackupCommand>
    {
        private readonly IDocumentStore documentStore;


        public RestoreBackupCommandHandler(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }


        public void Handle(RestoreBackupCommand command)
        {
            var embeddedStore = documentStore as EmbeddableDocumentStore;

            if (embeddedStore == null)
            {
                throw new NotSupportedException("Document Storage is not embedded storage");
            }

            var configuration = embeddedStore.Configuration;

            var backupLocation = Path.Combine(ConfigurationContext.Current.GetBackupPath(), command.Name);
            var databaseLocation = ConfigurationContext.Current.GetRavenDataPath();

            documentStore.Dispose();

            Thread.Sleep(5000);

            EmptyDirectory(new DirectoryInfo(databaseLocation));

            DocumentDatabase.Restore(configuration, backupLocation, databaseLocation, s => { }, defrag: true);

        }


        private static void EmptyDirectory(DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }
            foreach (var subDirectory in directory.GetDirectories())
            {
                subDirectory.Delete(true);
            }
        }
    }
}