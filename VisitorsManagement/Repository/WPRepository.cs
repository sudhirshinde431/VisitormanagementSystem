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
using VisitorsManagement.Models;
using VisitorsManagement.Models.Contractor;
using VisitorsManagement.Models.WP;

namespace VisitorsManagement.Repository
{
    public class WPRepository : IWPRepository
    {

        private readonly IGenericRepository _genericRepository;
        private readonly IContractorRepository _contractorRepository;
        public WPRepository(IGenericRepository genericRepository, IContractorRepository contractorRepository)
        {
            _genericRepository = genericRepository;
            _contractorRepository = contractorRepository;
        }

        public async Task<int> ApproveRejectWP(ApproveRejectWP approveRejectWP)
        {
            var sQuery = @"";

            try
            {
                if (approveRejectWP.Approver == "HR")
                {
                    sQuery = @"UPDATE [dbo].[tbl_WP] SET HRApproval = @HRApproval,HRComment = @HRComment,Status=@Status ,HRId=@HRId,HRApprovedDate=@HRApprovedDate WHERE WPID = @WPID";
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@HRApproval", approveRejectWP.ApproveReject);
                    param.Add("@HRComment", approveRejectWP.Comment);
                    param.Add("@HRId", approveRejectWP.ApprovedId);
                    param.Add("@Status", approveRejectWP.Status);
                    param.Add("@HRApprovedDate", DateTime.Now.Date);
                    param.Add("@WPID", approveRejectWP.WPID);

                    var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);
                    return result;
                }
                else if (approveRejectWP.Approver == "Manager")
                {
                    sQuery = @"UPDATE [dbo].[tbl_WP] SET FinalApproval = @FinalApproval,FinalComment = @FinalComment,Status=@Status ,FinalId=@FinalId,FinalApprovedDate=@FinalApprovedDate WHERE WPID = @WPID";
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@FinalApproval", approveRejectWP.ApproveReject);
                    param.Add("@FinalComment", approveRejectWP.Comment);
                    param.Add("@FinalId", approveRejectWP.ApprovedId);
                    param.Add("@Status", approveRejectWP.Status);
                    param.Add("@FinalApprovedDate", DateTime.Now.Date);
                    param.Add("@WPID", approveRejectWP.WPID);

                    var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);

                    if (result > 0 && approveRejectWP.Status == "Approved")
                        await generateVMPass(approveRejectWP.WPID);

                    return result;
                  
                }
                else if (approveRejectWP.Approver == "IMS")
                {
                    sQuery = @"UPDATE [dbo].[tbl_WP] SET IMSApproval = @IMSApproval,IMSComment = @IMSComment,Status=@Status ,IMSId=@IMSId,IMSApprovedDate=@IMSApprovedDate WHERE WPID = @WPID";
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@IMSApproval", approveRejectWP.ApproveReject);
                    param.Add("@IMSComment", approveRejectWP.Comment);
                    param.Add("@IMSId", approveRejectWP.ApprovedId);
                    param.Add("@Status", approveRejectWP.Status);
                    param.Add("@IMSApprovedDate", DateTime.Now.Date);
                    param.Add("@WPID", approveRejectWP.WPID);

                    var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);
                    if (result > 0 && approveRejectWP.Status == "Approved")
                        await generateVMPass(approveRejectWP.WPID);
                    return result;
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> CreateWP(WP workPermit)
        {
            IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
            conn.Open();

            using (var transction = conn.BeginTransaction())
            {
                var WPId = 0;
                var index = 0;

                try
                {
                    var sQuery = @"";

                    if (workPermit.WPID > 0)
                    {
                        sQuery = @"UPDATE [dbo].[tbl_WP]
                                   SET 
	                                   [WPDate] = @WPDate
                                      ,[WPType] = @WPType
                                      ,[ContractorId] = @ContractorId
                                      ,[WorkStartDate] = @WorkStartDate
                                      ,[WorkEndDate] = @WorkEndDate
                                      ,[InitiatedById] = @InitiatedById
                                      ,[NatureOfWork] = @NatureOfWork
                                      ,[HRId] = @HRId
                                      ,[FinalId] = @FinalId
                                      ,[IMSId] = @IMSId
                                      ,[Status] = @Status
                                      ,[IsSubmitted] = @IsSubmitted    
                                     
                                      ,[UpdatedBy] = @UpdatedBy
                                      ,[UpdatedDate] = @UpdatedDate
                                 WHERE WPID = @WPID";

                        //,[SafetyTraining] = @SafetyTraining 
                        //,[TrainedDate] = @TrainedDate

                        WPId = workPermit.WPID.Value;

                        DynamicParameters param = new DynamicParameters();
                        param.Add("@WPDate", workPermit.WPDate);
                        param.Add("@WPType", workPermit.WPType);
                        //param.Add("@Unit", workPermit.Unit);
                        //param.Add("@WorkLocation", workPermit.WorkLocation);
                        param.Add("@ContractorId", workPermit.ContractorId);
                        param.Add("@WorkStartDate", workPermit.WorkStartDate);
                        param.Add("@WorkEndDate", workPermit.WorkEndDate);
                        param.Add("@InitiatedById", workPermit.InitiatedById);
                        param.Add("@NatureOfWork", workPermit.NatureOfWork);
                        //param.Add("@SafetyTraining", workPermit.SafetyTraining);
                        //param.Add("@TrainedBy", workPermit.TrainedBy);
                        //param.Add("@TrainedDate", workPermit.TrainedDate);
                        param.Add("@Status", workPermit.StatusNew);
                        param.Add("@IsSubmitted", workPermit.IsSubmitted);
                        param.Add("@HRId", workPermit.HRId);
                        param.Add("@IMSId", workPermit.IMSId);
                        param.Add("@FinalId", workPermit.FinalId);
                        param.Add("@UpdatedBy", workPermit.UpdatedBy);
                        param.Add("@UpdatedDate", workPermit.UpdatedDate);
                        param.Add("@WPID", workPermit.WPID);

                        await conn.ExecuteAsync(sQuery, param, transction, 180).ConfigureAwait(false);

                        sQuery = $@"DELETE FROM tbl_WP_Employee WHERE EMPLOYEEID NOT IN ({workPermit.SelectedEmployees})";

                        await conn.ExecuteAsync(sQuery, null, transction, 180).ConfigureAwait(false);


                        if (!string.IsNullOrEmpty(workPermit.SelectedEmployees))
                        {
                            string[] employeeList = workPermit.SelectedEmployees.Split(',');

                            foreach (var item in employeeList)
                            {
                                sQuery = $@"IF NOT EXISTS(SELECT EMPLOYEEID FROM tbl_WP_Employee WHERE EmployeeID = {item} AND WPId = {workPermit.WPID})
                                            BEGIN
	                                            INSERT INTO TBL_WP_Employee (WPId,EmployeeId,PFInsuranceDetails,PFInsuranceFile,PFInsuranceFileName,ESICDetails,
                                                ESICFile,ESICFileName)
                                                SELECT {workPermit.WPID},EmployeeId,PFInsuranceDetails,PFInsuranceFile,PFInsuranceFileName,ESICDetails,ESICFile,ESICFileName 
                                                FROM tbl_EmployeDetails WHERE EmployeeId = { item }
                                            END";

                                await conn.ExecuteAsync(sQuery, param, transction, 180).ConfigureAwait(false);
                            }
                        }

                        //for (int i = 0; i < workPermit.listEmployee.Count; i++)
                        //{

                        //    if (workPermit.listEmployee[i].EmployeeId == 0)
                        //    {
                        //        sQuery = @"INSERT INTO [dbo].[tbl_EmployeDetails]
                        //                   ([EmployeeName],[PFInsuranceDetails],[PFInsuranceFile],[ESICDetails],[ESICFile])
                        //                    VALUES 
                        //                   (@EmployeeName,@PFInsuranceDetails,@PFInsuranceFile,@ESICDetails,@ESICFile)";

                        //        param = new DynamicParameters();
                        //        param.Add("@EmployeeName", workPermit.listEmployee[i].EmployeeName);
                        //        param.Add("@PFInsuranceDetails", workPermit.listEmployee[i].PFInsuranceDetails);
                        //        param.Add("@PFInsuranceFile", workPermit.listEmployee[i].PFInsuranceFile);
                        //        param.Add("@ESICDetails", workPermit.listEmployee[i].ESICDetails);
                        //        param.Add("@ESICFile", workPermit.listEmployee[i].ESICFile);
                        //    }
                        //    else
                        //    {
                        //        sQuery = @"UPDATE [dbo].[tbl_EmployeDetails]
                        //                   SET [EmployeeName] = @EmployeeName
                        //                      ,[PFInsuranceDetails] = @PFInsuranceDetails
                        //                      ,[ESICDetails] = @ESICDetails
                        //                 WHERE EmployeeId = @EmployeeId";

                        //        if (workPermit.listEmployee[i].PFInsuranceFile != null && workPermit.listEmployee[i].ESICFile == null)
                        //        {
                        //            sQuery = @"UPDATE [dbo].[tbl_EmployeDetails]
                        //                   SET [EmployeeName] = @EmployeeName
                        //                      ,[PFInsuranceDetails] = @PFInsuranceDetails
                        //                      ,[PFInsuranceFile] = @PFInsuranceFile
                        //                      ,[PFInsuranceFileName] = @PFInsuranceFileName
                        //                      ,[ESICDetails] = @ESICDetails
                        //                 WHERE EmployeeId = @EmployeeId";

                        //        }

                        //        if (workPermit.listEmployee[i].PFInsuranceFile == null && workPermit.listEmployee[i].ESICFile != null)
                        //        {
                        //            sQuery = @"UPDATE [dbo].[tbl_EmployeDetails]
                        //                   SET [EmployeeName] = @EmployeeName
                        //                      ,[PFInsuranceDetails] = @PFInsuranceDetails
                        //                      ,[ESICDetails] = @ESICDetails
                        //                      ,[ESICFile] = @ESICFile
                        //                      ,[ESICFileName] = @ESICFileName
                        //                 WHERE EmployeeId = @EmployeeId";

                        //        }

                        //        param = new DynamicParameters();
                        //        param.Add("@EmployeeName", workPermit.listEmployee[i].EmployeeName);
                        //        param.Add("@PFInsuranceDetails", workPermit.listEmployee[i].PFInsuranceDetails);
                        //        if (workPermit.listEmployee[i].PFInsuranceFile != null)
                        //        {
                        //            param.Add("@PFInsuranceFile", workPermit.listEmployee[i].PFInsuranceFile);
                        //            param.Add("@PFInsuranceFileName", workPermit.listEmployee[i].PFInsuranceFileName);
                        //        }

                        //        param.Add("@ESICDetails", workPermit.listEmployee[i].ESICDetails);
                        //        if (workPermit.listEmployee[i].ESICFile != null)
                        //        {
                        //            param.Add("@ESICFile", workPermit.listEmployee[i].ESICFile);
                        //            param.Add("@ESICFileName", workPermit.listEmployee[i].ESICFileName);
                        //        }
                        //        param.Add("@EmployeeId", workPermit.listEmployee[i].EmployeeId);
                        //    }

                        //    await conn.ExecuteAsync(sQuery, param, transction, 180).ConfigureAwait(false);
                        //}
                    }
                    else
                    {
                        sQuery = @"DECLARE @len int
                        DECLARE @I int
                        DECLARE @NextNumber VARCHAR(50)
		
		                        BEGIN
			                        SELECT @len =isnull (LEN(max(cast(substring(WPNO,4,len(WPNO)) AS int))),0),
			                        @I=ISNULL(max(cast(substring(WPNo,4,len(WPNO)) AS int)),0)+1 FROM tbl_WP 
			                        IF len(@I)=0
		                            BEGIN
				                        SET @NextNumber = 'WP-' + '0001'  
			                        END
			                        ELSE
			                        IF len(@I) = 1
			                        BEGIN
				                        SET @NextNumber ='WP-'+'000'+ CAST(@I AS VARCHAR)
			                        END
			                        ELSE
			                        IF len(@I)=2
			                        BEGIN
				                        SET @NextNumber ='WP-'+'00'+ CAST(@I AS VARCHAR)
			                        END
			                        Else
			                        IF len(@I)=3
			                        BEGIN 
				                        SET @NextNumber ='WP-'+'0'+ CAST(@I AS VARCHAR)
			                        END
			                        ELSE
			                        BEGIN
			                           SET @NextNumber ='WP-'+ CAST(@I AS VARCHAR)
			                        END
		                        END
                                INSERT INTO [dbo].[tbl_WP]
                                    ([WPNO],[WPDate],[WPType],[ContractorId],[WorkStartDate],[WorkEndDate],[InitiatedById]
                                    ,[NatureOfWork],[Status],[IsSubmitted],[HRId],[IMSId],[FinalId]
                                    ,[CreatedBy],[CreatedDate])
                                VALUES
                                    (@NextNumber,@WPDate,@WPType,@ContractorId,@WorkStartDate,@WorkEndDate,@InitiatedById
                                    ,@NatureOfWork,@Status,@IsSubmitted,@HRId,@IMSId,@FinalId
                                    ,@CreatedBy,@CreatedDate);
                            SELECT CAST(SCOPE_IDENTITY() as int);";


                        DynamicParameters param = new DynamicParameters();
                        param.Add("@WPNO", workPermit.WPNO);
                        param.Add("@WPDate", workPermit.WPDate);
                        param.Add("@WPType", workPermit.WPType);
                        param.Add("@ContractorId", workPermit.ContractorId);
                        param.Add("@WorkStartDate", workPermit.WorkStartDate);
                        param.Add("@WorkEndDate", workPermit.WorkEndDate);
                        param.Add("@InitiatedById", workPermit.InitiatedById);
                        param.Add("@NatureOfWork", workPermit.NatureOfWork);
                        param.Add("@Status", workPermit.StatusNew);
                        param.Add("@IsSubmitted", workPermit.IsSubmitted);
                        //param.Add("@HRApproval", workPermit.HRApproval);
                        //param.Add("@HRComment", workPermit.HRComment);
                        param.Add("@HRId", workPermit.HRId);
                        //param.Add("@HRApprovedDate", workPermit.HRApprovedDate);
                        //param.Add("@IMSApproval", workPermit.IMSApproval);
                        param.Add("@IMSId", workPermit.IMSId);
                        //param.Add("@IMSComment", workPermit.IMSComment);
                        //param.Add("@IMSApprovedDate", workPermit.IMSApprovedDate);
                        //param.Add("@FinalApproval", workPermit.FinalApproval);
                        //param.Add("@FinalComment", workPermit.FinalComment);
                        param.Add("@FinalId", workPermit.FinalId);
                        //param.Add("@FinalApprovedDate", workPermit.FinalApprovedDate);
                        param.Add("@CreatedBy", workPermit.CreatedBy);
                        param.Add("@CreatedDate", workPermit.CreatedDate);
                        //param.Add("@SafetyTraining", workPermit.SafetyTraining);
                        //param.Add("@TrainedBy", workPermit.TrainedBy);
                        //param.Add("@TrainedDate", workPermit.TrainedDate);

                        WPId = conn.QuerySingle<int>(sQuery, param, transction, 180);

                        if (!string.IsNullOrEmpty(workPermit.SelectedEmployees))
                        {
                            sQuery = $@"INSERT INTO TBL_WP_Employee (WPId,EmployeeId,PFInsuranceDetails,PFInsuranceFile,PFInsuranceFileName,ESICDetails,
                                        ESICFile,ESICFileName)
                                        SELECT {WPId},EmployeeId,PFInsuranceDetails,PFInsuranceFile,PFInsuranceFileName,ESICDetails,ESICFile,ESICFileName 
                                        FROM tbl_EmployeDetails WHERE EmployeeId IN ({workPermit.SelectedEmployees})";
                            await conn.ExecuteAsync(sQuery, param, transction, 180).ConfigureAwait(false);
                        }


                        //for (int i = 0; i < workPermit.listEmployee.Count; i++)
                        //{

                        //    if (workPermit.listEmployee[i].EmployeeId == 0)
                        //    {
                        //        //sQuery = @"INSERT INTO [dbo].[tbl_EmployeDetails]
                        //        //           ([WPID],[EmployeeName],[PFInsuranceDetails],[PFInsuranceFile],[ESICDetails],[ESICFile])
                        //        //            VALUES 
                        //        //           (@WPID,@EmployeeName,@PFInsuranceDetails,@PFInsuranceFile,@ESICDetails,@ESICFile)";

                        //        sQuery = @"INSERT INTO [dbo].[tbl_EmployeDetails]
                        //                   ([WPID],[EmployeeName],[PFInsuranceDetails],[PFInsuranceFile],[PFInsuranceFileName],[ESICDetails],[ESICFile],[ESICFileName])
                        //                    VALUES 
                        //                   (@WPID,@EmployeeName,@PFInsuranceDetails,@PFInsuranceFile,@PFInsuranceFileName,@ESICDetails,@ESICFile,@ESICFileName)";

                        //        param = new DynamicParameters();
                        //        param.Add("@WPID", WPId);
                        //        param.Add("@EmployeeName", workPermit.listEmployee[i].EmployeeName);
                        //        param.Add("@PFInsuranceDetails", workPermit.listEmployee[i].PFInsuranceDetails);
                        //        param.Add("@PFInsuranceFile", workPermit.listEmployee[i].PFInsuranceFile);
                        //        param.Add("@PFInsuranceFileName", workPermit.listEmployee[i].PFInsuranceFileName);
                        //        param.Add("@ESICDetails", workPermit.listEmployee[i].ESICDetails);
                        //        param.Add("@ESICFile", workPermit.listEmployee[i].ESICFile);
                        //        param.Add("@ESICFileName", workPermit.listEmployee[i].ESICFileName);
                        //    }
                        //    else
                        //    {
                        //        sQuery = @"UPDATE [dbo].[tbl_EmployeDetails]
                        //                   SET [EmployeeName] = @EmployeeName
                        //                      ,[PFInsuranceDetails] = @PFInsuranceDetails
                        //                      ,[PFInsuranceFile] = @PFInsuranceFile
                        //                      ,[PFInsuranceFileName] = @PFInsuranceFileName
                        //                      ,[ESICDetails] = @ESICDetails
                        //                      ,[ESICFile] = @ESICFile
                        //                      ,[ESICFileName] = @ESICFileName
                        //                 WHERE EmployeeId = @EmployeeId";

                        //        if (workPermit.listEmployee[i].PFInsuranceFile != null && workPermit.listEmployee[i].ESICFile == null)
                        //        {
                        //            sQuery = @"UPDATE [dbo].[tbl_EmployeDetails]
                        //                   SET [EmployeeName] = @EmployeeName
                        //                      ,[PFInsuranceDetails] = @PFInsuranceDetails
                        //                      ,[PFInsuranceFile] = @PFInsuranceFile
                        //                      ,[PFInsuranceFileName] = @PFInsuranceFileName
                        //                      ,[ESICDetails] = @ESICDetails
                        //                 WHERE EmployeeId = @EmployeeId";

                        //        }

                        //        if (workPermit.listEmployee[i].PFInsuranceFile == null && workPermit.listEmployee[i].ESICFile != null)
                        //        {
                        //            sQuery = @"UPDATE [dbo].[tbl_EmployeDetails]
                        //                   SET [EmployeeName] = @EmployeeName
                        //                      ,[PFInsuranceDetails] = @PFInsuranceDetails
                        //                      ,[ESICDetails] = @ESICDetails
                        //                      ,[ESICFile] = @ESICFile
                        //                      ,[ESICFileName] = @ESICFileName
                        //                 WHERE EmployeeId = @EmployeeId";

                        //        }

                        //        param = new DynamicParameters();
                        //        param.Add("@EmployeeName", workPermit.listEmployee[i].EmployeeName);
                        //        param.Add("@PFInsuranceDetails", workPermit.listEmployee[i].PFInsuranceDetails);

                        //        if (workPermit.listEmployee[i].PFInsuranceFile != null) {
                        //            param.Add("@PFInsuranceFile", workPermit.listEmployee[i].PFInsuranceFile);
                        //            param.Add("@PFInsuranceFileName", workPermit.listEmployee[i].PFInsuranceFileName);
                        //        }

                        //        param.Add("@ESICDetails", workPermit.listEmployee[i].ESICDetails);
                        //        if (workPermit.listEmployee[i].ESICFile != null)
                        //        {
                        //            param.Add("@ESICFile", workPermit.listEmployee[i].ESICFile);
                        //            param.Add("@ESICFileName", workPermit.listEmployee[i].ESICFileName);
                        //        }
                        //        param.Add("@EmployeeId", workPermit.listEmployee[i].EmployeeId);
                        //    }

                        //    await conn.ExecuteAsync(sQuery, param, transction, 180).ConfigureAwait(false);
                        //}
                    }

                    transction.Commit();

                    return WPId;
                }
                catch (Exception ex)
                {
                    transction.Rollback();
                    conn.Close();
                    return 0;
                }
            }
        }

