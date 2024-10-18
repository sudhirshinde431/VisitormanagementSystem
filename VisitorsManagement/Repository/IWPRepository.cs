using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using VisitorsManagement.Models;
using VisitorsManagement.Models.Contractor;
using VisitorsManagement.Models.WP;

namespace VisitorsManagement.Repository
{
    public interface IWPRepository
    {
        Task<IEnumerable<WP>> GetWP(WPFilter filter);
        Task<int> CreateWP(WP workPermit);

        Task<IEnumerable<DropDownModel>> GetWorkGroup(string permissionType);

        Task<int> ApproveRejectWP(ApproveRejectWP approveRejectWP);

        Task<EmployeeDetails> getEmployeeFile(EmployeeFilter details);

        Task<int> removeEmployee(int EmployeeId);

        Task<int> SaveSafetyTraining(SafetyTrainingModel safetyTraining);

        Task<int> CloseWorkPermit(int WPID);

        Task<DataSet> Wpreminder();
    }
}
