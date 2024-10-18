using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VisitorsManagement.Models;
using VisitorsManagement.Models.Contractor;
using VisitorsManagement.Repository;

namespace VisitorsManagement.Controllers
{
    public class ContractorController : Controller
    {
        private readonly IContractorRepository _contractorRepository;
        public ContractorController(IContractorRepository contractorRepository)
        {
            _contractorRepository = contractorRepository;
        }
        // GET: Contractor
        public ActionResult Index(string id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> getContractorSelectList()
        {
            try
            {
                var result = await _contractorRepository.GetContractorsSelectList();
                return Json(result);
            }

            catch (Exception ex)
            {
                DB.insertErrorlog("Contractor", "getContractorSelectList", ex.Message, Convert.ToInt16(Session["UserID"]));
                return Content("Failed : Error Occured");
            }
        }

        [HttpPost]
        public async Task<ActionResult> getAllContractor(ContractorFilter filter)
        {
            try
            {
                var result = await _contractorRepository.GetAllContractors(filter);
                return Json(result);
            }

            catch (Exception ex)
            {
                DB.insertErrorlog("Contractor", "getAllContractor", ex.Message, Convert.ToInt16(Session["UserID"]));
                return Content("Failed : Error Occured");
            }
        }

        [HttpPost]
        public async Task<ActionResult> getAllEmployee(EmployeeFilter filter)
        {
            try
            {
                var result = await _contractorRepository.GetAllEmployee(filter);
                var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }

            catch (Exception ex)
            {
                DB.insertErrorlog("Contractor", "getAllEmployee", ex.Message, Convert.ToInt16(Session["UserID"]));
                return Content("Failed : Error Occured");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateContractor(ContractorMaster contractor)
        {
            ResponseClass clsResponse = new ResponseClass();

            contractor.CreatedBy = Convert.ToInt32(Session["UserID"]);
            contractor.UpdatedBy = Convert.ToInt32(Session["UserID"]);
            contractor.CreatedDate = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");
            contractor.UpdatedDate = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _contractorRepository.CreateContractor(contractor);
                    if (result > 0)
                    {
                        clsResponse.isSuccessful = true;
                        clsResponse.message = "Record Saved Successfully.";
                        clsResponse.PrimaryKeyID = result;
                    }
                    else
                    {
                        clsResponse.isSuccessful = false;
                        clsResponse.message = "Error Occured.";
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
                DB.insertErrorlog("Contractor", "SaveContractor", ex.Message, Convert.ToInt16(Session["UserID"]));
                clsResponse.isSuccessful = false;
                clsResponse.message = "Error Occured.";
            }
            return Json(clsResponse);
        }


        [HttpPost]
        public async Task<ActionResult> CreateEmployee(EmployeeDetails employee)
        {
            ResponseClass clsResponse = new ResponseClass();

            employee.CreatedBy = Convert.ToInt32(Session["UserID"]);
            employee.UpdatedBy = Convert.ToInt32(Session["UserID"]);
            employee.CreatedDate = DateTime.UtcNow.ToShortDateString();
            employee.UpdatedDate = DateTime.UtcNow.ToShortDateString();
            try
            {

                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {

                        HttpPostedFileBase postedFile = Request.Files[i];

                        if (employee.ESICFileName == Request.Files[i].FileName && employee.ESICFile == null)
                        {
                            using (Stream fs1 = Request.Files[i].InputStream)
                            {
                                using (BinaryReader br1 = new BinaryReader(fs1))
                                {
                                    byte[] bytes1 = br1.ReadBytes((Int32)fs1.Length);
                                    if (bytes1.Count() != 0)
                                    {
                                        employee.ESICFile = bytes1;
                                    }
                                }
                            }
                        }
                        if (employee.PFInsuranceFileName == Request.Files[i].FileName && employee.PFInsuranceFile == null)
                        {
                            using (Stream fs = Request.Files[i].InputStream)
                            {
                                using (BinaryReader br = new BinaryReader(fs))
                                {
                                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                    if (bytes.Count() != 0)
                                    {
                                        employee.PFInsuranceFile = bytes;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(employee.ESICFileBase64))
                        employee.ESICFile = Convert.FromBase64String(employee.ESICFileBase64);
                    if (!string.IsNullOrEmpty(employee.PFInsuranceFileBase64))
                        employee.PFInsuranceFile = Convert.FromBase64String(employee.PFInsuranceFileBase64);
                }


                if (ModelState.IsValid)
                {
                    string errorMessage = string.Empty;
                    if (string.IsNullOrEmpty(employee.PFInsuranceFileName) && employee.PFInsuranceFile == null)
                    {
                        clsResponse.isSuccessful = false;
                        errorMessage = "PF Insurance File is required.";
                        return Json(clsResponse);
                    }
                    if (string.IsNullOrEmpty(employee.ESICFileName) && employee.ESICFile == null)
                    {
                        clsResponse.isSuccessful = false;
                        errorMessage = "ESIC File is required.";
                        return Json(clsResponse);
                    }


                    var result = await _contractorRepository.CreateEmployee(employee);
                    if (result > 0)
                    {
                        clsResponse.isSuccessful = true;
                        clsResponse.message = "Record Saved Successfully.";
                    }
                    else
                    {
                        clsResponse.isSuccessful = false;
                        clsResponse.message = "Error Occured.";
                    }
                }
                else
                {
                    clsResponse.isSuccessful = false;
                    clsResponse.message = DB.getModelErrors(ViewData.ModelState.Values);
                }
            }
            catch (Exception ex)
            {
                DB.insertErrorlog("Contractor", "CreateEmployee", ex.Message, Convert.ToInt16(Session["UserID"]));
                clsResponse.isSuccessful = false;
                clsResponse.message = "Error Occured.";
            }
            return Json(clsResponse);
        }


        [HttpPost]
        public async Task<ActionResult> getEmployeeFile(EmployeeFilter filter)
        {
            try
            {
                ResponseClass clsResponse = new ResponseClass();
                clsResponse.isSuccessful = false;
                var result = await _contractorRepository.GetAllEmployee(filter);

                if (result != null)
                {
                    EmployeeDetails empDetails = result.FirstOrDefault();

                    clsResponse.isSuccessful = true;

                    DB.deleteOldUnusedFiles(System.Web.HttpContext.Current.Server.MapPath("~/Downloads/"));
                    if (filter.fileType == "ESIC")
                    {
                        //if (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Downloads/" + empDetails.ESICFileName)))
                        System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath("~/Downloads/" + empDetails.ESICFileName)); // Requires System.IO

                        System.IO.File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Downloads/" + empDetails.ESICFileName), empDetails.ESICFile); // Requires System.IO
                        clsResponse.data = DB.GetAbsoluteUrl("~/Downloads/" + empDetails.ESICFileName);
                        clsResponse.message = empDetails.ESICFileName;
                    }
                    else if (filter.fileType == "PF")
                    {
                        System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath("~/Downloads/" + empDetails.PFInsuranceFileName)); // Requires System.IO
                                                                                                                                               // if (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Downloads/" + empDetails.PFInsuranceFileName)))
                        System.IO.File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Downloads/" + empDetails.PFInsuranceFileName), empDetails.PFInsuranceFile); // Requires System.IO
                        clsResponse.data = DB.GetAbsoluteUrl("~/Downloads/" + empDetails.PFInsuranceFileName);
                        clsResponse.message = empDetails.PFInsuranceFileName;
                    }
                }
                return Json(clsResponse);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<ActionResult> UploadEmployeeDetails(string items, int ContractorId)
        {
            ResponseClass clsResponse = new ResponseClass();

            try
            {

                string reqColumns = "Employee_Name,PF_Insurance_Details,ESIC_Details";

                string Result = null;

                DataTable dtValidate = new DataTable();
                dtValidate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(items);

                string missingColumns = DB.ContainColumn(reqColumns.Split(','), dtValidate);
                if (!string.IsNullOrEmpty(missingColumns))
                {
                    Result = "Missing Columns : " + missingColumns;
                    clsResponse.isSuccessful = false;
                    clsResponse.message = Result;
                    return Json(clsResponse);
                }


                DataTable dtDataTable = new DataTable();
                List<EmployeeUpload> lstEmployeeUpload = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmployeeUpload>>(items);

                dtDataTable = lstEmployeeUpload.ToDataTable<EmployeeUpload>();

                dtDataTable.SetColumnsOrder("Employee_Name", "PF_Insurance_Details", "ESIC_Details");

                Result = ValidateDataTable(dtDataTable, "Employee_Name IS NULL", "Employee_Name");
                if (Result != null)
                {
                    clsResponse.isSuccessful = false;
                    clsResponse.message = Result;
                    return Json(clsResponse);
                }

                Result = ValidateDataTable(dtDataTable, "PF_Insurance_Details IS NULL", "PF_Insurance_Details");
                if (Result != null)
                {
                    clsResponse.isSuccessful = false;
                    clsResponse.message = Result;
                    return Json(clsResponse);
                }

                Result = ValidateDataTable(dtDataTable, "ESIC_Details IS NULL", "ESIC_Details");
                if (Result != null)
                {
                    clsResponse.isSuccessful = false;
                    clsResponse.message = Result;
                    return Json(clsResponse);
                }

                var result = await _contractorRepository.UploadEmployeeDetails(dtDataTable, ContractorId);
                if (result > 0)
                {
                    clsResponse.isSuccessful = true;
                    clsResponse.message = "Employee Details Uploaded Successfully.";
                }
                else
                {
                    clsResponse.isSuccessful = false;
                    clsResponse.message = "Error Occured.";
                }
            }
            catch (Exception ex)
            {
                clsResponse.isSuccessful = false;
                clsResponse.message = ex.Message;
            }
            return Json(clsResponse);
        }

        private string ValidateDataTable(DataTable dtInputDataTable, string sFilterCondition, string sMessageContent)
        {
            DataRow[] drTemp;
            string sTemp = "";
            drTemp = dtInputDataTable.Select(sFilterCondition);
            if (drTemp.Length > 0)
            {
                if (sFilterCondition.Contains("NULL"))
                {
                    return sMessageContent + " field should not be empty.\nPlease resolve it & try again";
                }
                else
                {
                    foreach (DataRow drCurrentRow in drTemp)
                    {
                        bool IsExists = false;
                        if (!string.IsNullOrEmpty(sTemp))
                        {
                            string[] str = sTemp.Split(',');
                            for (int i = 0; i < str.Length; i++)
                            {
                                if (str[i] == Convert.ToString(drCurrentRow[sMessageContent]))
                                {
                                    IsExists = true;
                                    break;
                                }
                            }
                        }

                        if (!IsExists)
                            sTemp += drCurrentRow[sMessageContent] + ", ";
                    }

                    return "Invalid [" + sMessageContent + @"] : " + sTemp.Remove(sTemp.LastIndexOf(", ")) + " .\nPlease resolve it & try again";
                }
            }
            return null;
        }

    }
}