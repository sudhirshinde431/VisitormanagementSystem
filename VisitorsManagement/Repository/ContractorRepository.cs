using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VisitorsManagement.Models;
using VisitorsManagement.Models.Contractor;

namespace VisitorsManagement.Repository
{
    public class ContractorRepository : IContractorRepository
    {
        private readonly IGenericRepository _genericRepository;

        public ContractorRepository(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<IEnumerable<DropDownModel>> GetContractorsSelectList()
        {
            var sQuery = @"SELECT Name 'Text',ContractorId 'ValueInt',LicenseDetails 'Value' FROM tbl_ContractorMaster";

            var result = await _genericRepository.GetAsync<DropDownModel>(sQuery, null);

            return result;
        }

        public async Task<IEnumerable<ContractorMaster>> GetAllContractors(ContractorFilter filter)
        {
            var sQuery = @"SELECT ContractorId,Name,LicenseDetails,Replace(convert(char(11),IssuiedOn,106),' ', '-') 'IssuiedOn',Replace(convert(char(11),ValidTill,106),' ', '-') 'ValidTill',ContactPersonName,ContactPersonMobileNo,IsActive,EmailId,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate FROM [dbo].[tbl_ContractorMaster] ";

            if (filter.ContractorId > 0)
                sQuery = sQuery + $" WHERE ContractorId = { filter.ContractorId }";
            else if (!string.IsNullOrEmpty(filter.FilterText))
                sQuery = sQuery + $" WHERE Name LIKE '%{ filter.FilterText }%' OR ContactPersonName LIKE '%{ filter.FilterText }%' OR ContactPersonMobileNo LIKE '%{ filter.FilterText }%'";


            sQuery = sQuery + " ORDER BY ContractorId DESC";

            var result = await _genericRepository.GetAsync<ContractorMaster>(sQuery, null);

            return result;
        }

        public async Task<int> CreateContractor(ContractorMaster contractor)
        {
            IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString); conn.Open();

            using (var transction = conn.BeginTransaction())
            {
                try
                {
                    var sQuery = @"";
                    int contractorId = 0;
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Name", contractor.Name);
                    param.Add("@LicenseDetails", contractor.LicenseDetails);
                    param.Add("@EmailId", contractor.EmailId);
                    param.Add("@IssuiedOn", contractor.IssuiedOn);
                    param.Add("@ValidTill", contractor.ValidTill);
                    param.Add("@ContactPersonName", contractor.ContactPersonName);
                    param.Add("@ContactPersonMobileNo", contractor.ContactPersonMobileNo);
                    param.Add("@UpdatedBy", contractor.UpdatedBy);
                    param.Add("@UpdatedDate", contractor.UpdatedDate);

                    if (contractor.ContractorId > 0)
                    {
                        contractorId = contractor.ContractorId;
                        param.Add("@ContractorId", contractor.ContractorId);
                        sQuery = $@"UPDATE [dbo].[tbl_ContractorMaster]
                                   SET [Name] = @Name
                                      ,[LicenseDetails] = @LicenseDetails
                                      ,[EmailId] = @EmailId 
                                      ,[IssuiedOn] = @IssuiedOn
                                      ,[ValidTill] = @ValidTill
                                      ,[ContactPersonName] = @ContactPersonName
                                      ,[ContactPersonMobileNo] = @ContactPersonMobileNo
                                      ,[UpdatedBy] = @UpdatedBy
                                      ,[UpdatedDate] = @UpdatedDate
                                 WHERE ContractorId = @ContractorId";

                        await conn.ExecuteAsync(sQuery, param, transction, 180).ConfigureAwait(false);
                    }
                    else
                    {
                        param.Add("@CreatedBy", contractor.CreatedBy);
                        param.Add("@CreatedDate", contractor.CreatedDate);
                        sQuery = $@"INSERT INTO [dbo].[tbl_ContractorMaster]
                                   ([Name],[LicenseDetails],[EmailId],[IssuiedOn],[ValidTill],[ContactPersonName]
                                   ,[ContactPersonMobileNo],[CreatedBy],[CreatedDate])
                                    VALUES
                                   (@Name,@LicenseDetails,@EmailId,@IssuiedOn,@ValidTill,@ContactPersonName
                                   ,@ContactPersonMobileNo,@CreatedBy,@CreatedDate) SELECT CAST(SCOPE_IDENTITY() as int);";

                        contractorId = conn.QuerySingle<int>(sQuery, param, transction, 180);
                    }
                    //if (contractor.EmployeeList != null)
                    //{
                    //    for (int i = 0; i < contractor.EmployeeList.Count; i++)
                    //    {
                    //        param = new DynamicParameters();

                    //        param.Add("@EmployeeName", contractor.EmployeeList[i].EmployeeName);
                    //        param.Add("@PFInsuranceDetails", contractor.EmployeeList[i].PFInsuranceDetails);
                    //        param.Add("@PFInsuranceFile", contractor.EmployeeList[i].PFInsuranceFile);
                    //        param.Add("@ESICDetails", contractor.EmployeeList[i].ESICDetails);
                    //        param.Add("@ESICFile", contractor.EmployeeList[i].ESICFile);
                    //        param.Add("@IsActive", contractor.EmployeeList[i].IsActive);

                    //        if (contractor.EmployeeList[i].EmployeeId > 0)
                    //        {
                    //            sQuery = $@"UPDATE [dbo].[tbl_EmployeDetails]
                    //                       SET 
                    //                          ,[EmployeeName] = @EmployeeName
                    //                          ,[PFInsuranceDetails] = @PFInsuranceDetails
                    //                          ,[PFInsuranceFile] = @PFInsuranceFile
                    //                          ,[ESICDetails] = @ESICDetails
                    //                          ,[ESICFile] = @ESICFile
                    //                          ,[IsActive] = @IsActive  
                    //                     WHERE EmployeeId = @EmployeeId";
                    //            param.Add("@EmployeeId", contractor.EmployeeList[i].EmployeeId);
                    //            await conn.ExecuteAsync(sQuery, param, transction, 180).ConfigureAwait(false);
                    //        }
                    //        else
                    //        {
                    //            param.Add("@ContractorId", contractorId);
                    //            sQuery = @"INSERT INTO [dbo].[tbl_EmployeDetails]
                    //                       ([ContractorId]
                    //                       ,[EmployeeName]
                    //                       ,[PFInsuranceDetails]
                    //                       ,[PFInsuranceFile]
                    //                       ,[ESICDetails]
                    //                       ,[ESICFile])
                    //                         VALUES
                    //                       (@ContractorId
                    //                       ,@EmployeeName
                    //                       ,@PFInsuranceDetails
                    //                       ,@PFInsuranceFile
                    //                       ,@ESICDetails
                    //                       ,@ESICFile)";
                    //            await conn.ExecuteAsync(sQuery, param, transction, 180).ConfigureAwait(false);
                    //        }
                    //    }
                    //}
                    transction.Commit();
                    return contractorId;
                }
                catch (Exception ex)
                {
                    transction.Rollback();
                    conn.Close();
                    return 0;
                }
            }
        }

