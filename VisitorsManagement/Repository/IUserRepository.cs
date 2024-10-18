using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VisitorsManagement.Models;

namespace VisitorsManagement.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUser(int UserId = 0, string Email = "");
        Task<IEnumerable<DropDownModel>> GetUserSelectList(bool includeSecurity = true);
        Task<int> CreateUser(User user);

        Task<int> DeleteUser(int UserId);

        Task<IEnumerable<User>> GetUsers(UserFilter filter);

        //Task<IEnumerable<string>> GetClaims();
        Task<IEnumerable<Claims>> GetDefaultClaims();

        Task<int> ChangePassword(UserChangePassword change);

        Task<int> GetSuperAdminCount();
    }
}