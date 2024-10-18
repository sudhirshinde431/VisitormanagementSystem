using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisitorsManagement.Models.Contractor
{
    public class ContractorMaster
    {
        public int ContractorId { get; set; }

        [Required(ErrorMessage = "Contractor Name is required")]
        [MaxLength(200, ErrorMessage = "Contractor Name should not be Greater than 200 character's.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "License Details are required")]
        [MaxLength(500, ErrorMessage = "License Details should not be Greater than 500 character's.")]
        public string LicenseDetails { get; set; }
        public string IssuiedOn { get; set; }
        public string ValidTill { get; set; }
        [Required(ErrorMessage = "Contact Person Name is required")]
        [MaxLength(80, ErrorMessage = "Contact Person Name should not be Greater than 80 character's.")]
        public string ContactPersonName { get; set; }
        [Required(ErrorMessage = "Contact Person Mobile No. is required")]
        [MaxLength(10, ErrorMessage = "Contact Person Mobile No. should not be Greater than 10 character's.")]
        public string ContactPersonMobileNo { get; set; }

        [Required(ErrorMessage = "Contractor's Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        public string EmailId { get; set; }
        public bool? IsActive { get; set; }
        public int CreatedBy { get; set; }
        //[JsonIgnore]
        public string CreatedDate { get; set; }
        //[JsonIgnore]
        public int UpdatedBy { get; set; }
        //[JsonIgnore]
        public string UpdatedDate { get; set; }
        //public List<EmployeeDetails> EmployeeList { get; set; }
    }
}