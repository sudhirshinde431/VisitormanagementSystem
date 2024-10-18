using System;
using System.Collections.Generic;
using System.Text;

namespace VisitorsManagement.Models
{
    public class LoginResult
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public CurrentUserDto CurrentUserDto { get; set; }
    }
}