        //public async Task<int> CreateWP(WP workPermit)
        //{
        //    try
        //    {
        //        var sQuery = @"";

        //        if (workPermit.PermissionID > 0)
        //        {
        //            sQuery = $@"UPDATE [dbo].[tbl_WorkPermit]
        //                   SET 
        //                      ,[PermissionType] = @PermissionType
        //                      ,[WorkGroup] = @WorkGroup
        //                      ,[DocumentNo] = @DocumentNo
        //                      ,[RevisionNo] = @RevisionNo
        //                      ,[PermissionDate] = @PermissionDate
        //                      ,[ValidityDate] = @ValidityDate
        //                      ,[Label] = @Label
        //                      ,[Description] = @Description
        //                      ,[UpdatedBy] = @UpdatedBy
        //                      ,[UpdatedDate] = @UpdatedDate
        //                 WHERE PermissionID = @PermissionID";

        //            //,[File] = @File
        //            //,[PublishDate] = @PublishDate
        //            //,[Status] = @Status
        //            //,[PreparedBy] = @PreparedBy
        //            //,[IsDeleted] = @IsDeleted

        //            DynamicParameters param = new DynamicParameters();
        //            param.Add("@PermissionType", workPermit.PermissionType);
        //            param.Add("@WorkGroup", workPermit.WorkGroup);
        //            param.Add("@DocumentNo", workPermit.DocumentNo);
        //            param.Add("@RevisionNo", workPermit.RevisionNo);
        //            param.Add("@PermissionDate", workPermit.PermissionDate);
        //            param.Add("@ValidityDate", workPermit.ValidityDate);
        //            param.Add("@Label", workPermit.Label);
        //            param.Add("@Description", workPermit.Description);
        //            //param.Add("@File", workPermit.File);
        //            //param.Add("@PublishDate", workPermit.PublishDate);
        //            //param.Add("@Status", workPermit.Status);
        //            //param.Add("@PreparedBy", workPermit.PreparedBy);
        //            param.Add("@CreatedBy", workPermit.CreatedBy);
        //            param.Add("@CreatedDate", workPermit.CreatedDate);
        //            param.Add("@PermissionID", workPermit.PermissionID);

