using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VisitorsManagement.Models;
using VisitorsManagement.Models.Report;

namespace VisitorsManagement.Repository
{
    public interface IVMReportRepository
    {
        Task<IEnumerable<VMReport>> GetVisitorsReport(VMReportFilter report);
    }
}