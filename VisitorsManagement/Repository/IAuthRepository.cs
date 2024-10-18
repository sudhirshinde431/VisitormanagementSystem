using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VisitorsManagement.Models;

namespace VisitorsManagement.Repository
{
    public interface IAuthRepository
    {
        Task<CurrentUserDto> GetCurrentUser(string email);
        Task<CurrentUserDto> LoginUser(LoginUser loginUser);
        Task<CurrentUserDto> LoginUserActive(LoginUser loginUser);
    }
}