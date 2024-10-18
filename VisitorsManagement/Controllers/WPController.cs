using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VisitorsManagement.App_Start;
using VisitorsManagement.Models;
using VisitorsManagement.Models.Contractor;
using VisitorsManagement.Models.WP;
using VisitorsManagement.Repository;
using static QRCoder.PayloadGenerator;

namespace VisitorsManagement.Controllers
{
    [BasicAuthentication]
    public class WPController : Controller
    {
        // GET: WP
        private readonly IWPRepository _wpRepository;
        private readonly IVisitorsManagementRepository _visitorsManagementRepository;
        private readonly IContractorRepository _contractorRepository;
        public WPController(IWPRepository wpRepository, IVisitorsManagementRepository visitorsManagementRepository,
            IContractorRepository contractorRepository)
        {
            _wpRepository = wpRepository;
            _visitorsManagementRepository = visitorsManagementRepository;
            _contractorRepository = contractorRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> getAllWorkPermit(WPFilter filter)
        {
            try
            {
                var result = await _wpRepository.GetWP(filter);
                return Json(result);
            }
            catch (Exception ex)
            {
                DB.insertErrorlog("WP", "getAllWorkPermit", ex.Message, Convert.ToInt16(Session["UserID"]));
                return Content("Failed : Error Occured");
            }
        }
        [HttpPost]
        public async Task<ActionResult> SendReminder(WPFilter filter)
        {

            ResponseClass clsResponse = new ResponseClass();
            try
            {
                await SendWorkPermitNotification(Convert.ToInt32(filter.WPID), "Reminder");

                clsResponse.isSuccessful = false;
                clsResponse.message = "reminder sent";
                return Json(clsResponse);

            }
            catch (Exception ex)
            {
                DB.insertErrorlog("WP", "SendReminder", ex.Message, Convert.ToInt16(Session["UserID"]));
                return Content("Failed : Error Occured");
            }
        }
        public async Task<ActionResult> Wpreminder()
        {
            _wpRepository.Wpreminder();
            return Content("Failed : Error Occured");
        }


        [HttpPost]
        public async Task<ActionResult> removeEmployee(EmployeeDetails filter)
        {
            ResponseClass clsResponse = new ResponseClass();
            try
            {
                var result = await _wpRepository.removeEmployee(filter.EmployeeId);
                if (result == 1)
                    clsResponse.isSuccessful = true;
                else
                {
                    clsResponse.message = "Error Occured";
                    clsResponse.isSuccessful = false;
                }
            }
            catch (Exception ex)
            {
                clsResponse.isSuccessful = false;
                DB.insertErrorlog("WP", "removeEmployee", ex.Message, Convert.ToInt16(Session["UserID"]));
                return Content("Failed : Error Occured");
            }
            return Json(clsResponse);
        }

        [HttpPost]
        public async Task<ActionResult> closeWorkPermit(WPFilter filter)
        {
            ResponseClass clsResponse = new ResponseClass();
            try
            {
                var result = await _wpRepository.CloseWorkPermit(filter.WPID.Value);
                if (result == 1)
                    clsResponse.isSuccessful = true;
                else
                {
                    clsResponse.message = "Error Occured";
                    clsResponse.isSuccessful = false;
                }
            }
            catch (Exception ex)
            {
                clsResponse.isSuccessful = false;
                DB.insertErrorlog("WP", "closeWorkPermit", ex.Message, Convert.ToInt16(Session["UserID"]));
                return Content("Failed : Error Occured");
            }
            return Json(clsResponse);
        }

        [HttpPost]
        public async Task<ActionResult> ApproveRejectWP(ApproveRejectWP approveRejectWP)
        {
            ResponseClass clsResponse = new ResponseClass();
            approveRejectWP.ApprovedId = Convert.ToInt32(Session["UserID"]);
            var result = await _wpRepository.ApproveRejectWP(approveRejectWP);

            if (result > 0)
            {
                clsResponse.isSuccessful = true;
                await SendWorkPermitNotification(approveRejectWP.WPID);

            }
            else
                clsResponse.isSuccessful = false;


            return Json(clsResponse);
        }

        //[HttpPost]
        //public async Task<ActionResult> SaveWorkPermit(WorkPermit wp)
        //{
        //    return Json("");
        //}

        [HttpPost]
        public async Task<ActionResult> SaveWorkPermit(WP wp)
        {
            ResponseClass clsResponse = new ResponseClass();
            try
            {

                wp.CreatedBy = Convert.ToString(Session["UserID"]);
                wp.CreatedDate = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");
                wp.UpdatedBy = Convert.ToString(Session["UserID"]);
                wp.UpdatedDate = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");

                if (wp.WPID == 0)
                    wp.InitiatedById = Convert.ToInt32(Session["UserID"]);

                if (wp.SelectedEmployees == null)
                {
                    clsResponse.isSuccessful = false;
                    clsResponse.message = "Please add atleast one employee details.";
                    return Json(clsResponse);
                }
                if (wp.SelectedEmployees == "")
                {
                    clsResponse.isSuccessful = false;
                    clsResponse.message = "Please add atleast one employee details.";
                    return Json(clsResponse);

                }

                if (ModelState.IsValid)
                {
                    var result = await _wpRepository.CreateWP(wp);
                    if (result > 0)
                    {
                        clsResponse.PrimaryKeyID = result;
                        clsResponse.isSuccessful = true;
                        if (wp.IsSubmitted)
                            await SendWorkPermitNotification(clsResponse.PrimaryKeyID);
                    }
                    else
                        clsResponse.isSuccessful = false;
                }
                else
                {
                    clsResponse.isSuccessful = false;
                    clsResponse.message = DB.getModelErrors(ViewData.ModelState.Values);
                }
            }
            catch (Exception ex)
            {
                DB.insertErrorlog("WP", "getAllWorkPermit", ex.Message, Convert.ToInt16(Session["UserID"]));
                clsResponse.isSuccessful = false;
                clsResponse.message = ex.Message;
            }
            return Json(clsResponse);
        }

        [HttpPost]
        public async Task<ActionResult> getWPFile(EmployeeFilter details)
        {
            ResponseClass clsResponse = new ResponseClass();
            clsResponse.isSuccessful = false;
            var result = await _wpRepository.getEmployeeFile(details);

            if (result != null)
            {
                clsResponse.isSuccessful = true;

                DB.deleteOldUnusedFiles(System.Web.HttpContext.Current.Server.MapPath("~/Downloads/"));
                if (details.fileType == "ESIC")
                {
                    if (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Downloads/" + result.ESICFileName)))
                        System.IO.File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Downloads/" + result.ESICFileName), result.ESICFile); // Requires System.IO
                    clsResponse.data = DB.GetAbsoluteUrl("~/Downloads/" + result.ESICFileName);
                    clsResponse.message = result.ESICFileName;
                }
                else if (details.fileType == "PF")
                {
                    if (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Downloads/" + result.PFInsuranceFileName)))
                        System.IO.File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Downloads/" + result.PFInsuranceFileName), result.PFInsuranceFile); // Requires System.IO
                    clsResponse.data = DB.GetAbsoluteUrl("~/Downloads/" + result.PFInsuranceFileName);
                    clsResponse.message = result.PFInsuranceFileName;
                }
            }
            return Json(clsResponse);
        }

        [HttpPost]
        public async Task<ActionResult> SaveSafetyTraining(SafetyTrainingModel safetyTraining)
        {
            ResponseClass clsResponse = new ResponseClass();

            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _wpRepository.SaveSafetyTraining(safetyTraining);
                    if (result == 1)
                    {
                        clsResponse.isSuccessful = true;
                       // await SendWorkPermitNotification(safetyTraining.WPID.Value);
                    }
                    else
                        clsResponse.isSuccessful = false;
                }
                else
                {
                    clsResponse.isSuccessful = false;
                    clsResponse.message = DB.getModelErrors(ViewData.ModelState.Values);
                }

            }
            catch (Exception ex)
            {
                DB.insertErrorlog("WP", "SaveSafetyTraining", ex.Message, Convert.ToInt16(Session["UserID"]));
                clsResponse.isSuccessful = false;
                clsResponse.message = ex.Message;
            }
            return Json(clsResponse);
        }


        private async Task SendWorkPermitNotification(int WPID, string RequestFor = null)
        {
            WPFilter filter = new WPFilter();
            filter.WPID = WPID;
            var result = await _wpRepository.GetWP(filter);

            WP workPermit = result.FirstOrDefault();

            string ContractorEmail = workPermit.ContractorEmailId;




            string WPNO = workPermit.WPNO;
            string WPDate = workPermit.strWPDate;
            string WPType = workPermit.WPType;
            string WorkStartDate = workPermit.strWorkStartDate;
            string WorkEndDate = workPermit.strWorkEndDate;
            string InitiatedBy = workPermit.InitiatedByName;
            string strStatus = workPermit.Status;
            string applicationURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

            var nameAndEmail_Initiator_Task = _visitorsManagementRepository.GetVisitingPersonEmail(workPermit.InitiatedById.Value);
            var nameAndEmail_HR_Task = _visitorsManagementRepository.GetVisitingPersonEmail(workPermit.HRId);
            var nameAndEmail_IMS_Task = _visitorsManagementRepository.GetVisitingPersonEmail(workPermit.IMSId);
            var nameAndEmail_Final_Task = _visitorsManagementRepository.GetVisitingPersonEmail(workPermit.FinalId);

            await Task.WhenAll(
                nameAndEmail_Initiator_Task,
                nameAndEmail_HR_Task,
                nameAndEmail_IMS_Task,
                nameAndEmail_Final_Task
                ).ConfigureAwait(false);

            var nameAndEmail_Initiator = nameAndEmail_Initiator_Task.Result;
            var nameAndEmail_HR = nameAndEmail_HR_Task.Result;
            var nameAndEmail_IMS = nameAndEmail_IMS_Task.Result;
            var nameAndEmail_Final = nameAndEmail_Final_Task.Result;

            try
            {
                string body = string.Empty;
                string file = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, "EmailTemplate/WorkPermitTemplate.html");
                using (StreamReader reader = new StreamReader(file))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{{WPNO}}", WPNO);
                body = body.Replace("{{WPDate}}", WPDate);
                body = body.Replace("{{WPType}}", WPType);
                body = body.Replace("{{WorkStartDate}}", WorkStartDate);
                body = body.Replace("{{WorkEndDate}}", WorkEndDate);
                body = body.Replace("{{InitiatedBy}}", InitiatedBy);
                body = body.Replace("{{strStatus}}", strStatus);
                body = body.Replace("{{applicationURL}}", applicationURL);


                string sendTo = $@"{nameAndEmail_Initiator.Item2},{nameAndEmail_HR.Item2},{nameAndEmail_IMS.Item2},{nameAndEmail_Final.Item2}";
                if ((workPermit.Status == "Waiting for Manager Approval" && RequestFor == "Reminder") || workPermit.Status == "Waiting for Manager Approval")
                {
                    sendTo = $@"{nameAndEmail_Final.Item2}";
                }
                else if ((workPermit.Status == "Waiting for HR Action" && RequestFor == "Reminder") || workPermit.Status == "Waiting for HR Action")
                {
                    sendTo = $@"{nameAndEmail_HR.Item2}";
                }
                else if ((workPermit.Status == "Waiting for Safety Training" && RequestFor == "Reminder") || workPermit.Status == "Waiting for Safety Training")
                {
                    sendTo = $@"{nameAndEmail_Initiator.Item2}";
                }
                else if ((workPermit.Status == "Waiting for IMS Approval" && RequestFor == "Reminder") || workPermit.Status == "Waiting for IMS Approval" || workPermit.Status == "Waiting for Safety Training & EHS Approval")
                {
                    sendTo = $@"{nameAndEmail_IMS.Item2}";
                }


                if (workPermit.Status == "Approved")
                    sendTo = sendTo + "," + ContractorEmail;

                DB.SendMailAsync(sendTo, body, $"Work Permit :{WPNO} updated status :{ strStatus }");

                if (workPermit.Status == "Approved")
                {
                    await SendVisitorPassNotification(WPID).ConfigureAwait(false);
                    //await Task.Delay(60000).ContinueWith(t => SendVisitorPassNotification(WPID));
                }
            }
            catch (Exception ex)
            {
                DB.insertErrorlog("WP", "SendWorkPermitNotification", ex.Message, 0, ex.StackTrace);
                // throw;
            }



        }

