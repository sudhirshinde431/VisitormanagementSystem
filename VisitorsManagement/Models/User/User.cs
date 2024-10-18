using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisitorsManagement.Models
{
    public class User
    {
        public int? UserID { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(100, ErrorMessage = "First Name should not be Greater than 50 character's.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(100, ErrorMessage = "Last Name should not be Greater than 50 character's.")]
        public string LastName { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Email Id is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        [MaxLength(100, ErrorMessage = "Email Id should not be Greater than 80 character's.")]
        public string EmailID { get; set; }
        //[JsonIgnore]
        public string Password { get; set; }
        //[JsonIgnore]
        public DateTime? LastLogin { get; set; }
        //[JsonIgnore]
        public bool IsDeleted { get; set; }
        //[JsonIgnore]
        public int CreatedBy { get; set; }
        //[JsonIgnore]
        public DateTime? CreatedDate { get; set; }
        //[JsonIgnore]
        public int UpdatedBy { get; set; }
        //[JsonIgnore]
        public DateTime? UpdatedDate { get; set; }

        public List<Claims> UserClaim { get; set; }

        [JsonIgnore]
        public string applicationURL { get; set; }
        public List<UserAccess> UserAccess { get; set; }

        public bool Disable { get; set; }
    }

    public class UserAccess
    {
        public int UserAccesID { get; set; }
        public int UserID { get; set; }
        public string Claim { get; set; }
    }
}
