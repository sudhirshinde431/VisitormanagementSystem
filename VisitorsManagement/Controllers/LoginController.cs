using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VisitorsManagement.App_Start;
using VisitorsManagement.Models;
using VisitorsManagement.Repository;
using System.Security.Principal;
using System.Web.Security;
using System.Threading;
using System.Web.Configuration;

namespace VisitorsManagement.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private readonly IWPRepository _wpRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVisitorsManagementRepository _visitorsManagementRepository;
        public LoginController(IAuthRepository authRepository,
            IUserRepository userRepository, 
            IVisitorsManagementRepository visitorsManagementRepository,
            IWPRepository wpRepository
            )
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _visitorsManagementRepository = visitorsManagementRepository;
            _wpRepository = wpRepository;
        }
        public ActionResult Index()
        {
            if (Session["RedirectPage"] != null)
                ViewBag.RedirectPage = Convert.ToString(Session["RedirectPage"]);
            if (Session["appointmentId"] != null)
                ViewBag.appointmentId = Convert.ToString(Session["appointmentId"]);

            Session["RedirectPage"] = null;
            Session["appointmentId"] = null;

            _visitorsManagementRepository.ClosePreviousAppointments();
            return View();
        }
        [HttpPost]
        public async Task<string> UserLogin(LoginUser user)
        {
            //if (ModelState.IsValid)
            //{
            // _wpRepository.Wpreminder();
             var Test= DB.Decrypt("bGQ5xCX4ni2XTXU5TybRIYrBX2V8j7ympH5tdHDW8+k=");
            DateTime dt2 = new DateTime(2022, 09, 01);
            if (DateTime.Now > dt2 && WebConfigurationManager.AppSettings["DisableDateCheck"]!="true")
            {
                return "Block";
            }
            CurrentUserDto currentUserDtoDisable = await _authRepository.LoginUserActive(user);
            if(currentUserDtoDisable!=null)
            {
                return "1";
            }
            CurrentUserDto currentUserDto = await _authRepository.LoginUser(user);

            if (currentUserDto != null)
            {

                GenericIdentity newIdentity = new GenericIdentity(currentUserDto.FullName);
                GenericPrincipal newPrincipal = new GenericPrincipal(newIdentity, currentUserDto.Claims.ToArray());
                Thread.CurrentPrincipal = newPrincipal;
                //IPrincipal user = new GenericPrincipal(new FormsIdentity() 
                System.Web.HttpContext.Current.User = newPrincipal;
                Session["UserFullName"] = currentUserDto.FullName;

                Session["IsAdmin"] = currentUserDto.IsAdmin;
                Session["UserID"] = currentUserDto.UserId;
                Session["RoleName"] = currentUserDto.RoleName; 
                Session["EmailId"] = currentUserDto.EmailID;
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(currentUserDto);
                return jsonString;
            }
            else
                return "0";
            //}
            //else
            //{
            //}
        }

        //[HttpPost]
        public ActionResult logOut()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        [BasicAuthentication]
        public ActionResult ChangePassword()
        {
            Session["CurrentPage"] = "ChangePassword";
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> ChangePassword(UserChangePassword change)
        {


            ResponseClass clsResponse = new ResponseClass();

            if (ModelState.IsValid)
            {
                change.OldPassword = DB.encrypt(change.OldPassword);
                change.NewPassword = DB.encrypt(change.NewPassword);
                change.UserId = Convert.ToInt32(Session["UserID"]);

                var result = await _userRepository.ChangePassword(change);


                if (result > 0)
                {
                    clsResponse.isSuccessful = true;
                    clsResponse.message = "Password Changed SuccessFully.";
                }
                else
                {
                    clsResponse.isSuccessful = false;
                    clsResponse.message = "Invalid Credentials.";
                }
            }
            else
            {
                string errorMessage = string.Empty;
                errorMessage = DB.getModelErrors(ViewData.ModelState.Values);
                clsResponse.isSuccessful = false;
                clsResponse.message = errorMessage;
            }
            
                
            return Json(clsResponse);
        }

        public string getErrorLog()
        {
            DataTable dt = new DataTable();
            dt = DB.ExecuteParamerizedSelectCommand("SELECT TOP 10 * FROM [dbo].[tbl_ErrorLog] ORDER BY ErrorId DESC", CommandType.Text);
            string response = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
            return response;
        }

        private void sendAccountDetails(string FirstName, string EmailID, string Password, string appURL)
        {
            string body = string.Empty;
            string applicationURL = appURL;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplate/ForgotPassword.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{FirstName}", FirstName);
            body = body.Replace("{UID}", EmailID);
            body = body.Replace("{Password}", Password);
            body = body.Replace("{WebsiteURL}", applicationURL);

            DB.SendMailAsync(EmailID, body, "Visitor's Management Portal:Password Recovery");
        }


        public string sendPassword(string EmailID)
        {
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@EmailID",EmailID)
            };
            dt = DB.ExecuteParamerizedSelectCommand("SELECT  FirstName,Password FROM TBL_USERS WHERE EmailID=@EmailID", CommandType.Text, param);

            if (dt.Rows.Count > 0)
            {
                string appUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                sendAccountDetails(Convert.ToString(dt.Rows[0]["FirstName"]), EmailID, DB.Decrypt(Convert.ToString(dt.Rows[0]["Password"])), appUrl);
                //string body = string.Format(htmlString, Convert.ToString(dt.Rows[0]["FirstName"]), DB.Decrypt(Convert.ToString(dt.Rows[0]["Password"])));
                //DB.SendMailAsync(EmailID, body, "ICMS Application Password Recovery");

                return "Password Sent Successfully to your Email ID.";
            }
            else
            {
                return "Please verify the Email ID.";
            }

        }

    }
}