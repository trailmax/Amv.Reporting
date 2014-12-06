using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AmvReporting.Domain.Backup;
using AmvReporting.Domain.Backup.Commands;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Filters;


namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public partial class BackupsController : BaseController
    {
        private readonly IMediator mediator;

        public BackupsController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        public virtual ActionResult Index()
        {
            return QueriedView(new AllBackupsQuery());
        }


        [HttpPost]
        public virtual ActionResult CreateBackup()
        {
            mediator.ProcessCommand(new CreateBackupCommand());

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

            mediator.ProcessCommand(new RestoreBackupCommand(model.Name));

            return RedirectToAction(MVC.Backups.Index());
        }
    }







    public class RestoreBackupViewModel
    {
        [Required]
        public String Name { get; set; }

        [Display(Name = "Please type 'Restore' here to confirm")]
        public String Confirmation { get; set; }
    }
}