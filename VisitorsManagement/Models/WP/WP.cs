using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VisitorsManagement.Models.Contractor;

namespace VisitorsManagement.Models.WP
{
    public class WP
    {

        //public int? WPID { get; set; }

        //public string WPNO { get; set; }

        //[Required(ErrorMessage = "Work Permit Date is required")]
        //public string WPDate { get; set; }

        //[Required(ErrorMessage = "Work Permit Type is required")]
        //public string WPType { get; set; }

        //[Required(ErrorMessage = "Contractor is required")]
        //public int ContractorId { get; set; }
        //public string SelectedEmployees { get; set; }

        //[Required(ErrorMessage = "Work Permit Start Date is required")]
        //public string WorkStartDate { get; set; }

        //[Required(ErrorMessage = "Work End Date is required")]
        //public string WorkEndDate { get; set; }
        //public int? InitiatedById { get; set; }

        //[Required(ErrorMessage = "Nature of Work is required")]
        //[MaxLength(600, ErrorMessage = "Nature of Work should not be Greater than 600 character's.")]
        //public string NatureOfWork { get; set; }

        //[Required(ErrorMessage = "Safety Training is required")]
        //public string Status { get; set; }
        //public bool IsSubmitted { get; set; }

        //[Required(ErrorMessage = "HR Approver is required")]
        //public int HRId { get; set; }

        //[Required(ErrorMessage = "IMS Approver is required")]
        //public int IMSId { get; set; }

        //[Required(ErrorMessage = "Final Approver is required")]
        //public int FinalId { get; set; }

        //public int? CreatedBy { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public int? UpdatedBy { get; set; }
        //public DateTime? UpdatedDate { get; set; }

        //public string LicenseDetails { get; set; }

        //public string InitiatedByName { get; set; }




        public int? WPID { get; set; }

        public string WPNO { get; set; }

        [Required(ErrorMessage = "Work Permit Date is required")]
        //[MaxLength(100, ErrorMessage = "Visitor's Name should not be Greater than 50 character's.")]
        public string WPDate { get; set; }
        public string strWPDate { get; set; }

        [Required(ErrorMessage = "Work Permit Type is required")]
        //[MaxLength(100, ErrorMessage = "Visitor's Name should not be Greater than 50 character's.")]
        public string WPType { get; set; }

        //[Required(ErrorMessage = "Unit is required")]
        //[MaxLength(200, ErrorMessage = "Unit should not be Greater than 200 character's.")]
        public string Unit { get; set; }

        //[Required(ErrorMessage = "Work Location is required")]
        //[MaxLength(100, ErrorMessage = "Work Location should not be Greater than 100 character's.")]
        public string WorkLocation { get; set; }

        [Required(ErrorMessage = "Contractor is required")]
        //[MaxLength(100, ErrorMessage = "Visitor's Name should not be Greater than 50 character's.")]
        public int ContractorId { get; set; }
        public string ContractorName { get; set; }
        public string LicenseDetails { get; set; }
        public string ContractorEmailId { get; set; }
        public string SelectedEmployees { get; set; }

        [Required(ErrorMessage = "Work Permit Start Date is required")]
        //[MaxLength(100, ErrorMessage = "Visitor's Name should not be Greater than 50 character's.")]
        public string WorkStartDate { get; set; }
        public string strWorkStartDate { get; set; }

        [Required(ErrorMessage = "Work End Date is required")]
        //[MaxLength(100, ErrorMessage = "Visitor's Name should not be Greater than 50 character's.")]
        public string WorkEndDate { get; set; }
        public string strWorkEndDate { get; set; }
        public int? InitiatedById { get; set; }
        public string InitiatedByName { get; set; }

        [Required(ErrorMessage = "Nature of Work is required")]
        [MaxLength(600, ErrorMessage = "Nature of Work should not be Greater than 600 character's.")]
        public string NatureOfWork { get; set; }

        //[Required(ErrorMessage = "Safety Training is required")]
        public string SafetyTraining { get; set; }

        //[Required(ErrorMessage = "Trained By is required")]
        //[MaxLength(100, ErrorMessage = "Nature of Work should not be Greater than 100 character's.")]
        //public string TrainedBy { get; set; }

        //[Required(ErrorMessage = "Trained Date is required")]
        public string TrainedDate { get; set; }

        //public string strTrainedDate { get; set; }

        public string Status { get; set; }
        public string StatusNew { get; set; }
        //[Required(ErrorMessage = "Employee Details are required")]
        public List<EmployeeDetails> listEmployee { get; set; }

        public bool IsSubmitted { get; set; }

        //[JsonIgnore]
        public bool? HRApproval { get; set; }
        //[JsonIgnore]
        public string HRComment { get; set; }

        [Required(ErrorMessage = "HR Approver is required")]
        public int HRId { get; set; }
        public string HRApproverName { get; set; }

        //[JsonIgnore]
        //public string HRApprovedDate { get; set; }


        //[JsonIgnore]
        public bool? IMSApproval { get; set; }
        //[JsonIgnore]
        public string IMSComment { get; set; }
        [Required(ErrorMessage = "IMS Approver is required")]
        public int IMSId { get; set; }
        public string IMSApproverName { get; set; }
        //[JsonIgnore]
        //public string IMSApprovedDate { get; set; }


        //[JsonIgnore]
        public bool? FinalApproval { get; set; }
        //[JsonIgnore]
        public string FinalComment { get; set; }

        [Required(ErrorMessage = "Manager Approver is required")]
        public int FinalId { get; set; }
        public string FinalApproverName { get; set; }
        //[JsonIgnore]
        //public string FinalApprovedDate { get; set; }

        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public string CreatedDate { get; set; }
        [JsonIgnore]
        public string UpdatedBy { get; set; }
        [JsonIgnore]
        public string UpdatedDate { get; set; }


        //public int? PermissionID { get; set; }

        ////public int UserID { get; set; }

        //public string PermissionType { get; set; }

        //public int WorkGroup { get; set; }

        //public string WorkGroupName { get; set; }

        //public string DocumentNo { get; set; }

        //public string RevisionNo { get; set; }

        //public DateTime PermissionDate { get; set; }

        //public DateTime ValidityDate { get; set; }

        //public string strPermissionDate { get; set; }

        //public string strValidityDate { get; set; }

        //public string Label { get; set; }

        //public string Description { get; set; }

        //public byte[] File { get; set; }

        //public DateTime? PublishDate { get; set; }

        //public int Status { get; set; }

        //public int PreparedBy { get; set; }
        ///public string PreparedByName { get; set; }
        ///
    }

}
