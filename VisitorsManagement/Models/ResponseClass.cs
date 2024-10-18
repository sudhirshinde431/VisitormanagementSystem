using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorsManagement.Models
{
    public class ResponseClass
    {
        public bool isSuccessful { get; set; }
        public string message { get; set; }
        public int PrimaryKeyID { get; set; }

        public dynamic data { get; set; }
    }
}