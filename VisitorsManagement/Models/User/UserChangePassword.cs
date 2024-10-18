using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisitorsManagement.Models
{
    public class UserChangePassword
    {
        [JsonIgnore]
        public int UserId { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string NewPassword { get; set; }
    }
}