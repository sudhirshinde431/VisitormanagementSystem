using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisitorsManagement.Models.Contractor
{
    public class EmployeeDetails
    {
        public string SrNo { get; set; }

        public int WPID { get; set; }
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Contractor is required")]
        public int ContractorId { get; set; }
        [Required(ErrorMessage = "Employee Name is required")]
        [MaxLength(80, ErrorMessage = "Employee Name should not be Greater than 80 character's.")]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "PF Insurance Details are required")]
        [MaxLength(200, ErrorMessage = "PF Insurance Details should not be Greater than 200 character's.")]
        public string PFInsuranceDetails { get; set; }
        //[Required(ErrorMessage = "PF Insurance File is required")]
        public byte[] PFInsuranceFile { get; set; }
        public string PFInsuranceFileBase64 { get; set; }
        public string PFInsuranceFileName { get; set; }

        [Required(ErrorMessage = "ESIC Details are required")]
        [MaxLength(200, ErrorMessage = "ESIC Details should not be Greater than 200 character's.")]
        public string ESICDetails { get; set; }
        //[Required(ErrorMessage = "ESIC File is required")]
        public byte[] ESICFile { get; set; }
        public string ESICFileName { get; set; }
        public string ESICFileBase64 { get; set; }
        public string fileType { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
        //[JsonIgnore]
        public string CreatedDate { get; set; }
        //[JsonIgnore]
        public int UpdatedBy { get; set; }
        //[JsonIgnore]
        public string UpdatedDate { get; set; }
    }

    public class EmployeeUpload
    {
        public string Employee_Name { get; set; }
        public string PF_Insurance_Details { get; set; }
        public string ESIC_Details { get; set; }
    }
}
