using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorsManagement.Models.Contractor
{
    public class EmployeeFilter
    {
        public string FilterText { get; set; }
        public string EmployeeId { get; set; }
        public int ContractorId { get; set; }
        public int? WOId { get; set; }

        [JsonIgnore]
        public string fileType { get; set; }
        
    }
}