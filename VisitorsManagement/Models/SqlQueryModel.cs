using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisitorsManagement.Models
{
    public class SqlQueryModel
    {
        public string SqlQuery { get; set; }
        public string ReturnParameterName { get; set; }
        public int ReturnParameterValue { get; set; }
        public DynamicParameters SqlParameters { get; set; }
        public bool IsReturn { get; set; }
    }
}