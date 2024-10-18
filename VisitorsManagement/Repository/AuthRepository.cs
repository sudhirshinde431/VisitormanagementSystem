using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VisitorsManagement.Models;

namespace VisitorsManagement.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IGenericRepository _genericRepository;

        public AuthRepository(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<CurrentUserDto> GetCurrentUser(string email)
        {
            var sQuery = $@"SELECT UserID,FirstName,LastName,FirstName + ' ' +LastName as 'FullName',EmailID,Role as 'RoleName' FROM tbl_Users WHERE 
                            EmailId=@EmailId";

            var parameters = new DynamicParameters();
            parameters.Add("@EmailId", email);

            CurrentUserDto currentUserDto = new CurrentUserDto();

            var result = await _genericRepository.GetAsync<CurrentUserDto>(sQuery, parameters);

            currentUserDto = result.FirstOrDefault();

            List<string> Claims = new List<string>();

            sQuery = @"SELECT Claim FROM tbl_UserAccess WHERE UserId = @UserId";

            parameters = new DynamicParameters();
            parameters.Add("@UserId", currentUserDto.UserId);

            var resultClaims = await _genericRepository.GetAsync<string>(sQuery, parameters);
            currentUserDto.Claims = resultClaims;
            return currentUserDto;
        }

        public async Task<CurrentUserDto> LoginUser(LoginUser loginUser)
        {
            try
            {
                //var Test = DB.Decrypt("HZNFvxbLPVcXCDUaV46DCLMscjYEOo4SSb/0i3iUW8g=");
                var sQuery = $@"SELECT UserId,FirstName,LastName,FirstName + ' ' +LastName as 'FullName',EmailID,Role as 'RoleName' FROM tbl_Users WHERE 
                            EmailId='{ loginUser.email }' AND Disable<>1 AND Password='{DB.encrypt(loginUser.password)}' COLLATE SQL_Latin1_General_CP1_CS_AS";

                var parameters = new DynamicParameters();
                //parameters.Add("@EmailId", loginUser.email);
                //parameters.Add("@Password", DB.encrypt(loginUser.password));

                CurrentUserDto currentUserDto = new CurrentUserDto();

                var result = await _genericRepository.GetAsync<CurrentUserDto>(sQuery, null);

                if (result != null && result.Count() > 0)
                {
                    currentUserDto = result.FirstOrDefault();

                    List<string> Claims = new List<string>();

                    sQuery = @"SELECT Claim FROM tbl_UserAccess WHERE UserId = @UserId";

                    parameters = new DynamicParameters();
                    parameters.Add("@UserId", currentUserDto.UserId);

                    var resultClaims = await _genericRepository.GetAsync<string>(sQuery, parameters);
                    currentUserDto.Claims = resultClaims;
                    return currentUserDto;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public async Task<CurrentUserDto> LoginUserActive(LoginUser loginUser)
        {
            try
            {
                //var Test = DB.Decrypt("HZNFvxbLPVcXCDUaV46DCLMscjYEOo4SSb/0i3iUW8g=");
                var sQuery = $@"SELECT UserId,FirstName,LastName,FirstName + ' ' +LastName as 'FullName',EmailID,Role as 'RoleName' FROM tbl_Users WHERE 
                            EmailId='{ loginUser.email }' AND Disable=1 AND Password='{DB.encrypt(loginUser.password)}' COLLATE SQL_Latin1_General_CP1_CS_AS";

                var parameters = new DynamicParameters();
                //parameters.Add("@EmailId", loginUser.email);
                //parameters.Add("@Password", DB.encrypt(loginUser.password));

                CurrentUserDto currentUserDto = new CurrentUserDto();

                var result = await _genericRepository.GetAsync<CurrentUserDto>(sQuery, null);

                if (result != null && result.Count() > 0)
                {
                    currentUserDto = result.FirstOrDefault();

                    List<string> Claims = new List<string>();

                    sQuery = @"SELECT Claim FROM tbl_UserAccess WHERE UserId = @UserId";

                    parameters = new DynamicParameters();
                    parameters.Add("@UserId", currentUserDto.UserId);

                    var resultClaims = await _genericRepository.GetAsync<string>(sQuery, parameters);
                    currentUserDto.Claims = resultClaims;
                    return currentUserDto;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {

                throw;
            }


        }


    }
}