        public async Task<int> CreateEmployee(EmployeeDetails employee)
        {
            var sQuery = @"";
            var param = new DynamicParameters();

            param.Add("@EmployeeName", employee.EmployeeName);
            param.Add("@PFInsuranceDetails", employee.PFInsuranceDetails);
            param.Add("@ESICDetails", employee.ESICDetails);

            if (employee.EmployeeId > 0)
            {
                if (employee.PFInsuranceFile != null)
                {
                    param.Add("@PFInsuranceFile", employee.PFInsuranceFile);
                    param.Add("@PFInsuranceFileName", employee.PFInsuranceFileName);
                }
                param.Add("@ESICDetails", employee.ESICDetails);
                if (employee.ESICFile != null)
                {
                    param.Add("@ESICFile", employee.ESICFile);
                    param.Add("@ESICFileName", employee.ESICFileName);
                }

                sQuery = $@"UPDATE [dbo].[tbl_EmployeDetails]
                                   SET 
                                      [EmployeeName] = @EmployeeName
                                      ,[PFInsuranceDetails] = @PFInsuranceDetails
                                      ,[ESICDetails] = @ESICDetails  ";
                if (employee.PFInsuranceFile != null)
                {
                    sQuery += @",[PFInsuranceFile] = @PFInsuranceFile
                                ,[PFInsuranceFileName] = @PFInsuranceFileName";
                }
                if (employee.ESICFile != null)
                {
                    sQuery += @",[ESICFile] = @ESICFile
                                ,[ESICFileName] = @ESICFileName";
                }
                sQuery += @",[IsActive] = @IsActive  
                                 WHERE EmployeeId = @EmployeeId";
                param.Add("@EmployeeId", employee.EmployeeId);
                param.Add("@IsActive", employee.IsActive);
                param.Add("@UpdatedBy", employee.UpdatedBy);
                param.Add("@UpdatedDate", employee.UpdatedDate);
            }
            else
            {

                param.Add("@PFInsuranceFile", employee.PFInsuranceFile);
                param.Add("@PFInsuranceFileName", employee.PFInsuranceFileName);

                param.Add("@ESICFile", employee.ESICFile);
                param.Add("@ESICFileName", employee.ESICFileName);

                param.Add("@ContractorId", employee.ContractorId);
                param.Add("@CreatedBy", employee.CreatedBy);
                param.Add("@CreatedDate", employee.CreatedDate);
                sQuery = @"INSERT INTO [dbo].[tbl_EmployeDetails]
                                   ([ContractorId]
                                   ,[EmployeeName]
                                   ,[PFInsuranceDetails]
                                   ,[PFInsuranceFile]
                                   ,[PFInsuranceFileName]
                                   ,[ESICDetails]
                                   ,[ESICFile]
                                   ,[ESICFileName])
                                     VALUES
                                   (@ContractorId
                                   ,@EmployeeName
                                   ,@PFInsuranceDetails
                                   ,@PFInsuranceFile
                                   ,@PFInsuranceFileName
                                   ,@ESICDetails
                                   ,@ESICFile
                                   ,@ESICFileName)";
            }

            var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);

