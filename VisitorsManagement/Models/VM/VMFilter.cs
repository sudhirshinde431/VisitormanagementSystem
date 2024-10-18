using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Visitors_Management.Dto.VM
{
    public class VMFilter
    {
        public string FilterText { get; set; }
        public int AppointmentId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        public string ForAutocomplete { get; set; }
    }
}
