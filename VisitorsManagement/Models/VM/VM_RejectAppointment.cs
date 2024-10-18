using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Visitors_Management.Dto.VM
{
    public class VM_RejectAppointment
    {
        public int AppointmentID { get; set; }
        public string Status { get; set; }
        [JsonIgnore]
        public int UpdatedBy { get; set; }
        [JsonIgnore]
        public string UpdatedDate { get; set; }
    }
}
