using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VisitorsManagement.Models;
using VisitorsManagement.Models.Contractor;

namespace VisitorsManagement.Repository
{
    public interface IContractorRepository
    {
        Task<IEnumerable<DropDownModel>> GetContractorsSelectList();
        Task<IEnumerable<ContractorMaster>> GetAllContractors(ContractorFilter filter);

        Task<int> CreateContractor(ContractorMaster contractor);

        Task<int> CreateEmployee(EmployeeDetails employee);

        Task<IEnumerable<EmployeeDetails>> GetAllEmployee(EmployeeFilter filter);

        Task<int> UploadEmployeeDetails(DataTable dt, int ContractorId);
    }
}