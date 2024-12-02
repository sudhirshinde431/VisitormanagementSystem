using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using VisitorsManagement.Models;
using VisitorsManagement.Models.Contractor;
using VisitorsManagement.Models.RemoteEmployee;
using VisitorsManagement.Models.WP;
using static Antlr.Runtime.Tree.TreeWizard;

namespace VisitorsManagement.Repository
{
    public class RemoteEmployeeRepository : VisitorsManagement.Repository.IRemoteEmployee
    {
        private readonly IGenericRepository _genericRepository;
        private readonly IContractorRepository _contractorRepository;
        public RemoteEmployeeRepository(IGenericRepository genericRepository, IContractorRepository contractorRepository)
        {
            _genericRepository = genericRepository;
            _contractorRepository = contractorRepository;
        }

        public async Task<IEnumerable<RemoteEmployee>> getRemoteEmployee(RemoteEmployeeFilter filter)
        {

            var sQuery = $@"SELECT
                            Pkey,Hcode,Name,RemoteEmployee.EmailID
                            --,Replace(convert(char(11),CheckinDateTime,106),' ', '-') as 'CheckinDateTime'                           
                            --,Replace(convert(char(11),CheckOutDateTime,106),' ', '-') as 'CheckOutDateTime' 
                             ,FORMAT(CheckOutDateTime, 'dd-MMM-yyyy HH:mm') as CheckOutDateTime
                              ,FORMAT(CheckinDateTime, 'dd-MMM-yyyy HH:mm') as CheckinDateTime
                            ,IsVehicalParkedOnPremises
                            ,VehicalNumber
                            ,Comments
                            ,CONCAT(ISNULL(CreatedBy.FirstName,' '),' ',ISNULL(CreatedBy.LastName,' ')) as CreatedByName
							,CONCAT(ISNULL(UpdatedBy.FirstName,' '),' ',ISNULL(UpdatedBy.LastName,' ')) as UpdatedByName
							,RemoteEmployee.CreatedDate
							,RemoteEmployee.UpdatedDate
                            ,Case when RemoteEmployee.GuestAccessCardIssue IS NULL THEN
                            'Not Done'
                            ELSE
                            'Done'
                            END AS SecurityCheckDone
,Case when RemoteEmployee.GuestAccessCardIssue IS NULL OR RemoteEmployee.GuestAccessCardIssue='No' THEN
                            'Not Issued'
                            ELSE
                            'Issued'
                            END AS GuestAccessCardIssue
                          ,Case when RemoteEmployee.AccessCardCollectionStatus IS NULL THEN
                            ''
                           WHEN RemoteEmployee.AccessCardCollectionStatus='No' THEN
                            'Not Collected' 
                           ELSE
                            'Collected'
                            END AS AccessCardCollectionStatus

,Escalation
,Status
,Re_Number
,DeafultGuestCardNumber
,CONCAT(ISNULL(SecurityCheckDoneBy.FirstName,' '),' ',ISNULL(SecurityCheckDoneBy.LastName,' ')) as SecurityCheckDoneBy
,CreatedBySC as SecurityCheckDoneOn,
Case when Status='Open' Then
1
when Status='Check In' Then
2
when Status='Checked Out' Then
3
Else
4
END as SortOrder

                            FROM [dbo].[RemoteEmployee]
                            LEFT JOIN tbl_Users as CreatedBy on
							RemoteEmployee.CReatedBy=CreatedBy.UserID
							LEFT JOIN tbl_Users as UpdatedBy on
							RemoteEmployee.UpdatedBy=UpdatedBy.UserID
                            LEFT JOIN tbl_Users as SecurityCheckDoneBy on
							RemoteEmployee.UpdatedBy=SecurityCheckDoneBy.UserID
                             ";


            sQuery = sQuery + $" ORDER BY SortOrder";
            if (!string.IsNullOrEmpty(filter.Hcode))
            {
                sQuery = $@"SELECT
                            Pkey,Hcode,Name,EmailID,Replace(convert(char(11),CheckinDateTime,106),' ', '-') as 'CheckinDateTime'                           
                            ,Replace(convert(char(11),CheckOutDateTime,106),' ', '-') as 'CheckOutDateTime' 
                            ,IsVehicalParkedOnPremises
                            ,VehicalNumber
                            ,Comments
                            ,Status
                            ,Re_Number
                            FROM [dbo].[RemoteEmployee]";
                sQuery = sQuery + $" WHERE Hcode like '%" + filter.Hcode + "%' ";


            }
        
            if (!string.IsNullOrEmpty(filter.Pkey))
            {
                sQuery = $@"SELECT
                            Pkey,Hcode,Name,EmailID
                            --,Replace(convert(char(11),CheckinDateTime,106),' ', '-') as 'CheckinDateTime'                           
                            --,Replace(convert(char(11),CheckOutDateTime,106),' ', '-') as 'CheckOutDateTime' 
                             ,FORMAT(CheckOutDateTime, 'dd-MMM-yyyy HH:mm') as CheckOutDateTime
                              ,FORMAT(CheckinDateTime, 'dd-MMM-yyyy HH:mm') as CheckinDateTime
                            ,IsVehicalParkedOnPremises
                            ,VehicalNumber
                            ,Comments
                            ,GuestAccessCardIssue
                            ,AccessCardCollectionStatus
                             ,Escalation
                             ,DeafultGuestCardNumber
                             ,CreatedDateSC
                             ,UpdatedDateSC
                            ,CreatedBySC
                            ,UpdatedBySC
   ,Status
,Re_Number
                            FROM [dbo].[RemoteEmployee]";
                sQuery = sQuery + $" WHERE Pkey ='" + filter.Pkey + "' ";


            }

            if (filter.ForAutocomplete == "ForAutocomplete")
            {
                sQuery = $@"SELECT Hcode,EmailID,Name
                             FROM RemoteEmployee";

            }
            if (filter.ForAutocomplete == "ForAutocomplete")
            {
                sQuery = sQuery + " Where Hcode is not null GROUP BY Hcode,EmailID,Name";

            }
            var result = await _genericRepository.GetAsync<RemoteEmployee>(sQuery, null);

            if (!string.IsNullOrEmpty(filter.Hcode))
            {
                List<RemoteEmployee> remoteEmployee = new List<RemoteEmployee>();
                if (filter.FilterText == "ForAutotCompleteSelect")
                {
                    sQuery = @"SELECT Hcode,Name,EmailID
                           FROM RemoteEmployee 
                           Where Hcode = @Hcode
                           GROUP BY Hcode,EmailID,Name
                           ";

                }
                else
                {
                    sQuery = @"SELECT ROW_NUMBER() OVER(ORDER BY Hcode ASC) AS Pkey,Hcode,Name,EmailID,CheckinDateTime,CheckOutDateTime,
                           IsVehicalParkedOnPremises,VehicalNumber
                           ,Comments
                           FROM RemoteEmployee 
                           Where Hcode = @Hcode";
                }

               
                DynamicParameters param = new DynamicParameters();
                param.Add("Hcode", filter.Hcode);
                var resultEmployee = await _genericRepository.GetAsync<RemoteEmployee>(sQuery, param);


                result.FirstOrDefault().remoteEmployee = resultEmployee.ToList();
            }

            return result;
        }

