using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VisitorsManagement.App_Start;
using VisitorsManagement.Models;
using VisitorsManagement.Repository;

namespace VisitorsManagement.Controllers
{
    [BasicAuthentication]
    public class UsersController : Controller
    {
        // GET: Users
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetUsers(UserFilter filter)
        {
            //if (_workContext.currentUserDto != null && _workContext.currentUserDto.RoleName == "Employee")
            //    filter.UserId = _workContext.currentUserDto.UserId;

            var result = await _userRepository.GetUsers(filter);

          
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> GetUserSelectList()
        {
            var result = await _userRepository.GetUserSelectList(); ;
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> GetUserSelectListOnlyEmployee()
        {
            var result = await _userRepository.GetUserSelectList(false);
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> GetDefaultClaims()
        {
            var result = await _userRepository.GetDefaultClaims(); ;
            return Json(result);
        }
      
        [HttpPost]
        public async Task<ActionResult> CreateUser(User user)
        {
            ResponseClass clsResponse = new ResponseClass();

            user.CreatedBy = Convert.ToInt32(Session["UserID"]);
            user.UpdatedBy = Convert.ToInt32(Session["UserID"]);
            user.CreatedDate = DateTime.UtcNow;
            user.UpdatedDate = DateTime.UtcNow;
            user.applicationURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

            if (user.UserID == 0 || user.UserID == null)
                user.Password = DB.encrypt(DB.RandomString(8, true));

            try
            {
                if (ModelState.IsValid)
                {
                    if (user.UserID > 0)
                    {
                        var userResult = await _userRepository.GetUsers(new UserFilter() { UserId = user.UserID.Value });
                        if (userResult.FirstOrDefault().Role == "Super Admin" && user.Role != "Super Admin")
                        {
                            int superAdminCount = await _userRepository.GetSuperAdminCount();
                            if (superAdminCount == 1)
                            {
                                clsResponse.isSuccessful = false;
                                clsResponse.message = "There is only one Super Admin so first make someone else super admin and then change the role.";
                                return Json(clsResponse);
                            }
                        }
                    }
                    var result = await _userRepository.CreateUser(user);
                    if (result > 0)
                    {
                        clsResponse.isSuccessful = true;
                        if (user.UserID == 0 || user.UserID == null)
                            sendAccountCreateNotification(user.FirstName, user.EmailID, DB.Decrypt(user.Password), user.applicationURL);
                    }
                        
                    else
                        clsResponse.isSuccessful = false;
                }
                else
                {
                    string errorMessage = string.Empty;
                    errorMessage = DB.getModelErrors(ViewData.ModelState.Values);

                    clsResponse.isSuccessful = false;
                    clsResponse.message = errorMessage;
                }
            }
            catch (Exception ex)
            {
                DB.insertErrorlog("User", "SaveUser", ex.Message, Convert.ToInt16(Session["UserID"]));
                clsResponse.isSuccessful = false;
                clsResponse.message = "Error Occured.";
            }
            return Json(clsResponse);
        }

        private void sendAccountCreateNotification(string FirstName, string EmailID, string Password, string appURL)
        {
            string body = string.Empty;
            string applicationURL = appURL;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplate/UserCreation.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{FirstName}", FirstName);
            body = body.Replace("{UID}", EmailID);
            body = body.Replace("{Password}", Password);
            body = body.Replace("{WebsiteURL}", applicationURL);

            DB.SendMailAsync(EmailID, body, "Visitor's Management Portal: Account Created");
        }
    }
}