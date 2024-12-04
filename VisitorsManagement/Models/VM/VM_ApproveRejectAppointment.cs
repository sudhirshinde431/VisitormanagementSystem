using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;
//using System.Text.Json.Serialization;

namespace VisitorsManagement.Models
{
    public class VM_ApproveRejectAppointment
    {
        public int AppointmentID { get; set; }
        public string Status { get; set; }
        [JsonIgnore]
        public int UpdatedBy { get; set; }
        [JsonIgnore]
        public DateTime UpdatedDate { get; set; }

        public Boolean IsDirectApproved { get; set; }
    }
}
