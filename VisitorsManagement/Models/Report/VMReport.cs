using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorsManagement.Models.Report
{
    public class VMReport
    {
        public string AppointmentNo { get; set; }

        public string GatePassNumber { get; set; }
        public string Date { get; set; }
        public string VisitorName { get; set; }
        public string VisitorPhoneNumber { get; set; }

        public string RepresentingCompany { get; set; }

        public string PersonToVisitName { get; set; }

        public string Address { get; set; }

        public string PurposeToVisit { get; set; }

        public string VisitorsEmails { get; set; }

        public int? NumberOfPerson { get; set; }

        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        
       
    }
}