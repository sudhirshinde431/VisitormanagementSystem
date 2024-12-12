using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisitorsManagement.Models.RemoteEmployee
{
    public class RemoteEmployeeSecurityCheck
    {
        public string Pkey { get; set; }
        public string Hcode { get; set; }
        public string Name { get; set; }
        public string EmailID { get; set; }
        public string CheckinDateTime { get; set; }
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

        [Required(ErrorMessage = "Guest Access Card Issuee is required")]
        public string GuestAccessCardIssue { get; set; }
        public string AccessCardCollectionStatus { get; set; }
        public string Escalation { get; set; }
        public string DeafultGuestCardNumber { get; set; }
        public string CreatedBySCName { get; set; }
        public string UpdatedBySCName { get; set; }
        public string CreatedDateSC { get; set; }
        public string UpdatedDateSC { get; set; }
        public Int32 CreatedBySC { get; set; }
        public Int32 UpdatedBySC { get; set; }
        public string Re_Number { get; set; }
        public string Status { get; set; }

        public bool? IsCheckInToday { get; set; }
        public string ConcernPersonName { get; set; }
        public Int32 ConcernPersonId { get; set; }

    }
}