        private async Task SendVisitorPassNotification(int WPID)
        {
            VM vmmodel = await _visitorsManagementRepository.GetVisitorByWPID(WPID);

            string VisitingPersonName = vmmodel.PersonToVisitName;
            string VisitorsName = vmmodel.VisitorName;
            string MobileNo = vmmodel.VisitorPhoneNumber;
            string DateDetail = Convert.ToDateTime(vmmodel.Date).ToString("dd-MMM-yyyy");
            string RepresentingCompany = vmmodel.RepresentingCompany;
            string PurposeOfVisit = vmmodel.PurposeToVisit;
            string AppointmentNo = vmmodel.AppointmentNo;
            string applicationURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, "EmailTemplate/AppiontmentRaised.html")))
            {
                body = reader.ReadToEnd();
            }
           
            body = body.Replace("{{VisitingPersonName}}", VisitingPersonName);
            body = body.Replace("{{VisitorsName}}", VisitorsName);
            body = body.Replace("{{DateDetail}}", DateDetail);
            body = body.Replace("{{RemarkDetail}}", vmmodel.Remark);
            body = body.Replace("{{CompanyName}}", RepresentingCompany);
            body = body.Replace("{{PurposeOfVisit}}", PurposeOfVisit);
            body = body.Replace("{{VisitTo}}", vmmodel.PersonToVisitName);
            body = body.Replace("{{AppointmentNo}}", AppointmentNo);
            body = body.Replace("{{ApplicationURL}}", applicationURL);
            string checkInUrl = applicationURL + @"/VM/CheckIn?appointmentId=" + DB.encrypt(vmmodel.AppointmentID.ToString());

            Url generator = new Url(checkInUrl);
            string payload = generator.ToString();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeAsBitmap = qrCode.GetGraphic(20);

            var qrBytes = DB.ImageToByte(qrCodeAsBitmap);

            string fileName = DB.RandomString(5, false);

            using (System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(qrBytes)))
            {
                image.Save(Server.MapPath("~/Downloads/") + fileName + ".jpg", ImageFormat.Jpeg);  // Or Png
                image.Dispose();
            }

            string folderPath = Server.MapPath("~/Downloads/");
            DB.deleteOldUnusedFiles(folderPath);

            DB.SendMailAsync(vmmodel.VisitorsEmails + "," + vmmodel.PersonToVisitEmailID, body, "Visitor's appointment raised for " + vmmodel.VisitorName, "Downloads/" + fileName + ".jpg");

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
        public async Task<string> downloadReport(int WPID)
        {
            ResponseClass responseClass = new ResponseClass();

            try
            {
                var language = "";

                var downloadbasepath = Server.MapPath("\\Downloads\\");

                deleteFilesfromDownloads();
                var basepath = Server.MapPath("\\");

                WPFilter filter = new WPFilter() { WPID = WPID };
                var result = await _wpRepository.GetWP(filter).ConfigureAwait(false);
                WP workPermit = new WP();
                if (result == null)
                {
                    responseClass.isSuccessful = false;
                    responseClass.message = "No Data Found.";
                }
                workPermit = result.FirstOrDefault();
                string tempFilename = "Work Permit - " + workPermit.WPNO + ".pdf";

                var fontFilePath = "";
                language = "English";

                var docName = tempFilename;

                var filapath = downloadbasepath + tempFilename;

                var absolutePath = DB.GetAbsoluteUrl("Downloads/");

                fontFilePath = Server.MapPath("~/fonts/arial.ttf");

                font = PdfFontFactory.CreateFont(fontFilePath, PdfEncodings.IDENTITY_H, true);

                PdfWriter writer = new PdfWriter(filapath);
                iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer);

                Document document = new Document(pdf, PageSize.A4);


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


                Image image = new Image(imageData).ScaleAbsolute(90, 30);

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
                title.Add(new Paragraph("Work Permit" + " ( " + workPermit.Status + " )").SetFont(font).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)).SetFontSize(12).SetBold().SetBorderTop(Border.NO_BORDER).SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 0.5f)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 0.5f)).SetBorderBottom(new SolidBorder(ColorConstants.BLACK, 0.5f));
                title.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                title.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
                title.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                headerTable.AddCell(title);
                #endregion

                float[] ColstblCommonSection = { 1, 1 };
                Table tblCommonSection = new Table(ColstblCommonSection, true);

                tblCommonSection.AddCell(addCellKeyValue("Work Permit No: ", workPermit.WPNO, true, 1, 8));
                tblCommonSection.AddCell(addCellKeyValue("Work Permit Date: ", workPermit.strWPDate, true, 1, 8));
                tblCommonSection.AddCell(addCellKeyValue("Work Permit Type: ", workPermit.WPType, true, 2, 8));
                tblCommonSection.AddCell(addCellKeyValue("Work Start Date: ", workPermit.strWorkStartDate, true, 1, 8));
                tblCommonSection.AddCell(addCellKeyValue("Work End Date: ", workPermit.strWorkEndDate, true, 1, 8));
                tblCommonSection.AddCell(addCellKeyValue("Initiated By: ", workPermit.InitiatedByName, true, 2, 8));
                tblCommonSection.AddCell(addCellKeyValue("Nature of Work: ", workPermit.NatureOfWork, true, 2, 8));
                tblCommonSection.AddCell(addEmptyCell(1, 2, false, 8));

                //Cell cellBlank = new Cell(1, 2);
                Table tblSecondSectionHeader = new Table(ColstblCommonSection, true);
                Cell cellSecondSectionH1 = new Cell(1, 2).Add(new Paragraph("Contractor Details").SetPadding(1).SetFont(font).SetMargin(1).SetBold().SetFontSize(9));
                tblSecondSectionHeader.AddCell(cellSecondSectionH1);


                Table tblSecondSectionDetails = new Table(ColstblCommonSection, true);
                tblSecondSectionHeader.AddCell(addCellKeyValue("Contractor Name: ", workPermit.ContractorName, true, 1, 8));
                tblSecondSectionHeader.AddCell(addCellKeyValue("License Details: ", workPermit.LicenseDetails, true, 1, 8));
                tblSecondSectionHeader.AddCell(addEmptyCell(1, 2, false, 8));

                float[] ColstblEmployeeSection = { 0.8F, 1.2F, 1.0F, 1.0F };
                Table tblEmployeeSection = new Table(ColstblEmployeeSection, true);
                tblEmployeeSection.AddCell(addCellKeyValue("Employee Details: ", "", true, 4, 10));
                tblEmployeeSection.AddCell(addCellKeyValue("Sr. No ", "", true, 1, 8));
                tblEmployeeSection.AddCell(addCellKeyValue("Employee Name ", "", true, 1, 8));
                tblEmployeeSection.AddCell(addCellKeyValue("PF Details ", "", true, 1, 8));
                tblEmployeeSection.AddCell(addCellKeyValue("ESIC Details ", "", true, 1, 8));

                for (int i = 0; i < workPermit.listEmployee.Count; i++)
                {
                    tblEmployeeSection.AddCell(addCellKeyValue("", workPermit.listEmployee[i].SrNo, true, 1, 8));
                    tblEmployeeSection.AddCell(addCellKeyValue("", workPermit.listEmployee[i].EmployeeName, true, 1, 8));
                    tblEmployeeSection.AddCell(addCellKeyValue("", workPermit.listEmployee[i].PFInsuranceDetails, true, 1, 8));
                    tblEmployeeSection.AddCell(addCellKeyValue("", workPermit.listEmployee[i].ESICDetails, true, 1, 8));
                }
                tblEmployeeSection.AddCell(addEmptyCell(1, 2, false, 8));
                #region Section2

                Table tblThirdSectionHeader = new Table(new float[] { 1 }, true);
                Cell cellThirdSectionH1 = new Cell().Add(new Paragraph("Safety Training Details: ").SetMargin(1).SetPadding(1).SetFont(font).SetFontSize(10).SetBold());

                tblThirdSectionHeader.AddCell(cellThirdSectionH1);


                Table tblThirdSectionDetails = new Table(new float[] { 1, 1 }, true);
                tblThirdSectionDetails.AddCell(addCellKeyValue("Training On: ", workPermit.SafetyTraining, true, 1, 8));
                tblThirdSectionDetails.AddCell(addCellKeyValue("Training Date: ", workPermit.TrainedDate, true, 1, 8));


                tblThirdSectionDetails.AddCell(addEmptyCell(1, 2, false, 8));
                #endregion

                float[] ColstblApproverSection = { 1, 1, 1 };
                Table tblApproverSection = new Table(ColstblApproverSection, true);

                Cell cellapprover = new Cell(1, 3).Add(new Paragraph("Approval Details: ").SetMargin(1).SetPadding(1).SetFont(font).SetFontSize(10).SetBold());

                tblApproverSection.AddCell(cellapprover);
                tblApproverSection.AddCell(addCellKeyValue("Manager Approval: ", workPermit.FinalApproverName, true, 1, 8));
                tblApproverSection.AddCell(addCellKeyValue("HR Approval: ", workPermit.HRApproverName, true, 1, 8));
                tblApproverSection.AddCell(addCellKeyValue("EHS Approval: ", workPermit.IMSApproverName, true, 1, 8));
                tblApproverSection.AddCell(addEmptyCell(1, 3, false, 8));

                float[] ColstblSignatureSection = { 1, 1 };
                Table tblSignatureSection = new Table(ColstblSignatureSection, true);
                tblSignatureSection.AddCell(addCellKeyValue("Contractor Signature: ", "", true, 1, 8));
                tblSignatureSection.AddCell(addCellKeyValue("Closed by Signature(Initiator): ", "", true, 1, 8));
                tblSignatureSection.AddCell(addCellKeyValue("", "_________________________________", true, 1, 8));
                tblSignatureSection.AddCell(addCellKeyValue("", "_________________________________", true, 1, 8));
                tblSignatureSection.AddCell(addEmptyCell(1, 3, false, 8));


                document.Add(headerTable);
                document.Add(tblCommonSection);
                document.Add(tblSecondSectionHeader);
                document.Add(tblEmployeeSection);
                document.Add(tblSecondSectionDetails);
                document.Add(tblThirdSectionHeader);
                document.Add(tblThirdSectionDetails);
                document.Add(tblApproverSection);
                document.Add(tblSignatureSection);


                document.Close();
                writer.Close();

                using (MemoryStream ms = new MemoryStream())
                {
                    using (FileStream fs = System.IO.File.OpenRead(filapath))
                    {
                        fs.CopyTo(ms);
                    }

                    //Response.ContentType = "pdf/application";
                    //Response.AddHeader("content-disposition", $"attachment;filename={ System.IO.Path.GetFileName(filapath) }");
                    //Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                }


                //var cd = new System.Net.Mime.ContentDisposition
                //{
                //    // for example foo.bak
                //    FileName = absolutePath + docName,

                //    // always prompt the user for downloading, set to true if you want 
                //    // the browser to try to show the file inline
                //    Inline = false,
                //};
                //Response.AppendHeader("Content-Disposition", cd.ToString());

                //Response.Flush();
                //Response.End();
                //Response.Close();

                //ManipulatePdf(filapath, downloadbasepath + docName);
                responseClass.isSuccessful = true;
                responseClass.message = absolutePath + docName;

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

        //[HttpPost]
        //public async Task<ActionResult> getWorkPermitType()
        //{
        //    try
        //    {
        //        var result = await _wpRepository.GetWP(filter);
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        DB.insertErrorlog("WP", "getAllWorkPermit", ex.Message, Convert.ToInt16(Session["UserID"]));
        //        return Content("Failed : Error Occured");
        //    }
        //}
    }
}