using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Visitors_Management.Dto.VM
{
    public class VM_CheckIn
    {
        public int AppointmentID { get; set; }
        [Required(ErrorMessage = "Number of person is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of person is required")]
        public int NumberOfPerson { get; set; }
        public string Time { get; set; }
        public string GatePassNumber { get; set; }
        public string MaterialDetails { get; set; }
        public string VehicleDetails { get; set; }
        public string Status { get; set; }
        [JsonIgnore]
        public int UpdatedBy { get; set; }
        [JsonIgnore]
        public DateTime UpdatedDate { get; set; }
    }
}
