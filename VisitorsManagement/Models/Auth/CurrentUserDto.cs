using System;
using System.Collections.Generic;
using System.Text;

namespace VisitorsManagement.Models
{
   public class CurrentUserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string EmailID { get; set; }
        public string RoleName { get; set; }

        public bool IsAdmin { get; set; }

        public IEnumerable<string> Claims { get; set; }

    }
}