        //            var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);

        //            return result;
        //        }
        //        else
        //        {
        //            sQuery = $@"INSERT INTO [dbo].[tbl_WorkPermit]
        //                    ([PermissionType],[WorkGroup],[DocumentNo]
        //                    ,[RevisionNo],[PermissionDate],[ValidityDate],[Label]
        //                    ,[Description],[CreatedBy],[CreatedDate])                       
        //                    VALUES
        //                    (@PermissionType,@WorkGroup,@DocumentNo
        //                    ,@RevisionNo,@PermissionDate,@ValidityDate,@Label
        //                    ,@Description,@CreatedBy,@CreatedDate)";

        //            DynamicParameters param = new DynamicParameters();
        //            //param.Add("@UserID", workPermit.UserID);
        //            param.Add("@PermissionType", workPermit.PermissionType);
        //            param.Add("@WorkGroup", workPermit.WorkGroup);
        //            param.Add("@DocumentNo", workPermit.DocumentNo);
        //            param.Add("@RevisionNo", workPermit.RevisionNo);
        //            param.Add("@PermissionDate", workPermit.PermissionDate);
        //            param.Add("@ValidityDate", workPermit.ValidityDate);
        //            param.Add("@Label", workPermit.Label);
        //            param.Add("@Description", workPermit.Description);
        //            //param.Add("@File", workPermit.File);
        //            //param.Add("@PublishDate", workPermit.PublishDate);
        //            //param.Add("@Status", workPermit.Status);
        //            //param.Add("@PreparedBy", workPermit.PreparedBy);
        //            param.Add("@CreatedBy", workPermit.CreatedBy);
        //            param.Add("@CreatedDate", workPermit.CreatedDate);