            return result;
        }

        public async Task<IEnumerable<EmployeeDetails>> GetAllEmployee(EmployeeFilter filter)
        {
            var sQuery = @"SELECT ROW_NUMBER() OVER(ORDER BY EmployeeId ASC) AS SrNo,EmployeeId,ContractorId,EmployeeName,PFInsuranceDetails,PFInsuranceFile,PFInsuranceFileName,ESICDetails,ESICFile,ESICFileName,IsActive,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate FROM [dbo].[tbl_EmployeDetails] ";

            sQuery = sQuery + $" WHERE ContractorId = { filter.ContractorId }";

            if (!string.IsNullOrEmpty(filter.EmployeeId))
                sQuery = sQuery + $" AND EmployeeId IN ({ filter.EmployeeId })";
            else if (!string.IsNullOrEmpty(filter.FilterText))
                sQuery = sQuery + $" AND Name LIKE '%{ filter.FilterText }%' OR ContactPersonName LIKE '%{ filter.FilterText }%' OR ContactPersonMobileNo LIKE '%{ filter.FilterText }%'";

            sQuery = sQuery + " ORDER BY ContractorId DESC";

            var result = await _genericRepository.GetAsync<EmployeeDetails>(sQuery, null);

            //if (filter.EmployeeId > 0)
            //{
            //    foreach (var item in result)
            //    {
            //        item.PFInsuranceFile = null;
            //        item.ESICFile = null;
            //        //if (item.PFInsuranceFile != null)
            //        //    item.PFInsuranceFileBase64 = Convert.ToBase64String(item.PFInsuranceFile);

            //        //if (item.ESICFile != null)
            //        //    item.ESICFileBase64 = Convert.ToBase64String(item.ESICFile);
            //    }
            //}

            return result;
        }

        public async Task<int> UploadEmployeeDetails(DataTable dt, int ContractorId)
        {
            IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
            conn.Open();

            using (var transction = conn.BeginTransaction())
            {
                try
                {
                    var sQuery = @"";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var param = new DynamicParameters();

                        param.Add("@EmployeeName",Convert.ToString(dt.Rows[i]["Employee_Name"]));
                        param.Add("@PFInsuranceDetails", Convert.ToString(dt.Rows[i]["PF_Insurance_Details"]));
                        param.Add("@ESICDetails", Convert.ToString(dt.Rows[i]["ESIC_Details"]));
                        param.Add("@ContractorId", ContractorId);

                        sQuery = $@"IF NOT EXISTS(SELECT * FROM tbl_EmployeDetails WHERE [ContractorId] = @ContractorId AND [EmployeeName] = @EmployeeName AND [PFInsuranceDetails] = @PFInsuranceDetails AND [ESICDetails]=@ESICDetails)
                                    BEGIN
                                    INSERT INTO [dbo].[tbl_EmployeDetails]
                                   ([ContractorId],[EmployeeName],[PFInsuranceDetails],[ESICDetails])
                                     VALUES
                                   (@ContractorId,@EmployeeName,@PFInsuranceDetails,@ESICDetails)
                                    END";
                        await conn.ExecuteAsync(sQuery, param, transction, 180).ConfigureAwait(false);
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

        //public async Task<EmployeeDetails> getEmployeeFile(EmployeeDetails details)
        //{
        //    var sQuery = $@"SELECT EmployeeId,WPID,EmployeeName,PFInsuranceDetails,PFInsuranceFile,PFInsuranceFileName,ESICDetails,ESICFile,ESICFileName FROM tbl_EmployeDetails WHERE EmployeeId= @EmployeeId";

        //    DynamicParameters param = new DynamicParameters();
        //    param.Add("@EmployeeId", details.EmployeeId);
        //    try
        //    {
        //        var result = await _genericRepository.GetAsync<EmployeeDetails>(sQuery, param);

        //        return result.FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}
    }
}