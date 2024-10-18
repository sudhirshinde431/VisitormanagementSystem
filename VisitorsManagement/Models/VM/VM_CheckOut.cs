using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Visitors_Management.Dto.VM
{
    public class VM_CheckOut
    {
        public int AppointmentID { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
        [JsonIgnore]
        public int UpdatedBy { get; set; }
        [JsonIgnore]
        public string UpdatedDate { get; set; }

        public string Narration { get; set; }
    }
}
