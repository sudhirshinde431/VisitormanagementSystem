using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VisitorsManagement.Models;
using VisitorsManagement.Models.Report;

namespace VisitorsManagement.Repository
{
    public class VMReportRepository : IVMReportRepository
    {

        private readonly IGenericRepository _genericRepository;

        public VMReportRepository(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<IEnumerable<VMReport>> GetVisitorsReport(VMReportFilter report)
        {
            var sQuery = $@"SELECT AppointmentNo,Replace(convert(char(11),Date,106),' ', '-') as 'Date',VisitorName,VisitorPhoneNumber,RepresentingCompany,VisitorsEmails,Address,PurposeToVisit,U.FirstName + ' ' + U.LastName 'PersonToVisitName',
                            InTime,OutTime,NumberOfPerson,ISNULL(Status,'') 'Status'
                            FROM tbl_VM_Appointment VM LEFT JOIN tbl_Users U 
                            ON VM.PersonToVisitID = U.UserID WHERE CAST([DATE] AS DATE) BETWEEN CAST(@FROMDATE AS DATE) AND CAST(@TODATE AS DATE)";

            DynamicParameters param = new DynamicParameters();
            param.Add("@FROMDATE", report.FromDate);
            param.Add("@TODATE", report.ToDate);

            if (!string.IsNullOrEmpty(report.EmployeeId) && report.EmployeeId != "0")
                sQuery = sQuery + $" AND PersonToVisitID = { report.EmployeeId }";

            sQuery = sQuery + " ORDER BY AppointmentId DESC";

            var result = await _genericRepository.GetAsync<VMReport>(sQuery, param);

            return result;
        }
    }
}