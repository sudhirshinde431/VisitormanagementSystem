using System;
using System.Collections.Generic;
using System.Text;

namespace VisitorsManagement.Models
{
    public class Claims
    {
        public string PageName { get; set; }

        public string PageNameShort { get; set; }
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }

        //public bool CanRead { get; set; }
    }
}
