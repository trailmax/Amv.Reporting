using System;
using System.Collections.Generic;
using System.IO;
using AmvReporting.Infrastructure.Configuration;
using AmvReporting.Infrastructure.CQRS;


namespace AmvReporting.Domain.Backup
{
    public class BackupViewModel
    {
        public String Name { get; set; }
        public String FullPath { get; set; }
    }

    public class AllBackupsQuery : IQuery<IEnumerable<BackupViewModel>>
    {
    }

    public class AllBackupsQueryHandler : IQueryHandler<AllBackupsQuery, IEnumerable<BackupViewModel>>
    {
        public IEnumerable<BackupViewModel> Handle(AllBackupsQuery query)
        {
            var backupPath = ConfigurationContext.Current.GetBackupPath();
            var folders = Directory.GetDirectories(backupPath);

            var result = new List<BackupViewModel>();
            foreach (var folder in folders)
            {
                result.Add(new BackupViewModel()
                {
                    FullPath = folder,
                    Name = new DirectoryInfo(folder).Name,
                });
            }

            return result;
        }
    }
}