        public static int CalculateDaysBetweenDates(DateTime startDate, DateTime endDate)
        {
            // Calculate the difference in days
            TimeSpan dateDifference = endDate - startDate;
            return dateDifference.Days;
        }

        public async Task<int> SaveRemoteEmployee(RemoteEmployee remoteEmployee)
        {
            var sQuery = @"";



            try
            {
                if (!string.IsNullOrEmpty(remoteEmployee.Pkey))
                {
                    sQuery = $@"UPDATE [dbo].[RemoteEmployee]
                           SET 
                              [Hcode] = @Hcode
                              ,[Name] = @Name
                              ,[EmailID] = @EmailID                               
                              ,[CheckOutDateTime] = @CheckOutDateTime
                              ,[IsVehicalParkedOnPremises] = @IsVehicalParkedOnPremises
                              ,[VehicalNumber] = @VehicalNumber
                              ,[Comments] = @Comments
                              ,[CreatedBy] = @CreatedBy
                              ,[UpdatedBy] = @UpdatedBy
                              ,[CreatedDate] = @CreatedDate
                              ,[UpdatedDate] = @UpdatedDate    
                              ,[Status] = @Status     
                         WHERE [Pkey] = @Pkey";



                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Hcode", remoteEmployee.Hcode);
                    param.Add("@Name", remoteEmployee.Name);
                    param.Add("@EmailID", remoteEmployee.EmailID);                
                    param.Add("@CheckOutDateTime",Convert.ToDateTime(remoteEmployee.CheckOutDateTime));
                    param.Add("@IsVehicalParkedOnPremises", remoteEmployee.IsVehicalParkedOnPremises);
                    param.Add("@VehicalNumber", remoteEmployee.VehicalNumber);
                    param.Add("@Comments", remoteEmployee.Comments);
                    param.Add("@CreatedBy", remoteEmployee.CreatedBy);
                    param.Add("@UpdatedBy", remoteEmployee.UpdatedBy);
                    param.Add("@CreatedDate", remoteEmployee.CreatedDate);
                    param.Add("@UpdatedDate", remoteEmployee.UpdatedDate);
                    param.Add("@Pkey", remoteEmployee.Pkey);
                    param.Add("@Status", remoteEmployee.Status);
                    var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);
                    ///var result = await _genericRepository.GetAsync<int>(sQuery, param);

                    return result;
                }
                else
                {

                    int numberOfDays = CalculateDaysBetweenDates(Convert.ToDateTime(remoteEmployee.CheckinDateTime),
                        Convert.ToDateTime(remoteEmployee.CheckOutDateTime));
                    if (numberOfDays == 0)
                    {
                        numberOfDays = 1;
                    }
                    for (int i = 0; i < numberOfDays; i++)
                    {
                        DateTime CheckinDateTime = Convert.ToDateTime(remoteEmployee.CheckinDateTime);
                        TimeSpan CheckIntime = CheckinDateTime.TimeOfDay;
                        DateTime CheckInDate = CheckinDateTime.Date;
                        DateTime StartDateFInal= CheckInDate.Add(CheckIntime).AddDays(i);


                        TimeSpan time = Convert.ToDateTime(remoteEmployee.CheckOutDateTime).TimeOfDay;
                        DateTime dateOnly = CheckinDateTime.Date;
                        DateTime NewEndDate = dateOnly.Add(time).AddDays(i);

                        DateTime date = StartDateFInal;  // Use any DateTime here
                        bool isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

                        if(remoteEmployee.BookForWeekend==null)
                        {
                            remoteEmployee.BookForWeekend = "false";
                        }

                        if(Convert.ToBoolean(remoteEmployee.BookForWeekend)==false && isWeekend)
                        {
                            continue;  // Skip to the next iteration
                        }

                        sQuery = @"DECLARE @YR VARCHAR(10)
SET @YR = 'RE' + CAST((YEAR(GETDATE()) % 100) AS VARCHAR)
SELECT @YR +right(1000000 + ISNULL(MAX(SUBSTRING(Re_Number, 5, LEN(Re_Number) - 1)), 0) + 1, 4)
FROM[dbo].[RemoteEmployee] WHERE SUBSTRING(Re_Number,1,4) = @YR";

                        var Re_Number = await _genericRepository.GetAsync<string>(sQuery, null);


                        sQuery = $@"INSERT INTO [dbo].[RemoteEmployee]
                           ([Hcode],[Name],[EmailID],[CheckinDateTime]
                             ,[Status]
                             ,[Re_Number]
                           ,[CheckOutDateTime],[IsVehicalParkedOnPremises],[VehicalNumber],[Comments],[CreatedBy],[UpdatedBy],
                            [CreatedDate],[UpdatedDate])
                     VALUES
                           (@Hcode,@Name,@EmailID,@CheckinDateTime,
                            @Status,
                            @Re_Number,
                            @CheckOutDateTime,@IsVehicalParkedOnPremises,@VehicalNumber,@Comments,@CreatedBy,@UpdatedBy,
                            @CreatedDate,@UpdatedDate); SELECT SCOPE_IDENTITY();";

                        DynamicParameters param = new DynamicParameters();
                        param.Add("@Hcode", remoteEmployee.Hcode);
                        param.Add("@Name", remoteEmployee.Name);
                        param.Add("@EmailID", remoteEmployee.EmailID);
                        param.Add("@IsVehicalParkedOnPremises", remoteEmployee.IsVehicalParkedOnPremises);
                        param.Add("@Comments", remoteEmployee.Comments);
                        param.Add("@UpdatedBy", remoteEmployee.UpdatedBy);
                        param.Add("@CreatedBy", remoteEmployee.CreatedBy);
                        param.Add("@CreatedDate", remoteEmployee.CreatedDate);
                        param.Add("@UpdatedDate", remoteEmployee.UpdatedDate);
                        param.Add("@CheckinDateTime", StartDateFInal);
                        param.Add("@CheckOutDateTime", NewEndDate);
                        param.Add("@VehicalNumber", remoteEmployee.VehicalNumber);
                        param.Add("@Status", remoteEmployee.Status);
                        param.Add("@Re_Number", @Re_Number.FirstOrDefault());
                        //var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);
                        var result = await _genericRepository.GetAsync<int>(sQuery, param);
                        result.FirstOrDefault();
                    }
                    return 1;
                }
            }
            catch (Exception ex)
            {
                DB.insertErrorlog("remoteEmployee", "SaveRemoteEmployee", ex.Message, 1);
                return 0;
            }
        }