        //            var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);

        //            return result;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }


        //}

        public async Task<IEnumerable<DropDownModel>> GetWorkGroup(string permissionType)
        {
            var sQuery = $@"SELECT PromptKey 'text',PromptValue 'valueInt' FROM tbl_PromptTag WHERE PromptGroup= @PromptGroup";

            DynamicParameters param = new DynamicParameters();
            param.Add("@PromptGroup", permissionType);

            var result = await _genericRepository.GetAsync<DropDownModel>(sQuery, param);

            return result;
        }

        public async Task<IEnumerable<WP>> GetWP(WPFilter filter)
        {
            var sQuery = $@"SELECT
                            WPID,WPNO,WPDate,Replace(convert(char(11),WPDate,106),' ', '-') as 'strWPDate'
                            ,WPType	,WorkStartDate,	CM.Name 'ContractorName'
                            ,Replace(convert(char(11),WorkStartDate,106),' ', '-') as 'strWorkStartDate',WorkEndDate	
                            ,Replace(convert(char(11),WorkEndDate,106),' ', '-') as 'strWorkEndDate'
                            ,U.FirstName + ' ' + U.LastName 'InitiatedByName'	
                            ,Status
                            , WP.CreatedBy
                            FROM [dbo].[tbl_WP] WP 
                            LEFT JOIN [dbo].[tbl_ContractorMaster] CM ON WP.ContractorId = CM.ContractorId 
                            LEFT JOIN [dbo].[tbl_Users] U ON WP.InitiatedById = U.UserId 
                            LEFT JOIN [dbo].[tbl_Users] UHR ON UHR.UserID = WP.HRId
                            LEFT JOIN [dbo].[tbl_Users] UIMS ON UIMS.UserID = WP.IMSId
                            LEFT JOIN [dbo].[tbl_Users] UFINAL ON UFINAL.UserID = WP.FinalId ";



            if (filter.WPID > 0)
            {
                sQuery = $@"SELECT
                            WPID,WPNO,WPDate,Replace(convert(char(11),WPDate,106),' ', '-') as 'strWPDate'
                            ,WPType	,Unit	,WorkLocation	,WP.ContractorId	,CM.Name 'ContractorName',LicenseDetails,CM.EmailId  'ContractorEmailId',WorkStartDate	
                            ,Replace(convert(char(11),WorkStartDate,106),' ', '-') as 'strWorkStartDate',WorkEndDate	
                            ,Replace(convert(char(11),WorkEndDate,106),' ', '-') as 'strWorkEndDate',InitiatedById, U.FirstName + ' ' + U.LastName 'InitiatedByName'	
                            ,NatureOfWork	,ISNULL(SafetyTraining,'') SafetyTraining	,TrainedBy	,Replace(convert(char(11),TrainedDate,106),' ', '-') as 'TrainedDate',Status,
                            ISNULL(IsSubmitted,(CAST(0 as bit))) 'IsSubmitted',ISNULL(HRApproval,(CAST(0 as bit))) HRApproval,HRComment,
                            ISNULL(IMSApproval,(CAST(0 as bit))) IMSApproval,IMSComment,ISNULL(FinalApproval,(CAST(0 as bit))) FinalApproval,FinalComment
                            ,HRId,IMSId,FinalId, 
                            UHR.FirstName + ' ' + UHR.LastName 'HRApproverName',
                            UIMS.FirstName + ' ' + UIMS.LastName 'IMSApproverName',
                            UFINAL.FirstName + ' ' + UFINAL.LastName 'FinalApproverName',
                            WP.CreatedBy
                            FROM [dbo].[tbl_WP] WP 
                            LEFT JOIN [dbo].[tbl_ContractorMaster] CM ON WP.ContractorId = CM.ContractorId 
                            LEFT JOIN [dbo].[tbl_Users] U ON WP.InitiatedById = U.UserId 
                            LEFT JOIN [dbo].[tbl_Users] UHR ON UHR.UserID = WP.HRId
                            LEFT JOIN [dbo].[tbl_Users] UIMS ON UIMS.UserID = WP.IMSId
                            LEFT JOIN [dbo].[tbl_Users] UFINAL ON UFINAL.UserID = WP.FinalId ";
                sQuery = sQuery + $" WHERE WPID = { filter.WPID }";
            }
            sQuery = sQuery + $" ORDER BY WPID DESC";
            var result = await _genericRepository.GetAsync<WP>(sQuery, null);

            if (filter.WPID > 0)
            {
                List<EmployeeDetails> lstEmployee = new List<EmployeeDetails>();

                sQuery = @"SELECT ROW_NUMBER() OVER(ORDER BY WPE.EmployeeId ASC) AS SrNo,WPE.EmployeeId,EmployeeName,WPID,WPE.PFInsuranceDetails,WPE.PFInsuranceFileName,WPE.ESICDetails,WPE.ESICFileName FROM tbl_WP_Employee WPE
                           INNER JOIN tbl_EmployeDetails ED ON WPE.EmployeeId = ED.EmployeeId WHERE WPE.WPId = @WPID";
                DynamicParameters param = new DynamicParameters();
                param.Add("WPID", filter.WPID);
                var resultEmployee = await _genericRepository.GetAsync<EmployeeDetails>(sQuery, param);
                //if (resultEmployee.Count() > 0)
                //{
                //    for (int i = 0; i < resultEmployee.Count(); i++)
                //    {
                //        if (resultEmployee.ToList()[i].ESICFile != null)
                //            resultEmployee.ToList()[i].ESICFileBase64 = Convert.ToBase64String(resultEmployee.ToList()[i].ESICFile);
                //        if (resultEmployee.ToList()[i].PFInsuranceFile != null)
                //            resultEmployee.ToList()[i].PFInsuranceFileBase64 = Convert.ToBase64String(resultEmployee.ToList()[i].PFInsuranceFile);
                //    }
                //}

                result.FirstOrDefault().listEmployee = resultEmployee.ToList();
            }

            return result;
        }

