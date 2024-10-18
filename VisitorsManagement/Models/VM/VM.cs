using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Text.Json.Serialization;

namespace VisitorsManagement.Models

{
    public class VM 
    {
        public int? AppointmentID { get; set; }
        public string GatePassNumber { get; set; }
        public string BatchNumber { get; set; }

        public string AppointmentNo { get; set; }
        [Required(ErrorMessage = "Visitor's Name is required")]
        [MaxLength(100, ErrorMessage = "Visitor's Name should not be Greater than 80 character's.")]
        public string VisitorName { get; set; }
        [Required(ErrorMessage = "Visitor's Phone No is required")]
        [MaxLength(100, ErrorMessage = "Visitor's Phone No should not be Greater than 50 character's.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string VisitorPhoneNumber { get; set; }

        [Required(ErrorMessage = "Representing Company is required")]
        [MaxLength(100, ErrorMessage = "Representing Company should not be Greater than 100 character's.")]
        public string RepresentingCompany { get; set; }

        [Required(ErrorMessage = "Person To Visit is required")]
        public int PersonToVisitID { get; set; }
        //[JsonIgnore]
        public string PersonToVisitName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(100, ErrorMessage = "Address should not be Greater than 100 character's.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Purpose to Visit is required")]
        public string PurposeToVisit { get; set; }

       // [Required(ErrorMessage = "Visitor's Email is required")]
        //[RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Invalid Email Id")]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        public string VisitorsEmails { get; set; }

        [Required(ErrorMessage = "Number Of Person's is required")]
        public int? NumberOfPerson { get; set; }
        public string Remark { get; set; }
        //public string VehicleDetails { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public string Date { get; set; }
        public string strDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string Status { get; set; }

        public string PersonToVisitEmailID { get; set; }
        //public int InOutID { get; set; }
        //public bool IsDeleted { get; set; }
        [JsonIgnore]
        public bool IsCheckInToday { get; set; }

        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public string CreatedDate { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        [JsonIgnore]
        public string UpdatedDate { get; set; }
        public int? TotalCount { get; set; }

        public string DateInGlobalFormate { get; set; }
    }
}
