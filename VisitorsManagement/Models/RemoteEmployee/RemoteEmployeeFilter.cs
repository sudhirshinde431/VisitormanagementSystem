using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorsManagement.Models.RemoteEmployee
{
    public class RemoteEmployeeFilter
    {
        public string Pkey { get; set; }
        public string FilterText { get; set; }
        public string Hcode { get; set; }
        public string ForAutocomplete { get; set; }
    }
}