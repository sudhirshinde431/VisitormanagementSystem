using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorsManagement.Models;
using VisitorsManagement.Models.RemoteEmployee;

namespace VisitorsManagement.Repository
{
    public interface IRemoteEmployee
    {
        Task<IEnumerable<VisitorsManagement.Models.RemoteEmployee.RemoteEmployee>>
            getRemoteEmployee(VisitorsManagement.Models.RemoteEmployee.RemoteEmployeeFilter filter);

        Task<int> SaveRemoteEmployee(RemoteEmployee remoteEmployee);


        Task<int> SaveSecurityCheck(RemoteEmployeeSecurityCheck RemoteEmployeeSecurityCheck);

        Task<int> GetTodayVMCount();
        Task<int> GetTodayWPCount();
        Task<int> GetTodayRECount();

        Task<int> CancelRemoteEmployee(RemoteEmployee Re);
        
    }
}
