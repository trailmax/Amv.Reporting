using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web.Mvc;
using AmvReporting.Infrastructure.Configuration;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Embedded;


namespace AmvReporting.Controllers
{
    public partial class BackupsController : Controller
    {
        private readonly IDocumentStore documentStore;


        public BackupsController(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }


        public virtual ActionResult Index()
        {
            var backupPath = ConfigurationContext.Current.GetBackupPath();
            var folders = Directory.GetDirectories(backupPath);

            var model = new List<BackupViewModel>();
            foreach (var folder in folders)
            {
                model.Add(new BackupViewModel()
                {
                    FullPath = folder,
                    Name = new DirectoryInfo(folder).Name,
                });
            }


            return View(model);
        }


        [HttpPost]
        public virtual ActionResult CreateBackup()
        {
            var embeddedStore = documentStore as EmbeddableDocumentStore;

            if (embeddedStore == null)
            {
                throw new NotSupportedException("Document Storage is not embedded storage");
            }
            
            var path = Path.Combine(ConfigurationContext.Current.GetBackupPath(), String.Format("{0:yyyy-MM-dd-HHmm}", DateTime.Now));
            Directory.CreateDirectory(path);

            embeddedStore.DocumentDatabase.StartBackup(path, false, new DatabaseDocument());

            return RedirectToAction(MVC.Backups.Index());
        }


        public virtual ActionResult RestoreBackup(String name)
        {
            var model = new RestoreBackupViewModel()
            {
                Name = name,
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ConfirmRestoreBackup(RestoreBackupViewModel model)
        {
            if (model.Confirmation != "Restore")
            {
                ModelState.AddModelError("Confirmation", "Value does not match to 'Restore' (skip quotes)");
            }

            if (!ModelState.IsValid)
            {
                return View("RestoreBackup", model);
            }

            // do actual restore
            return RedirectToAction(MVC.Backups.Index());
        }
    }


    public class BackupViewModel
    {
        public String Name { get; set; }
        public String FullPath { get; set; }
    }


    public class RestoreBackupViewModel
    {
        [Required]
        public String Name { get; set; }

        [Display(Name = "Please type 'Restore' here to confirm")]
        public String Confirmation { get; set; }
    }
}