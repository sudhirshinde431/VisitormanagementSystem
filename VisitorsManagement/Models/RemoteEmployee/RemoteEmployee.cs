using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VisitorsManagement.Models.Contractor;

namespace VisitorsManagement.Models.RemoteEmployee
{
    public class RemoteEmployee
    {
        public string Pkey { get; set; }

        [Required(ErrorMessage = "H Code is required")]
        public string Hcode { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Check In date is required")]
        public string CheckinDateTime { get; set; }

        [Required(ErrorMessage = "Check Out date is required")]
        public string CheckOutDateTime { get; set; }
        public string IsVehicalParkedOnPremises { get; set; }
        public string VehicalNumber { get; set; }
        public string Comments { get; set; }
        public Int32 CreatedBy { get; set; }
        public Int32 UpdatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string SecurityCheckDone { get; set; }
        public string GuestAccessCardIssue { get; set; }
        public string BookForWeekend { get; set; }
        
        public string AccessCardCollectionStatus { get; set; }
        public string Escalation { get; set; }
        public string DeafultGuestCardNumber { get; set; }
        public string CreatedBySCName { get; set; }
        public string UpdatedBySCName { get; set; }
        public string CreatedDateSC { get; set; }
        public string UpdatedDateSC { get; set; }
        public Int32 CreatedBySC { get; set; }
        public Int32 UpdatedBySC { get; set; }
        public string SecurityCheckDoneOn { get; set; }
        public string SecurityCheckDoneBy { get; set; }
        public string Status { get; set; }
        public string Re_Number { get; set; }
        public bool? IsCheckInToday { get; set; }

        public string ConcernPersonName { get; set; }
        public Int32 ConcernPersonId { get; set; }
        public string RoleName { get; set; }

        
        public List<VisitorsManagement.Models.RemoteEmployee.RemoteEmployee> remoteEmployee { get; set; }
    }



}