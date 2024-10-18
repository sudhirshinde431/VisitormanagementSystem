using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VisitorsManagement.App_Start;
using VisitorsManagement.Models;
using VisitorsManagement.Models.Report;
using VisitorsManagement.Repository;

namespace VisitorsManagement.Controllers
{
    [ExceptionHandler]
    [BasicAuthentication]
    public class ReportController : Controller
    {

        private readonly IVMReportRepository _VMReportRepository;

        public ReportController(IVMReportRepository VMReportRepository)
        {
            _VMReportRepository = VMReportRepository;
        }
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public void DownloadVMReport(VMReportFilter report)
        {

            DataTable dt = new DataTable();

            var result = Task.Run(() => _VMReportRepository.GetVisitorsReport(report)).Result;

            // dt = DB.ToDataTable<VM>(result.ToList());
            ListToDataTableConverter convertor = new ListToDataTableConverter();
            dt = convertor.ToDataTable(result.ToList());
            string str = DB.ExportToExcel(dt, report.FileName);
        }
    }
}