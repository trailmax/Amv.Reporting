using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Queries;

namespace AmvReporting.Controllers
{
    public partial class DatabaseController : BaseController
    {
        private readonly IMediator mediator;

        public DatabaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public virtual ActionResult Index()
        {
            var databases = mediator.Request(new AllDatabasesQuery());
            return View(databases);
        }

        public virtual ActionResult Create()
        {
            return View();
        }

        public virtual ActionResult Edit(object linkname)
        {
            throw new System.NotImplementedException();
        }

        public virtual ActionResult Delete(string dbid)
        {
            throw new System.NotImplementedException();
        }
    }
}