using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorsManagement.Models.WP
{
    public class WorkPermit
    {
        public string WPID { get; set; }
        public string WPNO { get; set; }
        public string WPDate { get; set; }
        public string WPType { get; set; }
        public int InitiatedById { get; set; }
        public string InitiatedByName { get; set; }
        public string Status { get; set; }
    }
}