        ///Secuirty Check
        public async Task<int> SaveSecurityCheck(RemoteEmployeeSecurityCheck remoteEmployee)
        {
            var sQuery = @"";

            try
            {

                sQuery = $@"UPDATE [dbo].[RemoteEmployee]
                           SET 
                              [GuestAccessCardIssue] = @GuestAccessCardIssue
                              ,[AccessCardCollectionStatus] = @AccessCardCollectionStatus
                              ,[Escalation] = @Escalation 
                              ,[DeafultGuestCardNumber] = @DeafultGuestCardNumber                             
                              ,[CreatedBySC] = @CreatedBySC
                              ,[UpdatedBySC] = @UpdatedBySC
                              ,[CreatedDateSC] = @CreatedDateSC
                              ,[UpdatedDateSC] = @UpdatedDateSC     
                              ,[CheckOutDateTime]=@CheckOutDateTime
                              ,[Status]=@Status
                         WHERE [Pkey] = @Pkey";



                DynamicParameters param = new DynamicParameters();
                param.Add("@AccessCardCollectionStatus", remoteEmployee.AccessCardCollectionStatus);
                param.Add("@GuestAccessCardIssue", remoteEmployee.GuestAccessCardIssue);
                param.Add("@Escalation", remoteEmployee.Escalation);
                param.Add("@DeafultGuestCardNumber", remoteEmployee.DeafultGuestCardNumber);
                param.Add("@CreatedBySC", remoteEmployee.CreatedBySC);
                param.Add("@UpdatedBySC", remoteEmployee.UpdatedBySC);
                param.Add("@CreatedDateSC", remoteEmployee.CreatedDateSC);
                param.Add("@UpdatedDateSC", remoteEmployee.UpdatedDateSC);
                param.Add("@Pkey", remoteEmployee.Pkey);
                param.Add("@CheckOutDateTime", remoteEmployee.CheckOutDateTime);
                param.Add("@Status", remoteEmployee.Status);
                var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);
                ///var result = await _genericRepository.GetAsync<int>(sQuery, param);

                return result;


            }
            catch (Exception ex)
            {
                DB.insertErrorlog("remoteEmployee", "SaveSecurityCheck", ex.Message, 1);
                return 0;
            }
        }


        ///End Security Check

    }
}