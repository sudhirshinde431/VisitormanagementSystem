using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Visitors_Management.Dto.VM;
using VisitorsManagement.App_Start;
using VisitorsManagement.Models;
using VisitorsManagement.Repository;
using static QRCoder.PayloadGenerator;

using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using System.Diagnostics;

namespace VisitorsManagement.Controllers
{
    [ExceptionHandler]
    public class VMController : Controller
    {
        private readonly IVisitorsManagementRepository _visitorsManagementRepository;
        public VMController(IVisitorsManagementRepository visitorsManagementRepository)
        {
            _visitorsManagementRepository = visitorsManagementRepository;
        }
        // GET: VM
        [BasicAuthentication]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CheckIn(string appointmentId)
        {
            if (!string.IsNullOrEmpty(appointmentId))
                ViewBag.AppointmentId = DB.Decrypt(appointmentId);
            else
                ViewBag.AppointmentId = "";
            if (Session["UserID"] == null)
            {
                Session["RedirectPage"] = "CheckIn";
                Session["appointmentId"] = appointmentId;
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public ActionResult AppointmentApproval(string appointmentId)
        {
            if (!string.IsNullOrEmpty(appointmentId))
                ViewBag.AppointmentId = DB.Decrypt(appointmentId);
            else
                ViewBag.AppointmentId = "";

            return View();
        }


        public async Task<ActionResult> getAllAppointment(VMFilter filter)
        {
            try
            {
                string roleName = Convert.ToString(Session["RoleName"]);
                int userId = Convert.ToInt16(Session["UserID"]);

                if (roleName == "Employee")
                {
                    filter.UserId = userId;
                }

                IEnumerable<VM> result = await _visitorsManagementRepository.GetVisitors(filter);
                return Json(result);
            }

            catch (Exception ex)
            {
                DB.insertErrorlog("Appointment", "VM_getAllAppointment", ex.Message, Convert.ToInt16(Session["UserID"]));
                return Content("Failed : Error Occured");
            }
        }

        [HttpPost]
        //[BasicAuthentication]
        public async Task<ActionResult> raiseAppointment(VM vmmodel)
        {
            ResponseClass clsResponse = new ResponseClass();
            try
            {
                if (ModelState.IsValid)
                {

                    vmmodel.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    vmmodel.CreatedDate = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");
                    vmmodel.UpdatedBy = Convert.ToInt32(Session["UserID"]);
                    vmmodel.UpdatedDate = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");
                    var result = await _visitorsManagementRepository.RaiseAppointment(vmmodel);


                    if (result > 0)
                    {


                        if (vmmodel.AppointmentID == null || vmmodel.AppointmentID == 0)
                        {
                            var data = await _visitorsManagementRepository.GetVisitors(new VMFilter() { AppointmentId = result });
                            VM vM = data.FirstOrDefault();

                            string VisitingPersonName = vmmodel.PersonToVisitName;
                            string VisitorsName = vmmodel.VisitorName;
                            string MobileNo = vmmodel.VisitorPhoneNumber;
                            string DateDetail = Convert.ToDateTime(vmmodel.Date).ToString("dd-MMM-yyyy");
                            string RepresentingCompany = vmmodel.RepresentingCompany;
                            string PurposeOfVisit = vmmodel.PurposeToVisit;
                            string AppointmentNo = vM.AppointmentNo;
                            string DirectApproval = "No";
                            if (vmmodel.DirectApproval == "True")
                            {
                                DirectApproval = "Yes";
                            }
                            string applicationURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                            if (vmmodel.DirectApproval == "True")
                            {

                                string DETAILHEADER = "Appointment of Visitor " + vM.VisitorName + " has been direct approved,";

                                string BatchNo = vM.GatePassNumber;
                                //string applicationURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);


                                string body = string.Empty;
                                //string path = Path.Combine(_hostEnvironment.ContentRootPath, "EmailTemplate\\AppiontmentRaised.html");
                                //string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"EmailTemplate\\AppiontmentRaised.html"}");

                                string htmlFilePath = string.Empty;

                                htmlFilePath = System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplate/DirectApproved.html");

                                using (StreamReader reader = new StreamReader(htmlFilePath))
                                {
                                    body = reader.ReadToEnd();
                                }
                                body = body.Replace("{{DETAILHEADER}}", DETAILHEADER);
                                body = body.Replace("{{BodyHeader}}", "Appointment Details are as follows,");

                                body = body.Replace("{{VisitorsName}}", VisitorsName);
                                body = body.Replace("{{VisitingPersonName}}", VisitingPersonName);
                                body = body.Replace("{{DateDetail}}", DateDetail);
                                body = body.Replace("{{RemarkDetail}}", vM.Remark);
                                body = body.Replace("{{CompanyName}}", RepresentingCompany);
                                body = body.Replace("{{PurposeOfVisit}}", PurposeOfVisit);
                                body = body.Replace("{{VisitTo}}", vM.PersonToVisitName);
                                body = body.Replace("{{AppointmentNo}}", AppointmentNo);
                                body = body.Replace("{{BatchNo}}", BatchNo);
                                //body = body.Replace("{WebsiteURL}", applicationURL);

                                DB.SendMailAsync(vM.PersonToVisitEmailID + "," + vM.VisitorsEmails, body, DETAILHEADER);
                                clsResponse.isSuccessful = true;

                            }
                            else
                            {
                                if (Convert.ToString(Session["RoleName"]) == "Security")
                                {
                                    var nameAndEmail = await _visitorsManagementRepository.GetVisitingPersonEmail(vmmodel.PersonToVisitID);
                                    string body = string.Empty;
                                    //string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"EmailTemplate\\AppiontmentRaised.html"}");

                                    //string applicationURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                                    using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplate/AppiontmentRaisedBySecurity.html")))
                                    {
                                        body = reader.ReadToEnd();
                                    }
                                    body = body.Replace("{{VisitingPersonName}}", VisitingPersonName);
                                    body = body.Replace("{{VisitorsName}}", VisitorsName);
                                    body = body.Replace("{{DateDetail}}", DateDetail);
                                    body = body.Replace("{{RemarkDetail}}", vmmodel.Remark);
                                    body = body.Replace("{{CompanyName}}", RepresentingCompany);
                                    body = body.Replace("{{PurposeOfVisit}}", PurposeOfVisit);
                                    body = body.Replace("{{VisitTo}}", nameAndEmail.Item1);
                                    body = body.Replace("{{AppointmentNo}}", AppointmentNo);
                                    //string yesLink = applicationURL +  @"/VM/ApproveRejectAppointment?appointmentId=" + result + "&status=Open";
                                    //string noLink = applicationURL +  @"/VM/ApproveRejectAppointment?appointmentId=" + result + "&status=Rejected";

                                    string approveRejectUrl = applicationURL + @"/VM/AppointmentApproval?appointmentId=" + DB.encrypt(result.ToString());

                                    body = body.Replace("{{approveRejectURL}}", approveRejectUrl);



                                    DB.SendMailAsync(nameAndEmail.Item2, body, "Security raised as appointment for " + vmmodel.VisitorName);

                                }
                                else
                                {
                                    string body = string.Empty;
                                    //string path = Path.Combine(_hostEnvironment.ContentRootPath, "EmailTemplate\\AppiontmentRaised.html");
                                    //string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"EmailTemplate\\AppiontmentRaised.html"}");
                                    using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplate/AppiontmentRaised.html")))
                                    {
                                        body = reader.ReadToEnd();
                                    }
                                    body = body.Replace("{{VisitingPersonName}}", VisitingPersonName);
                                    body = body.Replace("{{VisitorsName}}", VisitorsName);
                                    body = body.Replace("{{DateDetail}}", DateDetail);
                                    body = body.Replace("{{RemarkDetail}}", vmmodel.Remark);
                                    body = body.Replace("{{CompanyName}}", RepresentingCompany);
                                    body = body.Replace("{{PurposeOfVisit}}", PurposeOfVisit);
                                    body = body.Replace("{{VisitTo}}", Convert.ToString(Session["UserFullName"]));
                                    body = body.Replace("{{AppointmentNo}}", AppointmentNo);
                                    body = body.Replace("{{DirectApproval}}", DirectApproval);
                                    string checkInUrl = applicationURL + @"/VM/CheckIn?appointmentId=" + DB.encrypt(result.ToString());

                                    Url generator = new Url(checkInUrl);
                                    string payload = generator.ToString();

                                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
                                    QRCode qrCode = new QRCode(qrCodeData);
                                    var qrCodeAsBitmap = qrCode.GetGraphic(20);

                                    var qrBytes = DB.ImageToByte(qrCodeAsBitmap);

                                    string fileName = DB.RandomString(5, false);

                                    //if (!System.IO.Directory.Exists(Server.MapPath("~/Downloads/")))
                                    //    System.IO.Directory.CreateDirectory(Server.MapPath("~/Downloads/"));

                                    using (System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(qrBytes)))
                                    {
                                        image.Save(Server.MapPath("~/Downloads/") + fileName + ".jpg", ImageFormat.Jpeg);  // Or Png
                                        image.Dispose();
                                    }

                                    //var i = Image.FromStream(new MemoryStream(qrBytes));

                                    //var i2 = new Bitmap(i);
                                    //i2.Save(Server.MapPath("~/Downloads/") + fileName + ".jpg", ImageFormat.Jpeg);

                                    //qrCodeAsBitmap.Save("QRcodeImage", System.Drawing.Imaging.ImageFormat.Png);
                                    string folderPath = Server.MapPath("~/Downloads/");
                                    DB.deleteOldUnusedFiles(folderPath);

                                    //body = body.Replace("{WebsiteURL}", applicationURL);
                                    DB.SendMailAsync(vmmodel.VisitorsEmails + "," + Convert.ToString(Session["EmailId"]), body, "Visitor's appointment raised for " + vmmodel.VisitorName, "Downloads/" + fileName + ".jpg");
                                    //EmailManager.SendEmail("Visitor's appointment raised for " + filter.VisitorName, body, filter.VisitorsEmails + "," + _workContext.currentUserDto.EmailID);
                                }
                            }
                            clsResponse.isSuccessful = true;

                        }
                        //else
                        //{
                        //    var data = await _visitorsManagementRepository.GetVisitors(new VMFilter() { AppointmentId = vm.AppointmentID.Value });
                        //    VM oldDetails = data.FirstOrDefault();

                        //    if (vm.Date != oldDetails.strDate)
                        //    {

                        //    }

                        //}
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
                DB.insertErrorlog("Appointment", "VM_getAllAppointment", ex.Message, Convert.ToInt16(Session["UserID"]));
                clsResponse.isSuccessful = false;
                clsResponse.message = "Error Occured.";
            }
            return Json(clsResponse);
        }

        [HttpPost]
        public async Task<ActionResult> ApproveRejectAppointment(VM_ApproveRejectAppointment filter)
        {
            ResponseClass clsResponse = new ResponseClass();

            try
            {
                VM_ApproveRejectAppointment appointment = new VM_ApproveRejectAppointment();
                appointment.UpdatedBy = 0;
                appointment.UpdatedDate = DateTime.UtcNow;
                appointment.AppointmentID = filter.AppointmentID;
                appointment.Status = filter.Status;
                var result = await _visitorsManagementRepository.ApproveRejectAppointment(appointment);

                if (result == 1)
                {
                    var data = await _visitorsManagementRepository.GetVisitors(new VMFilter() { AppointmentId = filter.AppointmentID });
                    VM vM = data.FirstOrDefault();

                    string DETAILHEADER = "";
                    if (filter.Status == "Open")
                        DETAILHEADER = "Appointment of Visitor " + vM.VisitorName + " has been approved,";
                    else
                        DETAILHEADER = "Appointment of Visitor " + vM.VisitorName + " has been Rejected,";

                    string VisitorsName = vM.VisitorName;
                    string VisitingPersonName = vM.PersonToVisitName;
                    string MobileNo = vM.VisitorPhoneNumber;
                    string DateDetail = Convert.ToDateTime(vM.strDate).ToString("dd-MMM-yyyy");
                    string RepresentingCompany = vM.RepresentingCompany;
                    string PurposeOfVisit = vM.PurposeToVisit;
                    string AppointmentNo = vM.AppointmentNo;
                    string BatchNo = vM.GatePassNumber;
                    //string applicationURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    try
                    {
                        string body = string.Empty;
                        //string path = Path.Combine(_hostEnvironment.ContentRootPath, "EmailTemplate\\AppiontmentRaised.html");
                        //string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"EmailTemplate\\AppiontmentRaised.html"}");

                        string htmlFilePath = string.Empty;

                        if (filter.Status == "Open")
                            htmlFilePath = System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplate/AppiontmentApprovals.html");
                        else
                            htmlFilePath = System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplate/RejectAppointment.html");

                        using (StreamReader reader = new StreamReader(htmlFilePath))
                        {
                            body = reader.ReadToEnd();
                        }
                        body = body.Replace("{{DETAILHEADER}}", DETAILHEADER);
                        if (filter.Status == "Open")
                            body = body.Replace("{{BodyHeader}}", "Appointment Details are as follows,");
                        else
                            body = body.Replace("{{BodyHeader}}", "Appointment has been rejected by the Approver, <br/ > <br/ >  Appointment Details are as follows,");

                        body = body.Replace("{{VisitorsName}}", VisitorsName);
                        body = body.Replace("{{VisitingPersonName}}", VisitingPersonName);
                        body = body.Replace("{{DateDetail}}", DateDetail);
                        body = body.Replace("{{RemarkDetail}}", vM.Remark);
                        body = body.Replace("{{CompanyName}}", RepresentingCompany);
                        body = body.Replace("{{PurposeOfVisit}}", PurposeOfVisit);
                        body = body.Replace("{{VisitTo}}", vM.PersonToVisitName);
                        body = body.Replace("{{AppointmentNo}}", AppointmentNo);
                        body = body.Replace("{{BatchNo}}", BatchNo);
                        //body = body.Replace("{WebsiteURL}", applicationURL);

                        DB.SendMailAsync(vM.PersonToVisitEmailID + "," + vM.VisitorsEmails, body, DETAILHEADER);
                        clsResponse.isSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        DB.insertErrorlog("VM", "CheckInAppointment", ex.Message, Convert.ToInt16(Session["UserID"]));
                        clsResponse.isSuccessful = true;
                    }
                }

                clsResponse.isSuccessful = true;
                return Json(clsResponse);
            }
            catch (Exception ex)
            {
                clsResponse.isSuccessful = false;
                clsResponse.message = ex.Message;
            }
            return Json(clsResponse);
        }

        [HttpPost]
        public async Task<ActionResult> CheckInAppointment(VM_CheckIn filter)
        {
            ResponseClass clsResponse = new ResponseClass();

            if (ModelState.IsValid)
            {
                filter.UpdatedBy = Convert.ToInt32(Session["UserID"]); ;
                filter.UpdatedDate = DB.getCurrentIndianDate();
                var result = await _visitorsManagementRepository.CheckInAppointment(filter);

                if (result == 1)
                {
                    var data = await _visitorsManagementRepository.GetVisitors(new VMFilter() { AppointmentId = filter.AppointmentID });
                    VM vM = data.FirstOrDefault();

                    string DETAILHEADER = "Visitor " + vM.VisitorName + " Checked in,";
                    string VisitorsName = vM.VisitorName;
                    string VisitingPersonName = vM.PersonToVisitName;
                    string MobileNo = vM.VisitorPhoneNumber;
                    string DateDetail = Convert.ToDateTime(vM.strDate).ToString("dd-MMM-yyyy");
                    string AppointmentNo = vM.AppointmentNo;
                    string BatchNo = vM.GatePassNumber;
                    string RepresentingCompany = vM.RepresentingCompany;
                    string PurposeOfVisit = vM.PurposeToVisit;
                    //string applicationURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    try
                    {
                        string body = string.Empty;
                        //string path = Path.Combine(_hostEnvironment.ContentRootPath, "EmailTemplate\\AppiontmentRaised.html");
                        //string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"EmailTemplate\\AppiontmentRaised.html"}");
                        using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplate/AppiontmentApprovals.html")))
                        {
                            body = reader.ReadToEnd();
                        }
                        body = body.Replace("{{DETAILHEADER}}", DETAILHEADER);
                        body = body.Replace("{{BodyHeader}}", "Appointment Details are as follows,");
                        body = body.Replace("{{VisitorsName}}", VisitorsName);
                        body = body.Replace("{{VisitingPersonName}}", VisitingPersonName);
                        body = body.Replace("{{DateDetail}}", DateDetail);
                        body = body.Replace("{{RemarkDetail}}", vM.Remark);
                        body = body.Replace("{{CompanyName}}", RepresentingCompany);
                        body = body.Replace("{{PurposeOfVisit}}", PurposeOfVisit);
                        body = body.Replace("{{VisitTo}}", VisitingPersonName);
                        body = body.Replace("{{AppointmentNo}}", AppointmentNo);
                        body = body.Replace("{{BatchNo}}", BatchNo);
                        //body = body.Replace("{WebsiteURL}", applicationURL);

                        DB.SendMailAsync(vM.PersonToVisitEmailID, body, "Visitor " + vM.VisitorName + " Checked in ");
                        clsResponse.isSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        DB.insertErrorlog("VM", "CheckInAppointment", ex.Message, Convert.ToInt16(Session["UserID"]));
                        clsResponse.isSuccessful = true;
                    }
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
        [HttpPost]

        [BasicAuthentication]
        public async Task<ActionResult> CheckOutAppointment(VM_CheckOut filter)
        {
            ResponseClass clsResponse = new ResponseClass();
            var result = await _visitorsManagementRepository.CheckOutAppointment(filter);

            if (result == 1)
            {
                var data = await _visitorsManagementRepository.GetVisitors(new VMFilter() { AppointmentId = filter.AppointmentID });
                VM vM = data.FirstOrDefault();

                string DETAILHEADER = "Visitor " + vM.VisitorName + " Checked out,";
                string VisitorsName = vM.VisitorName;
                string VisitingPersonName = vM.PersonToVisitName;
                string MobileNo = vM.VisitorPhoneNumber;
                string DateDetail = Convert.ToDateTime(vM.strDate).ToString("dd-MMM-yyyy");
                string RepresentingCompany = vM.RepresentingCompany;
                string PurposeOfVisit = vM.PurposeToVisit;
                string AppointmentNo = vM.AppointmentNo;
                string BatchNo = vM.GatePassNumber;
                //string applicationURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                try
                {
                    string body = string.Empty;
                    //string path = Path.Combine(_hostEnvironment.ContentRootPath, "EmailTemplate\\AppiontmentRaised.html");
                    //string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"EmailTemplate\\AppiontmentRaised.html"}");
                    using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplate/AppiontmentApprovals.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{{DETAILHEADER}}", DETAILHEADER);
                    body = body.Replace("{{BodyHeader}}", "Appointment Details are as follows,");
                    body = body.Replace("{{VisitingPersonName}}", VisitingPersonName);
                    body = body.Replace("{{VisitorsName}}", VisitorsName);
                    body = body.Replace("{{DateDetail}}", DateDetail);
                    body = body.Replace("{{RemarkDetail}}", vM.Remark);
                    body = body.Replace("{{CompanyName}}", RepresentingCompany);
                    body = body.Replace("{{PurposeOfVisit}}", PurposeOfVisit);
                    body = body.Replace("{{VisitTo}}", VisitingPersonName);
                    body = body.Replace("{{AppointmentNo}}", AppointmentNo);
                    body = body.Replace("{{BatchNo}}", BatchNo);
                    //body = body.Replace("{WebsiteURL}", applicationURL);
                    DB.SendMailAsync(vM.PersonToVisitEmailID, body, "Visitor " + vM.VisitorName + " Checked out");
                    clsResponse.isSuccessful = true;
                    //EmailManager.SendEmail("Visitor " + vM.VisitorName + "Checked out", body, _workContext.currentUserDto.EmailID);
                }
                catch (Exception ex)
                {
                    clsResponse.isSuccessful = true;
                    DB.insertErrorlog("VM", "CheckInAppointment", ex.Message, Convert.ToInt16(Session["UserID"]));
                }
            }
            return Json(clsResponse);
        }
        [HttpPost]

        [BasicAuthentication]
        public async Task<ActionResult> CancelAppointment(VM_CancelAppointment filter)
        {
            ResponseClass clsResponse = new ResponseClass();
            try
            {
                var result = await _visitorsManagementRepository.CancelAppointment(filter);

                if (result == 1)
                {
                    var data = await _visitorsManagementRepository.GetVisitors(new VMFilter() { AppointmentId = filter.AppointmentID });
                    VM vM = data.FirstOrDefault();

                    string DETAILHEADER = "Appointment of Visitor " + vM.VisitorName + " has been Cancelled,";
                    string VisitorsName = vM.VisitorName;
                    string VisitingPersonName = vM.PersonToVisitName;
                    string MobileNo = vM.VisitorPhoneNumber;
                    string DateDetail = Convert.ToDateTime(vM.strDate).ToString("dd-MMM-yyyy");
                    string RepresentingCompany = vM.RepresentingCompany;
                    string PurposeOfVisit = vM.PurposeToVisit;
                    //string applicationURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    try
                    {
                        string body = string.Empty;
                        //string path = Path.Combine(_hostEnvironment.ContentRootPath, "EmailTemplate\\AppiontmentRaised.html");
                        //string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"EmailTemplate\\AppiontmentRaised.html"}");
                        using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplate/AppiontmentApprovals.html")))
                        {
                            body = reader.ReadToEnd();
                        }
                        body = body.Replace("{{DETAILHEADER}}", DETAILHEADER);
                        body = body.Replace("{{BodyHeader}}", "Appointment Details are as follows,");
                        body = body.Replace("{{VisitorsName}}", VisitorsName);
                        body = body.Replace("{{VisitingPersonName}}", VisitingPersonName);
                        body = body.Replace("{{DateDetail}}", DateDetail);
                        body = body.Replace("{{RemarkDetail}}", vM.Remark);
                        body = body.Replace("{{CompanyName}}", RepresentingCompany);
                        body = body.Replace("{{PurposeOfVisit}}", PurposeOfVisit);
                        body = body.Replace("{{VisitTo}}", VisitingPersonName);
                        //body = body.Replace("{WebsiteURL}", applicationURL);

                        DB.SendMailAsync(vM.PersonToVisitEmailID, body, "Appointment of Visitor " + vM.VisitorName + " has been Cancelled");
                        clsResponse.isSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        DB.insertErrorlog("VM", "CancelAppointment", ex.Message, Convert.ToInt16(Session["UserID"]));
                        clsResponse.isSuccessful = true;
                    }
                }

                clsResponse.isSuccessful = true;

            }
            catch (Exception ex)
            {
                clsResponse.isSuccessful = true;
                DB.insertErrorlog("VM", "CheckInAppointment", ex.Message, Convert.ToInt16(Session["UserID"]));
            }

            return Json(clsResponse);
        }

        [HttpPost]
        public async Task<ActionResult> RejectAppointment(VM_RejectAppointment filter)
        {
            ResponseClass clsResponse = new ResponseClass();
            try
            {
                var result = await _visitorsManagementRepository.RejectAppointment(filter);

                if (result == 1)
                {
                    var data = await _visitorsManagementRepository.GetVisitors(new VMFilter() { AppointmentId = filter.AppointmentID });
                    VM vM = data.FirstOrDefault();

                    string DETAILHEADER = "Appointment of Visitor " + vM.VisitorName + " has been Rejected,";
                    string VisitorsName = vM.VisitorName;
                    string VisitingPersonName = vM.PersonToVisitName;
                    string MobileNo = vM.VisitorPhoneNumber;
                    string DateDetail = Convert.ToDateTime(vM.strDate).ToString("dd-MMM-yyyy");
                    string RepresentingCompany = vM.RepresentingCompany;
                    string PurposeOfVisit = vM.PurposeToVisit;
                    //string applicationURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    try
                    {
                        string body = string.Empty;
                        //string path = Path.Combine(_hostEnvironment.ContentRootPath, "EmailTemplate\\AppiontmentRaised.html");
                        //string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"EmailTemplate\\AppiontmentRaised.html"}");
                        using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplate/RejectAppointment.html")))
                        {
                            body = reader.ReadToEnd();
                        }
                        body = body.Replace("{{DETAILHEADER}}", DETAILHEADER);
                        body = body.Replace("{{BodyHeader}}", "Appointment Details are as follows,");
                        body = body.Replace("{{VisitorsName}}", VisitorsName);
                        body = body.Replace("{{VisitingPersonName}}", VisitingPersonName);
                        body = body.Replace("{{DateDetail}}", DateDetail);
                        body = body.Replace("{{RemarkDetail}}", vM.Remark);
                        body = body.Replace("{{CompanyName}}", RepresentingCompany);
                        body = body.Replace("{{PurposeOfVisit}}", PurposeOfVisit);
                        body = body.Replace("{{VisitTo}}", VisitingPersonName);
                        //body = body.Replace("{WebsiteURL}", applicationURL);

                        DB.SendMailAsync(vM.PersonToVisitEmailID, body, "Appointment of Visitor " + vM.VisitorName + " has been Rejected");
                        clsResponse.isSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        DB.insertErrorlog("VM", "RejectAppointment", ex.Message, Convert.ToInt16(Session["UserID"]));
                        clsResponse.isSuccessful = true;
                    }
                }

                clsResponse.isSuccessful = true;
            }
            catch (Exception ex)
            {
                DB.insertErrorlog("VM", "RejectAppointment", ex.Message, Convert.ToInt16(Session["UserID"]));
                clsResponse.isSuccessful = true;
            }
            return Json(clsResponse);
        }

        [HttpPost]
        [BasicAuthentication]
        public async Task<ActionResult> GenerateChart(VM_Chart filter)
        //[HttpGet]
        //public async Task<ActionResult> GenerateChart(string type)
        {
            //VM_Chart filter = new VM_Chart() { type = type };
            ResponseClass clsResponse = new ResponseClass();
            try
            {
                var result = await _visitorsManagementRepository.GenerateChart(filter);
                clsResponse.data = result;
                clsResponse.isSuccessful = true;
            }
            catch (Exception ex)
            {
                DB.insertErrorlog("VM", "CheckInAppointment", ex.Message, Convert.ToInt16(Session["UserID"]));
                clsResponse.isSuccessful = false;
            }
            return Json(clsResponse);
        }
        private void deleteFilesfromDownloads()
        {
            var downloadbasepath = Server.MapPath("\\Downloads\\");
            System.IO.DirectoryInfo di = new DirectoryInfo(downloadbasepath);

            foreach (FileInfo file in di.GetFiles())
            {
                if (DateTime.UtcNow - file.CreationTimeUtc > TimeSpan.FromDays(1))
                {
                    file.Delete();
                }
            }
        }
        iText.Kernel.Font.PdfFont font;
        public async Task<string> downloadReport(int AppointmentId)
        {
            ResponseClass responseClass = new ResponseClass();

            try
            {
                var language = "";

                var downloadbasepath = Server.MapPath("\\Downloads\\");

                deleteFilesfromDownloads();
                var basepath = Server.MapPath("\\");

                VMFilter filter = new VMFilter() { AppointmentId = AppointmentId };
                var result = await _visitorsManagementRepository.GetVisitors(filter).ConfigureAwait(false);
                VM vm = new VM();
                if (result == null)
                {
                    responseClass.isSuccessful = false;
                    responseClass.message = "No Data Found.";
                }
                vm = result.FirstOrDefault();
                string tempFilename = "Visitor's Appointment - " + vm.AppointmentNo + ".pdf";

                var fontFilePath = "";
                language = "English";

                var docName = tempFilename;

                var filapath = downloadbasepath + tempFilename;

                var absolutePath = DB.GetAbsoluteUrl("Downloads/");

                fontFilePath = Server.MapPath("~/fonts/arial.ttf");

                font = PdfFontFactory.CreateFont(fontFilePath, PdfEncodings.IDENTITY_H, true);

                PdfWriter writer = new PdfWriter(filapath);
                iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                iText.Kernel.Geom.Rectangle pagesize = new iText.Kernel.Geom.Rectangle(612, 864);
                PageSize pagesize3x5 = new PageSize(pagesize);

                Document document = new Document(pdf, pagesize3x5);


                document.SetMargins(20, 20, 20, 20);

                document.SetBorderTop(new SolidBorder(ColorConstants.BLACK, 1f));
                document.SetBorderTop(new SolidBorder(ColorConstants.BLACK, 1f));
                document.SetBorderTop(new SolidBorder(ColorConstants.BLACK, 1f));
                document.SetBorderTop(new SolidBorder(ColorConstants.BLACK, 1f));


                #region Header

                float[] pointColumnWidths = { 0.8F, 2.4F, 0.8F };

                Table headerTable = new Table(pointColumnWidths, true);
                var imageFilePAth = basepath + "content\\images\\MiniLogo.jpg";

                ImageData imageData = ImageDataFactory.Create(imageFilePAth);


                iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(100, 30);

                Cell cell1 = new Cell();     // Creating a cell 

                // Adding cell 2 to the table 
                Cell celladdress1 = new Cell();     // Creating a cell 
                Paragraph companyName = new Paragraph("Husqvarna (India) Products Private Limited.").SetFontSize(12).SetBold().SetFont(font);//.SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER); ;
                Paragraph companyAddress1 = new Paragraph("G No 312A/B266269/270/276 Hissa no 12,P-9 to15").SetFontSize(9).SetFont(font);//.SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER); ;
                Paragraph companyAddress2 = new Paragraph("Tehsil Gondedumala, Igatpuri Nashik, Maharashtra, India – 422403").SetFontSize(9).SetFont(font);//.SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER); ;

                companyName.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                companyAddress1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                companyAddress2.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                celladdress1.Add(companyName);
                celladdress1.Add(companyAddress1);
                celladdress1.Add(companyAddress2);

                Cell cellLogo = new Cell().Add(image).SetPaddingLeft(5);//.SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER); ;

                cellLogo.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                cellLogo.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);

                headerTable.AddCell(cellLogo);
                headerTable.AddCell(celladdress1);
                headerTable.AddCell(cell1);


                Cell title = new Cell(1, 3);
                title.Add(new Paragraph("Visitor's Appointment").SetFont(font).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)).SetFontSize(12).SetBold().SetBorderTop(Border.NO_BORDER).SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 0.5f)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 0.5f)).SetBorderBottom(new SolidBorder(ColorConstants.BLACK, 0.5f));
                title.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                title.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
                title.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                headerTable.AddCell(title);
                #endregion

                float[] ColstblCommonSection = { 1, 1 };
                Table tblCommonSection = new Table(ColstblCommonSection, true);
                tblCommonSection.AddCell(addCellKeyValue("Appointment No: ", vm.AppointmentNo, true, 1, 8));
                tblCommonSection.AddCell(addCellKeyValue("Appointment Date: ", vm.Date, true, 1, 8));
                tblCommonSection.AddCell(addCellKeyValue("Visitor Card: ", vm.GatePassNumber == null ? "" : vm.GatePassNumber, true, 1, 8));
                tblCommonSection.AddCell(addCellKeyValue("Visitor Name: ", vm.VisitorName, true, 2, 8));
                tblCommonSection.AddCell(addCellKeyValue("Visit to: ", vm.PersonToVisitName, true, 1, 8));
                tblCommonSection.AddCell(addCellKeyValue("Purpose to Visit: ", vm.PurposeToVisit, true, 1, 8));
                tblCommonSection.AddCell(addCellKeyValue("Company Name: ", vm.RepresentingCompany, true, 2, 8));
                tblCommonSection.AddCell(addCellKeyValue("Remark: ", vm.Remark, true, 2, 8));
                tblCommonSection.AddCell(addEmptyCell(1, 2, false, 8));
                tblCommonSection.AddCell(addEmptyCell(1, 2, false, 8));
                //Cell cellBlank = new Cell(1, 2);
                Table tblSecondSectionHeader = new Table(ColstblCommonSection, true);

                tblCommonSection.AddCell(addCellKeyValue("Visit to: ", "", true, 1, 8));
                tblCommonSection.AddCell(addCellKeyValue("Security: ", "", true, 1, 8));
                tblCommonSection.AddCell(addCellKeyValue("", "_________________________________", true, 1, 8));
                tblCommonSection.AddCell(addCellKeyValue("", "_________________________________", true, 1, 8));
                tblCommonSection.AddCell(addEmptyCell(1, 2, false, 8));

                document.Add(headerTable);
                document.Add(tblCommonSection);
                document.Add(tblSecondSectionHeader);

                document.Close();
                writer.Close();

                using (MemoryStream ms = new MemoryStream())
                {
                    using (FileStream fs = System.IO.File.OpenRead(filapath))
                    {
                        fs.CopyTo(ms);
                    }
                }

                responseClass.isSuccessful = true;
                responseClass.message = absolutePath + docName;

                //ProcessStartInfo info = new ProcessStartInfo();
                //info.Verb = "print";
                //info.FileName = DB.GetAbsoluteUrl("~/Downloads/") + docName;
                //info.CreateNoWindow = true;
                //info.WindowStyle = ProcessWindowStyle.Hidden;

                //Process p = new Process();
                //p.StartInfo = info;
                //p.Start();

                //p.WaitForInputIdle();
                //System.Threading.Thread.Sleep(3000);
                //if (false == p.CloseMainWindow())
                //    p.Kill();

                string results = Newtonsoft.Json.JsonConvert.SerializeObject(responseClass);
                return results;
            }
            catch (Exception ex)
            {
                responseClass.isSuccessful = false;
                responseClass.message = ex.Message;
                string results = Newtonsoft.Json.JsonConvert.SerializeObject(responseClass);
                return results;
            }
        }

        private Cell addCellKeyValue(string key, string value, bool isBorder = false, int colspan = 1, int fontSize = 7, int rowSpan = 1)
        {
            //Cell cell = new Cell(1, colspan).SetBorder(Border.NO_BORDER);
            Cell cell = new Cell(rowSpan, colspan);
            Text t1 = new Text(key).SetFont(font).SetBold().SetFontSize(fontSize);
            Text t2 = new Text(value).SetFont(font).SetFontSize(fontSize);

            Paragraph p = new Paragraph().Add(t1).Add(t2);

            p.SetPadding(1);
            p.SetMargin(1);

            if (isBorder)
                cell.SetBorder(new SolidBorder(0.5f));

            cell.Add(p);
            return cell;
        }

        private Cell addEmptyCell(int rowspan = 1, int colspan = 1, bool isBorder = false, int fontSize = 7)
        {
            //Cell cell = new Cell(1, colspan).SetBorder(Border.NO_BORDER);
            Cell cell = new Cell(1, colspan).SetBorder(Border.NO_BORDER);

            Paragraph p = new Paragraph();

            p.SetPadding(1);
            p.SetMargin(1);

            if (isBorder)
            {
                p.SetBorder(new SolidBorder(0.5f));
                cell.SetBorder(new SolidBorder(0.5f));
            }

            cell.Add(p);
            return cell;
        }
    }
}