        public async Task<EmployeeDetails> getEmployeeFile(EmployeeFilter details)
        {
            var sQuery = $@"SELECT WPE.EmployeeId,WPID,WPE.PFInsuranceDetails,WPE.PFInsuranceFile,WPE.PFInsuranceFileName,WPE.ESICDetails,WPE.ESICFile,WPE.ESICFileName FROM tbl_WP_Employee WPE
                            INNER JOIN tbl_EmployeDetails ED ON WPE.EmployeeId = ED.EmployeeId WHERE WPE.EmployeeId = @EmployeeId";

            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeId", details.EmployeeId);
            try
            {
                var result = await _genericRepository.GetAsync<EmployeeDetails>(sQuery, param);

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<int> removeEmployee(int EmployeeId)
        {
            var sQuery = $@"DELETE FROM tbl_WP_Employee WHERE EmployeeId ={ EmployeeId.ToString() }";

            var result = await _genericRepository.ExecuteCommandAsync(sQuery);

            return result;
        }

        public async Task<int> SaveSafetyTraining(SafetyTrainingModel safetyTraining)
        {
            var sQuery = $@"UPDATE tbl_WP SET SafetyTraining = @SafetyTraining, TrainedBy = @TrainedBy , TrainedDate = @TrainedDate,
                            Status = @Status WHERE WPID = @WPID";

            var parameters = new DynamicParameters();
            //parameters.Add("@Status", "Waiting for IMS Approval");
            parameters.Add("@SafetyTraining", safetyTraining.SafetyTraining);
            parameters.Add("@TrainedBy", safetyTraining.TrainedBy);
            parameters.Add("@TrainedDate", safetyTraining.TrainedDate);
            parameters.Add("@Status", safetyTraining.StatusNew);
            parameters.Add("@WPID", safetyTraining.WPID);

            var result = await _genericRepository.ExecuteCommandAsync(sQuery, parameters);

            return result;
        }

        private async Task generateVMPass(int WPID)
        {
            WP wp = new Models.WP.WP();
            var result = await GetWP(new WPFilter() { WPID = WPID });
            if (result != null && result.Count() > 0)
            {
                wp = result.FirstOrDefault();
            }
            else
                return;

            var sQuery = @"DECLARE @YR VARCHAR(10)
                           SET @YR = 'VS' + CAST((YEAR(GETDATE()) % 100) AS VARCHAR)
                           SELECT @YR +right(1000000 + ISNULL(MAX(SUBSTRING(APPOINTMENTNO, 5, LEN(APPOINTMENTNO) - 1)), 0) + 1, 4) FROM[dbo].[tbl_VM_Appointment] WHERE SUBSTRING(APPOINTMENTNO,1,4) = @YR";

            var appointmentNo = await _genericRepository.GetAsync<string>(sQuery, null);

            sQuery = $@"INSERT INTO [dbo].[tbl_VM_Appointment]
                           ([AppointmentNo],[VisitorName],[VisitorPhoneNumber],[RepresentingCompany]
                           ,[Address],[PersonToVisitID],[PurposeToVisit],[Date],[VisitorsEmails],[Remark],[NumberOfPerson],[Status],[WPID],[CreatedBy],[CreatedDate])
                     VALUES
                           (@AppointmentNo,@VisitorName,@VisitorPhoneNumber,@RepresentingCompany,
                            @Address,@PersonToVisitID,@PurposeToVisit,@Date,@VisitorsEmails,@Remark,@NumberOfPerson,@Status,@WPID,@CreatedBy,@CreatedDate); SELECT SCOPE_IDENTITY();";

            string visitorName = string.Empty;

            List<EmployeeDetails> listEmployee = wp.listEmployee;

            var resultContractor = await _contractorRepository.GetAllContractors(new ContractorFilter() { ContractorId = wp.ContractorId });
            ContractorMaster contractorMaster = new ContractorMaster();
            if (resultContractor != null)
                contractorMaster = resultContractor.FirstOrDefault();

            if (listEmployee.Count > 0)
                visitorName = listEmployee[0].EmployeeName;

            DynamicParameters param = new DynamicParameters();
            param.Add("@AppointmentNo", appointmentNo.FirstOrDefault());
            param.Add("@VisitorName", visitorName);
            param.Add("@VisitorPhoneNumber", "");
            param.Add("@RepresentingCompany", wp.ContractorName);
            param.Add("@Address", "");
            param.Add("@PersonToVisitID", wp.InitiatedById);
            param.Add("@PurposeToVisit", "Official");
            param.Add("@VisitorsEmails", contractorMaster.EmailId);
            param.Add("@Remark", "Generated from Work Permit: " + wp.WPNO);
            param.Add("@NumberOfPerson", listEmployee.Count);
            param.Add("@Status", "Open");
            param.Add("@WPID", WPID);
            param.Add("@Date", wp.WorkStartDate);
            param.Add("@CreatedBy", wp.InitiatedById);
            param.Add("@CreatedDate", DateTime.UtcNow);

            await _genericRepository.GetAsync<int>(sQuery, param);

        }

        public async Task<int> CloseWorkPermit(int WPID)
        {
            var sQuery = $@"UPDATE [dbo].[tbl_WP] SET Status = 'Closed' WHERE WPID = { WPID.ToString() }";
            var result = await _genericRepository.ExecuteCommandAsync(sQuery);

            return result;
        }

        public async Task<DataSet> Wpreminder()
        {
            var sQuery = $@"exec Wpreminder";

            try
            {
                DataSet result = await _genericRepository.GetAsyncProc<DataSet>("Wpreminder", null);
                if (result != null)
                {
                    DataTable dt = result.Tables[0];
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            bool ToMgr = false;
                            bool ToHr = false;
                            bool ToIMS = false;
                            bool ToCreator = false;                            
                            string wpid = "";
                            string ToMgrEmail = string.Empty;
                            string ToHrEmail = string.Empty;
                            string ToIMSEmail = string.Empty;
                            string ToCreatorEmail = string.Empty;
                            string EmailText = string.Empty;
                            string EmailSubject = string.Empty;
                            foreach (DataRow row in dt.Rows)
                            {
                                ToMgr = Convert.ToBoolean(row["SendToMgr"].ToString());
                                ToHr = Convert.ToBoolean(row["SendToHr"].ToString());
                                ToIMS = Convert.ToBoolean(row["SendToIMS"].ToString());
                                ToCreator = Convert.ToBoolean(row["SendToCreator"].ToString());
                                wpid = Convert.ToString(row["WPID"].ToString());
                                ToMgrEmail = Convert.ToString(row["ManagerEmail"]);
                                ToHrEmail = Convert.ToString(row["HrEmailId"]);
                                ToIMSEmail = Convert.ToString(row["IMSEmailId"]);
                                EmailText = Convert.ToString(row["EmailText"]);
                                EmailSubject = Convert.ToString(row["EmailSubject"]);
                                ToCreatorEmail = Convert.ToString(row["CreatorEmail"]);
                                if (ToMgr && !string.IsNullOrEmpty(ToMgrEmail))
                                {
                                    DB.SendMailAsync(ToMgrEmail, EmailText, EmailSubject);

                                }
                                if (ToHr && !string.IsNullOrEmpty(ToHrEmail))
                                {
                                    DB.SendMailAsync(ToHrEmail, EmailText, EmailSubject);
                                }
                                if (ToIMS && !string.IsNullOrEmpty(ToIMSEmail))
                                {
                                    DB.SendMailAsync(ToIMSEmail, EmailText, EmailSubject);
                                }
                                if (ToCreator && !string.IsNullOrEmpty(ToCreatorEmail))
                                {
                                    DB.SendMailAsync(ToCreatorEmail, EmailText, EmailSubject);
                                }
                                var sQueryforUpdate = $@"Insert into tbl_WPCloseReminderLog (WPID,SentToManager,SentToHR,SentIMS)
                                                Select '{wpid}','{ToMgr}','{ToHr}','{ToIMS}'";
                                var result1 = await _genericRepository.ExecuteCommandAsync(sQueryforUpdate);
                            }

                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        //public async Task<IEnumerable<WorkPermitType>> GetWPType()
        //{
        //    var sQuery = $@"SELECT
        //                    WPID,WPNO,WPDate,Replace(convert(char(11),WPDate,106),' ', '-') as 'strWPDate'
        //                    ,WPType	,WorkStartDate,	CM.Name 'ContractorName'
        //                    ,Replace(convert(char(11),WorkStartDate,106),' ', '-') as 'strWorkStartDate',WorkEndDate	
        //                    ,Replace(convert(char(11),WorkEndDate,106),' ', '-') as 'strWorkEndDate'
        //                    ,U.FirstName + ' ' + U.LastName 'InitiatedByName'	
        //                    ,Status
        //                    FROM [dbo].[tbl_WP] WP 
        //                    LEFT JOIN [dbo].[tbl_ContractorMaster] CM ON WP.ContractorId = CM.ContractorId 
        //                    LEFT JOIN [dbo].[tbl_Users] U ON WP.InitiatedById = U.UserId 
        //                    LEFT JOIN [dbo].[tbl_Users] UHR ON UHR.UserID = WP.HRId
        //                    LEFT JOIN [dbo].[tbl_Users] UIMS ON UIMS.UserID = WP.IMSId
        //                    LEFT JOIN [dbo].[tbl_Users] UFINAL ON UFINAL.UserID = WP.FinalId ";

        //    sQuery = sQuery + $" ORDER BY WPID DESC";
        //    var result = await _genericRepository.GetAsync<WP>(sQuery, null);

        //    return result;
        //}

    }

}
