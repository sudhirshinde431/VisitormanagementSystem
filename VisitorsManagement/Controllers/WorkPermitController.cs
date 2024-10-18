using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VisitorsManagement.Models.WP;

namespace VisitorsManagement.Controllers
{
    public class WorkPermitController : Controller
    {
        // GET: WorkPermit
        public ActionResult WorkPermit()
        {
            return View();
        }

        public async Task<ActionResult> SaveWorkPermit(WorkPermit workPermit)
        {
            return Json("");
        }
    }
}