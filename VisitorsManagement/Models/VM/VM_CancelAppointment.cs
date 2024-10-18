using System;
using System.Collections.Generic;
using System.Text;

namespace Visitors_Management.Dto.VM
{
    public class VM_CancelAppointment
    {
        public int AppointmentID { get; set; }
        public string Status { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
    }
}
