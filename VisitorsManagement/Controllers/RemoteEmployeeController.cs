using QRCoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Visitors_Management.Dto.VM;
using VisitorsManagement.App_Start;
using VisitorsManagement.Models;
using VisitorsManagement.Models.RemoteEmployee;
using VisitorsManagement.Repository;

namespace VisitorsManagement.Controllers
{
    [BasicAuthentication]
    public class RemoteEmployeeController : Controller
    {

        private readonly IRemoteEmployee _IRemoteEmployee;
        public RemoteEmployeeController(IRemoteEmployee remoteEmployee, IVisitorsManagementRepository visitorsManagementRepository,
            IContractorRepository contractorRepository)
        {
            _IRemoteEmployee = remoteEmployee;

        }

        // GET: RemoteEmployee
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> getRemoteEmployee(VisitorsManagement.Models.RemoteEmployee.RemoteEmployeeFilter filter)
        {
            try
            {
                var result = await _IRemoteEmployee.getRemoteEmployee(filter);
                return Json(result);
            }
            catch (Exception ex)
            {
                DB.insertErrorlog("WP", "getAllWorkPermit", ex.Message, Convert.ToInt16(Session["UserID"]));
                return Content("Failed : Error Occured");
            }
        }

        [HttpPost]
        //[BasicAuthentication]
        public async Task<ActionResult> SaveRemoteEmployee(RemoteEmployee RemoteEmployeeModel)
        {
            ResponseClass clsResponse = new ResponseClass();
            try
            {
                if (ModelState.IsValid)
                {

                    RemoteEmployeeModel.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    RemoteEmployeeModel.CreatedDate = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");
                    RemoteEmployeeModel.UpdatedBy = Convert.ToInt32(Session["UserID"]);
                    RemoteEmployeeModel.UpdatedDate = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");

                    var result = await _IRemoteEmployee.SaveRemoteEmployee(RemoteEmployeeModel);


                    if (result > 0)
                    {
                        clsResponse.isSuccessful = true;
                    }
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
                DB.insertErrorlog("RemoteEmployee", "SaveRemoteEmployee", ex.Message, Convert.ToInt16(Session["UserID"]));
                clsResponse.isSuccessful = false;
                clsResponse.message = "Error Occured.";
            }
            return Json(clsResponse);
        }




        [HttpPost]
        //[BasicAuthentication]
        public async Task<ActionResult> SaveSecurityCheck(RemoteEmployeeSecurityCheck RemoteEmployeeSecurityCheck)
        {
            ResponseClass clsResponse = new ResponseClass();
            try
            {
                if (ModelState.IsValid)
                {

                    RemoteEmployeeSecurityCheck.CreatedBySC = Convert.ToInt32(Session["UserID"]);
                    RemoteEmployeeSecurityCheck.CreatedDateSC = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");
                    RemoteEmployeeSecurityCheck.UpdatedBySC = Convert.ToInt32(Session["UserID"]);
                    RemoteEmployeeSecurityCheck.UpdatedDateSC = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");

                    var result = await _IRemoteEmployee.SaveSecurityCheck(RemoteEmployeeSecurityCheck);
                    if (RemoteEmployeeSecurityCheck.AccessCardCollectionStatus == "No" && result == 1)
                    {
                        string applicationURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                        VisitorsManagement.Models.RemoteEmployee.RemoteEmployeeFilter filter = new RemoteEmployeeFilter();
                        filter.Pkey = RemoteEmployeeSecurityCheck.Pkey;
                        var getRemoteEmployee = await _IRemoteEmployee.getRemoteEmployee(filter);
                        string body = string.Empty;
                        using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplate/RemoteEmployeeAccessCardNo.html")))
                        {
                            body = reader.ReadToEnd();
                        }
                        body = body.Replace("{{Re_Number}}", getRemoteEmployee.FirstOrDefault().Re_Number);
                        body = body.Replace("{{Hcode}}", getRemoteEmployee.FirstOrDefault().Hcode);
                        body = body.Replace("{{Name}}", getRemoteEmployee.FirstOrDefault().Name);
                        body = body.Replace("{{EmailID}}", getRemoteEmployee.FirstOrDefault().EmailID);
                        body = body.Replace("{{CheckinDateTime}}", getRemoteEmployee.FirstOrDefault().CheckinDateTime);
                        body = body.Replace("{{CheckOutDateTime}}", getRemoteEmployee.FirstOrDefault().CheckOutDateTime);
                        body = body.Replace("{{Status}}", getRemoteEmployee.FirstOrDefault().Status);
                        body = body.Replace("{{Escalation}}", getRemoteEmployee.FirstOrDefault().Escalation);


                        var EmIlId = ConfigurationManager.AppSettings["RemoteEmployeeCardNotCollectedEmailId"];
                        if(!string.IsNullOrEmpty(EmIlId))
                        {
                            DB.SendMailAsync(EmIlId, body, "Remote Employee Card Not Collected For RE Number:" + getRemoteEmployee.FirstOrDefault().Re_Number);


                        }





                    }


                    if (result > 0)
                    {
                        clsResponse.isSuccessful = true;
                    }
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
                DB.insertErrorlog("RemoteEmployee", "SaveRemoteEmployee", ex.Message, Convert.ToInt16(Session["UserID"]));
                clsResponse.isSuccessful = false;
                clsResponse.message = "Error Occured.";
            }
            return Json(clsResponse);
        }
    }
}