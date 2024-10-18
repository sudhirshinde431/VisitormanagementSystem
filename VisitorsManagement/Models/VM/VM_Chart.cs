using System;
using System.Collections.Generic;
using System.Text;

namespace Visitors_Management.Dto.VM
{
    public class VM_Chart
    {
        //public DateTime? StartDate { get; set; }
        //public DateTime? EndDate { get; set; }

        public string type { get; set; }
    }

    public class VM_ChartResponse
    {
        public string Date { get; set; }
        public DateTime CompareDate { get; set; }
        public int OpenCnt { get; set; }
        public int CheckedInCnt { get; set; }
        public int CancelledCnt { get; set; }
        public int CheckedOutCnt { get; set; }
        public int RejectedCnt { get; set; }
    }
}
