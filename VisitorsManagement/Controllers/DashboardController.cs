using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VisitorsManagement.App_Start;
using VisitorsManagement.Repository;

namespace VisitorsManagement.Controllers
{
    [BasicAuthentication]
    public class DashboardController : Controller
    {
        private readonly IRemoteEmployee _IRemoteEmployee;
        public DashboardController(IRemoteEmployee remoteEmployee, IVisitorsManagementRepository visitorsManagementRepository,
            IContractorRepository contractorRepository)
        {
            _IRemoteEmployee = remoteEmployee;

        }
        public async Task<ActionResult> Index()
        {
            ViewBag.GetTodayVMCount = await _IRemoteEmployee.GetTodayVMCount();
            ViewBag.GetTodayWPCount = await _IRemoteEmployee.GetTodayWPCount();
            ViewBag.GetTodayRECount = await _IRemoteEmployee.GetTodayRECount();
            return View();
        }
    }
}