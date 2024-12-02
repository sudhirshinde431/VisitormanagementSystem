using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VisitorsManagement.Models;

namespace VisitorsManagement.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IGenericRepository _genericRepository;
        public UserRepository(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<IEnumerable<DropDownModel>> GetUserSelectList(bool includeSecurity = true)
        {
            var sQuery = $@"SELECT FirstName + ' ' + LastName as 'Text' , UserID as 'ValueInt' FROM tbl_Users";

            if (!includeSecurity)
                sQuery = sQuery + " WHERE Role != 'Security'";

            var result = await _genericRepository.GetAsync<DropDownModel>(sQuery, null);

            return result;
        }

        public async Task<IEnumerable<User>> GetUser(int UserId = 0, string Email = "")
        {
            var sQuery = $@"SELECT FirstName + ' ' + LastName 'userName',* FROM TBL_USERS";

            if (UserId > 0)
                sQuery = sQuery + $" WHERE UserId = { UserId }";
            if (!string.IsNullOrEmpty(Email))
            {
                if (sQuery.Contains("WHERE"))
                    sQuery = sQuery + $" AND EmailId = '{ Email }'";
                else
                    sQuery = sQuery + $" WHERE EmailId = '{ Email }'";
            }

            var result = await _genericRepository.GetAsync<User>(sQuery, null);

            return result;
        }

        public async Task<int> CreateUser(User user)
        {

            IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString); conn.Open();

            using (var transction = conn.BeginTransaction())
            {
                var userId = 0;
                var index = 0;
                try
                {
                    var sQuery = @"";

                    if (user.UserID > 0)
                    {
                        sQuery = @"UPDATE [dbo].[tbl_Users]
                           SET [FirstName] = @FirstName
                              ,[LastName] = @LastName
                              ,[MobileNo] = @MobileNo
                              ,[EmailID] = @EmailID
                              ,[Role] = @Role
                              ,[Disable] = @Disable
                              ,[UpdatedBy] = @UpdatedBy
                              ,[UpdatedDate] = @UpdatedDate
                         WHERE UserID = @UserID";

                        DynamicParameters param = new DynamicParameters();
                        param.Add("@FirstName", user.FirstName);
                        param.Add("@LastName", user.LastName);
                        param.Add("@MobileNo", user.MobileNo);
                        param.Add("@EmailID", user.EmailID);
                        param.Add("@Role", user.Role);
                        param.Add("@Disable", user.Disable);
                        param.Add("@UpdatedBy", user.UpdatedBy);
                        param.Add("@UpdatedDate", user.UpdatedDate);
                        param.Add("@UserID", user.UserID);

                        await conn.ExecuteAsync(sQuery, param, transction, 180).ConfigureAwait(false);

                        sQuery = @"DELETE FROM tbl_UserAccess WHERE UserID = @UserID";

                        param = new DynamicParameters();
                        param.Add("@UserID", user.UserID);

                        await conn.ExecuteAsync(sQuery, param, transction, 180).ConfigureAwait(false);

                        List<UserAccess> userAccess = new List<UserAccess>();

                        for (int i = 0; i < user.UserClaim.Count; i++)
                        {
                            if (user.UserClaim[i].CanRead)
                                userAccess.Add(new UserAccess() { UserID = user.UserID.Value, Claim = "Read" + user.UserClaim[i].PageNameShort });
                            if (user.UserClaim[i].CanCreate)
                                userAccess.Add(new UserAccess() { UserID = user.UserID.Value, Claim = "Create" + user.UserClaim[i].PageNameShort });
                            if (user.UserClaim[i].CanUpdate)
                                userAccess.Add(new UserAccess() { UserID = user.UserID.Value, Claim = "Update" + user.UserClaim[i].PageNameShort });
                        }

                        for (int i = 0; i < userAccess.Count; i++)
                        {
                            sQuery = @"INSERT INTO [dbo].[tbl_UserAccess]
                                        ([UserID]
                                        ,[Claim])
                                     VALUES
                                        (@UserID
                                        ,@Claim)";

                            param = new DynamicParameters();
                            param.Add("@UserID", user.UserID.Value);
                            param.Add("@Claim", userAccess[i].Claim);

                            await conn.ExecuteAsync(sQuery, param, transction, 180).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        sQuery = @"INSERT INTO [dbo].[tbl_Users]
                           ([FirstName]
                           ,[LastName]
                           ,[MobileNo]
                           ,[EmailID]
                           ,[Password]
                           ,[Role]
                           ,[Disable]
                           ,[CreatedBy]
                           ,[CreatedDate])
                     VALUES
                           (@FirstName
                           , @LastName
                           , @MobileNo
                           , @EmailID
                           , @Password
                           , @Role 
                           , @Disable 
                           , @CreatedBy
                           , @CreatedDate);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

                        DynamicParameters param = new DynamicParameters();
                        param.Add("@FirstName", user.FirstName);
                        param.Add("@LastName", user.LastName);
                        param.Add("@MobileNo", user.MobileNo);
                        param.Add("@EmailID", user.EmailID);
                        param.Add("@Password", user.Password);
                        param.Add("@Role", user.Role);
                        param.Add("@Disable", user.Disable);
                        param.Add("@CreatedBy", user.CreatedBy);
                        param.Add("@CreatedDate", user.CreatedDate);

                        userId = conn.QuerySingle<int>(sQuery, param, transction, 180);

                        sQuery = @"DELETE FROM tbl_UserAccess WHERE UserID = @UserID";

                        param = new DynamicParameters();
                        param.Add("@UserID", userId);

                        await conn.ExecuteAsync(sQuery, param, transction, 180).ConfigureAwait(false);

                        List<UserAccess> userAccess = new List<UserAccess>();

                        for (int i = 0; i < user.UserClaim.Count; i++)
                        {
                            if (user.UserClaim[i].CanRead)
                                userAccess.Add(new UserAccess() { UserID = userId, Claim = "Read" + user.UserClaim[i].PageNameShort });
                            if (user.UserClaim[i].CanCreate)
                                userAccess.Add(new UserAccess() { UserID = userId, Claim = "Create" + user.UserClaim[i].PageNameShort });
                            if (user.UserClaim[i].CanUpdate)
                                userAccess.Add(new UserAccess() { UserID = userId, Claim = "Update" + user.UserClaim[i].PageNameShort });
                        }

                        for (int i = 0; i < userAccess.Count; i++)
                        {
                            sQuery = @"INSERT INTO [dbo].[tbl_UserAccess]
                                        ([UserID]
                                        ,[Claim])
                                     VALUES
                                        (@UserID
                                        ,@Claim)";

                            param = new DynamicParameters();
                            param.Add("@UserID", userId);
                            param.Add("@Claim", userAccess[i].Claim);

                            await conn.ExecuteAsync(sQuery, param, transction, 180).ConfigureAwait(false);
                        }
                    }

                    transction.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    transction.Rollback();
                    conn.Close();
                    return 0;
                }
            }
        }

        public async Task<int> DeleteUser(int UserId)
        {
            var sQuery = @"UPDATE tbl_Users SET IsDeleted = 1 WHERE UserID = @UserID";

            var param = new DynamicParameters();
            param.Add("@UserID", UserId);

            return await _genericRepository.ExecuteCommandAsync(sQuery, param);
        }

        public async Task<IEnumerable<User>> GetUsers(UserFilter filter)
        {
            var sQuery = $@"SELECT UserID,FirstName,LastName,FirstName + ' ' + LastName AS 'UserName',MobileNo,EmailID,Role,Disable FROM tbl_Users";

            if (filter.UserId > 0)
                sQuery = sQuery + $" WHERE UserId = { filter.UserId }";
            else if (!string.IsNullOrEmpty(filter.FilterText))
                sQuery = sQuery + $" WHERE FirstName LIKE '%{ filter.FilterText }%' OR EmailID LIKE '%{ filter.FilterText }%' OR LastName LIKE '%{ filter.FilterText }%' OR U.FirstName + ' ' + U.LastName LIKE '%{ filter.FilterText }%'";


            sQuery = sQuery + " ORDER BY UserID DESC";

            var result = await _genericRepository.GetAsync<User>(sQuery, null);

            List<Claims> claims = new List<Claims>();

            claims = GetClaims().Result.ToList();
            if (filter.UserId > 0 && result != null)
            {
                sQuery = @"SELECT UserAccesID , UserID, Claim FROM tbl_UserAccess WHERE UserID = @UserID";

                var param = new DynamicParameters();
                param.Add("@UserID", filter.UserId);

                var resultClaims = await _genericRepository.GetAsync<UserAccess>(sQuery, param);

                if (resultClaims != null && resultClaims.Count() > 0)
                {
                    for (int j = 0; j < claims.Count(); j++)
                    {
                        var page = claims[j].PageNameShort;

                        string read = resultClaims.Where(x => x.Claim == "Read" + page).Select(x => x.Claim).FirstOrDefault();
                        if (!string.IsNullOrEmpty(read))
                            claims[j].CanRead = true;

                        string create = resultClaims.Where(x => x.Claim == "Create" + page).Select(x => x.Claim).FirstOrDefault();
                        if (!string.IsNullOrEmpty(create))
                            claims[j].CanCreate = true;

                        string update = resultClaims.Where(x => x.Claim == "Update" + page).Select(x => x.Claim).FirstOrDefault();
                        if (!string.IsNullOrEmpty(update))
                            claims[j].CanUpdate = true;
                    }
                }
                result.FirstOrDefault().UserClaim = claims;
            }
            return result;
        }



        private async Task<IEnumerable<Claims>> GetClaims()
        {
            List<Claims> result = new List<Claims>();

            //result.Add(new Claims() { PageName = "Visitor's Management", CanCreate = false, CanRead = false, CanUpdate = false, PageNameShort = "VM" });
            result.Add(new Claims() { PageName = "Work Permit", CanCreate = false, CanRead = false, CanUpdate = false, PageNameShort = "WP" });
            //result.Add(new Claims() { PageName = "User Management", CanCreate = false, CanRead = false, CanUpdate = false, PageNameShort = "UM" });
            // result.Add(new Claims() { PageName = "Dashboard", CanCreate = false, CanRead = false, CanUpdate = false, PageNameShort = "DB" });
            result.Add(new Claims() { PageName = "Remote Employee", CanCreate = false, CanRead = false, CanUpdate = false, PageNameShort = "RE" });

            return result;
        }

        public Task<IEnumerable<Claims>> GetDefaultClaims()
        {
            return GetClaims();
        }

        public async Task<int> ChangePassword(UserChangePassword change)
        {
            var sQuery = @"Update tbl_Users Set Password = @NewPassword WHERE UserID= @UserID and Password=@OldPassword";

            var param = new DynamicParameters();
            param.Add("@UserID", change.UserId);
            param.Add("@OldPassword", change.OldPassword);
            param.Add("@NewPassword", change.NewPassword);

            return await _genericRepository.ExecuteCommandAsync(sQuery, param);
        }

        public async Task<int> GetSuperAdminCount()
        {
            try
            {


                var sQuery = $@"SELECT Count(UserID) FROM TBL_USERS WHERE Role = 'Super Admin'";


                var result = await _genericRepository.GetAsync<int>(sQuery, null);

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}