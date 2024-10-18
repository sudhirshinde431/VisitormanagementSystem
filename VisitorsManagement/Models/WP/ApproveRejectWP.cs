using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorsManagement.Models.WP
{
    public class ApproveRejectWP
    {
        public int WPID { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string Approver { get; set; }

        public bool ApproveReject { get; set; }
        public int ApprovedId { get; set; }
    }
}