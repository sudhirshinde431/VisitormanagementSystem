using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorsManagement.Models.Report
{
    public class VMReportFilter
    {
        public string FromDate { get; set; }
        
        public string ToDate { get; set; }

        public string EmployeeId { get; set; }

        public string FileName { get; set; }
    }
}