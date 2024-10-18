using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Visitors_Management.Dto.VM;
using VisitorsManagement.Models;

namespace VisitorsManagement.Repository
{
    public interface IVisitorsManagementRepository
    {
        Task<IEnumerable<VM>> GetVisitors(VMFilter filter);
        Task<VM> GetVisitorByWPID(int WPID);
        Task<int> RaiseAppointment(VM visitor);

        Task<int> CheckInAppointment(VM_CheckIn visitor);

        Task<int> CheckOutAppointment(VM_CheckOut visitor);

        Task<int> CancelAppointment(VM_CancelAppointment visitor);

        Task<int> RejectAppointment(VM_RejectAppointment visitor);

        Task<int> ApproveRejectAppointment(VM_ApproveRejectAppointment visitor);

        Task<Tuple<string, string>> GetVisitingPersonEmail(int userId);

        Task<IEnumerable<VM_ChartResponse>> GenerateChart(VM_Chart filter);

        Task<int> ClosePreviousAppointments();
    }
}
