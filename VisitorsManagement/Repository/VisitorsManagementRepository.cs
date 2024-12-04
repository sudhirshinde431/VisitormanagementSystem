using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitors_Management.Dto.VM;
using VisitorsManagement;
using VisitorsManagement.Models;
using VisitorsManagement.Repository;

namespace VisitorsManagement.Repository
{
    public class VisitorsManagementRepository : IVisitorsManagementRepository
    {
        private readonly IGenericRepository _genericRepository;

        public VisitorsManagementRepository(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<int> RaiseAppointment(VM visitor)
        {
            var sQuery = @"";

            try
            {
                if (visitor.AppointmentID > 0)
                {
                    sQuery = $@"UPDATE [dbo].[tbl_VM_Appointment]
                           SET 
                              [VisitorName] = @VisitorName
                              ,[VisitorPhoneNumber] = @VisitorPhoneNumber
                              ,[RepresentingCompany] = @RepresentingCompany
                              ,[Address] = @Address
                              ,[PersonToVisitID] = @PersonToVisitID
                              ,[PurposeToVisit] = @PurposeToVisit
                              ,[Date] = @Date
                              ,[VisitorsEmails] = @VisitorsEmails
                              ,[NumberOfPerson] = @NumberOfPerson
                              ,[Remark] = @Remark
                              ,[Status] = @Status
                              ,[UpdatedBy] = @UpdatedBy
                              ,[UpdatedDate] = @UpdatedDate
                         WHERE [AppointmentID] = @AppointmentID";



                    DynamicParameters param = new DynamicParameters();
                    param.Add("@VisitorName", visitor.VisitorName);
                    param.Add("@VisitorPhoneNumber", visitor.VisitorPhoneNumber);
                    param.Add("@RepresentingCompany", visitor.RepresentingCompany);
                    param.Add("@Address", visitor.Address);
                    param.Add("@PersonToVisitID", visitor.PersonToVisitID);
                    param.Add("@PurposeToVisit", visitor.PurposeToVisit);
                    param.Add("@Date", visitor.Date);
                    param.Add("@VisitorsEmails", visitor.VisitorsEmails);
                    param.Add("@NumberOfPerson", visitor.NumberOfPerson);
                    param.Add("@Remark", visitor.Remark);
                    param.Add("@Status", visitor.Status);
                    param.Add("@AppointmentID", visitor.AppointmentID);
                    param.Add("@UpdatedBy", visitor.UpdatedBy);
                    param.Add("@UpdatedDate", visitor.UpdatedDate);

                    var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);
                    ///var result = await _genericRepository.GetAsync<int>(sQuery, param);

                    return result;
                }
                else
                {

                    sQuery = @"DECLARE @YR VARCHAR(10)
                                SET @YR = 'VS' + CAST((YEAR(GETDATE()) % 100) AS VARCHAR)
                                SELECT @YR +right(1000000 + ISNULL(MAX(SUBSTRING(APPOINTMENTNO, 5, LEN(APPOINTMENTNO) - 1)), 0) + 1, 4) FROM[dbo].[tbl_VM_Appointment] WHERE SUBSTRING(APPOINTMENTNO,1,4) = @YR";

                    var appointmentNo = await _genericRepository.GetAsync<string>(sQuery, null);

                    sQuery = $@"INSERT INTO [dbo].[tbl_VM_Appointment]
                           ([AppointmentNo],[VisitorName],[VisitorPhoneNumber],[RepresentingCompany]
                           ,[Address],[PersonToVisitID],[PurposeToVisit],[Date],[VisitorsEmails],[Remark],[NumberOfPerson],[Status],[CreatedBy],[CreatedDate])
                     VALUES
                           (@AppointmentNo,@VisitorName,@VisitorPhoneNumber,@RepresentingCompany,
                            @Address,@PersonToVisitID,@PurposeToVisit,@Date,@VisitorsEmails,@Remark,@NumberOfPerson,@Status,@CreatedBy,@CreatedDate); SELECT SCOPE_IDENTITY();";

                    DynamicParameters param = new DynamicParameters();
                    param.Add("@AppointmentNo", appointmentNo.FirstOrDefault());
                    param.Add("@VisitorName", visitor.VisitorName);
                    param.Add("@VisitorPhoneNumber", visitor.VisitorPhoneNumber);
                    param.Add("@RepresentingCompany", visitor.RepresentingCompany);
                    param.Add("@Address", visitor.Address);
                    param.Add("@PersonToVisitID", visitor.PersonToVisitID);
                    param.Add("@PurposeToVisit", visitor.PurposeToVisit);
                    param.Add("@VisitorsEmails", visitor.VisitorsEmails);
                    param.Add("@Remark", visitor.Remark);
                    param.Add("@NumberOfPerson", visitor.NumberOfPerson);
                    param.Add("@Status", visitor.Status);
                    param.Add("@Date", visitor.Date);
                    param.Add("@CreatedBy", visitor.CreatedBy);
                    param.Add("@CreatedDate", visitor.CreatedDate);

                    //var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);
                    var result = await _genericRepository.GetAsync<int>(sQuery, param);

                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                DB.insertErrorlog("VMRepository", "RaiseAppointment", ex.Message, 1);
                return 0;
            }
        }

        public async Task<IEnumerable<VM>> GetVisitors(VMFilter filter)
        {
            DateTime currentDate = DB.getCurrentIndianDate();

            var sQuery = $@"SELECT  RepresentingCompany,CONVERT(char(10), Date,126) as DateInGlobalFormate,VisitorPhoneNumber,AppointmentId,AppointmentNo,Replace(convert(char(11),Date,106),' ', '-') as 'strDate',VisitorName,U.FirstName + ' ' + U.LastName 'PersonToVisitName',InTime,OutTime,NumberOfPerson,ISNULL(Status,'') 'Status',
                            'IsCheckInToday' = CASE WHEN DATEDIFF(DAY, CAST(Date AS DATE), CAST('{currentDate.ToString("dd-MMM-yyyy")}' AS DATE)) = 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END FROM tbl_VM_Appointment VM LEFT JOIN tbl_Users U 
                            ON VM.PersonToVisitID = U.UserID";
            if(filter.ForAutocomplete== "ForAutocomplete")
            {
                sQuery = $@"SELECT VM.VisitorName,VM.VisitorPhoneNumber, MAX(AppointmentID) as AppointmentID
                             FROM tbl_VM_Appointment VM LEFT JOIN tbl_Users U 
                            ON VM.PersonToVisitID = U.UserID";
							
            }

            if (filter.AppointmentId > 0)
            {
                sQuery = $@"SELECT AppointmentNo,Replace(convert(char(11),Date,106),' ', '-') as 'strDate', VM.*,U.FirstName + ' ' + U.LastName 'PersonToVisitName',InTime,OutTime,NumberOfPerson,ISNULL(Status,'') 'Status',U.EmailID 'PersonToVisitEmailID',BatchNumber,GatePassNumber,
                            'IsCheckInToday' = CASE WHEN DATEDIFF(DAY, CAST(Date AS DATE), CAST('{currentDate.ToString("dd-MMM-yyyy")}' AS DATE)) = 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END FROM tbl_VM_Appointment VM LEFT JOIN tbl_Users U 
                            ON VM.PersonToVisitID = U.UserID";
                sQuery = sQuery + $" WHERE AppointmentId = { filter.AppointmentId }";
            }
            else if (filter.UserId > 0)
                sQuery = sQuery + $" WHERE PersonToVisitID = { filter.UserId }";
            else if (!string.IsNullOrEmpty(filter.FilterText))
                sQuery = sQuery + $" WHERE VisitorName LIKE '%{ filter.FilterText }%' OR U.FirstName + ' ' + U.LastName LIKE '%{ filter.FilterText }%' OR AppointmentNo LIKE '%{ filter.FilterText }%' OR GatePassNumber LIKE '%{ filter.FilterText }%' OR Replace(convert(char(11),Date,106),' ', '-') LIKE '%{ filter.FilterText }%'";

            if (filter.ForAutocomplete != "ForAutocomplete")
            {
                sQuery = sQuery + " ORDER BY AppointmentId DESC";
            }
            if (filter.ForAutocomplete == "ForAutocomplete")
            {
                sQuery = sQuery + " GROUP BY VM.VisitorName,VM.VisitorPhoneNumber";

            }


            var result = await _genericRepository.GetAsync<VM>(sQuery, null);

            return result;
        }

        public async Task<int> CheckInAppointment(VM_CheckIn visitor)
        {
            var sQuery = @"";
            try
            {
                sQuery = $@"UPDATE [dbo].[tbl_VM_Appointment] 
                            SET InTime = @InTime 
                            ,Status = @Status
                            ,NumberOfPerson = @NumberOfPerson 
                            ,GatePassNumber = @GatePassNumber 
                            ,MaterialDetails = @MaterialDetails 
                            ,VehicleDetails = @VehicleDetails 
                            ,[UpdatedBy] = @UpdatedBy
                            ,[UpdatedDate] = @UpdatedDate
                            WHERE AppointmentID = @AppointmentID";



                DynamicParameters param = new DynamicParameters();
                param.Add("@InTime", visitor.Time);
                param.Add("@Status", visitor.Status);
                param.Add("@NumberOfPerson", visitor.NumberOfPerson);
                param.Add("@GatePassNumber", visitor.GatePassNumber);
                param.Add("@MaterialDetails", visitor.MaterialDetails);
                param.Add("@VehicleDetails", visitor.VehicleDetails);
                param.Add("@UpdatedBy", visitor.UpdatedBy);
                param.Add("@UpdatedDate", visitor.UpdatedDate);
                param.Add("@AppointmentID", visitor.AppointmentID);

                var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> ApproveRejectAppointment(VM_ApproveRejectAppointment visitor)
        {
            var sQuery = @"";
            try
            {
                sQuery = $@"UPDATE [dbo].[tbl_VM_Appointment] 
                            SET  
                             Status = @Status 
                            ,[UpdatedBy] = @UpdatedBy
                            ,[UpdatedDate] = @UpdatedDate
                            WHERE AppointmentID = @AppointmentID";

                //,[UpdatedBy] = @UpdatedBy
                //              ,[UpdatedDate] = @UpdatedDate

                DynamicParameters param = new DynamicParameters();
                //param.Add("@InTime", visitor.Time);
                param.Add("@Status", visitor.Status);
                param.Add("@UpdatedBy", visitor.UpdatedBy);
                param.Add("@UpdatedDate", visitor.UpdatedDate);
                param.Add("@AppointmentID", visitor.AppointmentID);

                var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> CheckOutAppointment(VM_CheckOut visitor)
        {
            var sQuery = @"";
            try
            {
                sQuery = $@"UPDATE [dbo].[tbl_VM_Appointment]
                            SET OutTime = @InOut
                            ,Status = @Status
                            ,Narration = @Narration 
                            ,[UpdatedBy] = @UpdatedBy
                            ,[UpdatedDate] = @UpdatedDate
                            WHERE AppointmentID = @AppointmentID";

                //,[UpdatedBy] = @UpdatedBy
                //              ,[UpdatedDate] = @UpdatedDate

                DynamicParameters param = new DynamicParameters();
                param.Add("@InOut", visitor.Time);
                param.Add("@Status", visitor.Status);
                param.Add("@Narration", visitor.Narration);
                param.Add("@UpdatedBy", visitor.UpdatedBy);
                param.Add("@UpdatedDate", visitor.UpdatedDate);
                param.Add("@AppointmentID", visitor.AppointmentID);

                var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> CancelAppointment(VM_CancelAppointment visitor)
        {
            var sQuery = @"";
            try
            {
                sQuery = $@"UPDATE [dbo].[tbl_VM_Appointment] SET
                            Status = @Status 
                            ,[UpdatedBy] = @UpdatedBy
                            ,[UpdatedDate] = @UpdatedDate
                            WHERE AppointmentID = @AppointmentID";

                //,[UpdatedBy] = @UpdatedBy
                //              ,[UpdatedDate] = @UpdatedDate

                DynamicParameters param = new DynamicParameters();
                param.Add("@Status", visitor.Status);
                param.Add("@UpdatedBy", visitor.UpdatedBy);
                param.Add("@UpdatedDate", visitor.UpdatedDate);
                param.Add("@AppointmentID", visitor.AppointmentID);

                var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<int> RejectAppointment(VM_RejectAppointment visitor)
        {
            var sQuery = @"";
            try
            {
                sQuery = $@"UPDATE [dbo].[tbl_VM_Appointment] 
                            Status = @Status 
                            ,[UpdatedBy] = @UpdatedBy
                            ,[UpdatedDate] = @UpdatedDate
                            WHERE AppointmentID = @AppointmentID";

                //,[UpdatedBy] = @UpdatedBy
                //              ,[UpdatedDate] = @UpdatedDate

                DynamicParameters param = new DynamicParameters();
                param.Add("@Status", visitor.Status);
                param.Add("@UpdatedBy", visitor.UpdatedBy);
                param.Add("@UpdatedDate", visitor.UpdatedDate);
                param.Add("@AppointmentID", visitor.AppointmentID);

                var result = await _genericRepository.ExecuteCommandAsync(sQuery, param);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<VM_ChartResponse>> GenerateChart(VM_Chart filter)
        {

            if (filter.type == "Daily")
            {
                DateTime baseDate = DB.getCurrentIndianDate().Date;

                var sQuery = $@"SELECT Date,[Open] as 'OpenCnt',[Cancelled] as 'CancelledCnt',[Checked In] as 'CheckedInCnt',[Checked Out] as 'CheckedOutCnt',[Rejected] as 'RejectedCnt'
                            FROM
                            (
                                SELECT Replace(convert(char(11),Date,106),' ', '-') as 'Date',
			                               [status]
                                FROM  [dbo].[tbl_VM_Appointment]   WHERE CAST(Date AS DATE) = CAST(@currentDate AS DATE)
                            ) AS SourceTable PIVOT(count([status]) FOR [status] IN([Open],
                                                                                     [Cancelled],
                                                                                     [Checked In],
                                                                                     [Checked Out],
                                                                                     [Rejected])) AS PivotTable order by date;";

                var parameters = new DynamicParameters();
                parameters.Add("@currentDate", baseDate);

                var result = await _genericRepository.GetAsync<VM_ChartResponse>(sQuery, parameters);

                List<VM_ChartResponse> lst = new List<VM_ChartResponse>();
                if (result.Count() > 0)
                {
                    lst.Add(new VM_ChartResponse() { Date = "Open", OpenCnt = result.ToList()[0].OpenCnt });
                    lst.Add(new VM_ChartResponse() { Date = "Cancelled", OpenCnt = result.ToList()[0].CancelledCnt });
                    lst.Add(new VM_ChartResponse() { Date = "Checked In", OpenCnt = result.ToList()[0].CheckedInCnt });
                    lst.Add(new VM_ChartResponse() { Date = "Checked Out", OpenCnt = result.ToList()[0].CheckedOutCnt });
                    // lst.Add(new VM_ChartResponse() { Date = "Checked Out", OpenCnt = result.ToList()[0].CheckedOutCnt });
                    lst.Add(new VM_ChartResponse() { Date = "Rejected", OpenCnt = result.ToList()[0].RejectedCnt });
                }

                return lst;

            }
            else if (filter.type == "Weekly")
            {
                DateTime startdate = new DateTime();
                DateTime enddate = new DateTime();
                DateTime baseDate = DB.getCurrentIndianDate();

                var today = baseDate;
                var yesterday = baseDate.AddDays(-1);
                startdate = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                enddate = startdate.AddDays(7).AddSeconds(-1);

                var sQuery = $@"SELECT Date,[Open] as 'OpenCnt',[Cancelled] as 'CancelledCnt',[Checked In] as 'CheckedInCnt',[Checked Out] as 'CheckedOutCnt',[Rejected] as 'RejectedCnt'
                            FROM
                            (
                                SELECT Replace(convert(char(11),Date,106),' ', '-') as 'Date',
			                               [status]
                                FROM  [dbo].[tbl_VM_Appointment]  WHERE CAST(Date AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)
                            ) AS SourceTable PIVOT(count([status]) FOR [status] IN([Open],
                                                                                     [Cancelled],
                                                                                     [Checked In],
                                                                                     [Checked Out],[Rejected])) AS PivotTable order by date;";

                var parameters = new DynamicParameters();
                parameters.Add("@StartDate", startdate);
                parameters.Add("@EndDate", enddate);

                try
                {
                    var result = await _genericRepository.GetAsync<VM_ChartResponse>(sQuery, parameters);

                    List<VM_ChartResponse> lst = new List<VM_ChartResponse>();

                    for (DateTime i = startdate; i < enddate; i = i.AddDays(1))
                    {
                        var r = result.Where(x => Convert.ToDateTime(x.Date) == i.Date).FirstOrDefault();

                        if (r != null)
                            lst.Add(r);
                        else
                            lst.Add(new VM_ChartResponse() { Date = i.ToString("dd-MMM-yyyy"), CompareDate = i, OpenCnt = 0, CancelledCnt = 0, CheckedInCnt = 0, CheckedOutCnt = 0 });
                    }

                    return lst;
                }
                catch (Exception ex)
                {

                    throw;
                }



            }
            else if (filter.type == "Monthly")
            {
                DateTime baseDate = DB.getCurrentIndianDate();
                int mth = baseDate.Month;
                int year = baseDate.Year;

                DateTime startdate = new DateTime();
                DateTime enddate = new DateTime();

                startdate = new DateTime(baseDate.Year, baseDate.Month, 1);
                enddate = startdate.AddMonths(1).AddDays(-1);

                var sQuery = @"SELECT Date,[Open] as 'OpenCnt',[Cancelled] as 'CancelledCnt',[Checked In] as 'CheckedInCnt',[Checked Out] as 'CheckedOutCnt',[Rejected] as 'RejectedCnt'
                            FROM
                            (
                                SELECT Replace(convert(char(11),Date,106),' ', '-') as 'Date',
			                               [status]
                                FROM  [dbo].[tbl_VM_Appointment]  WHERE month([Date]) = @MTH and year([date]) = @YR 
                            ) AS SourceTable PIVOT(count([status]) FOR [status] IN([Open],
                                                                                     [Cancelled],
                                                                                     [Checked In],
                                                                                     [Checked Out],[Rejected])) AS PivotTable order by date;";

                var parameters = new DynamicParameters();
                parameters.Add("@MTH", mth);
                parameters.Add("@YR", year);

                var result = await _genericRepository.GetAsync<VM_ChartResponse>(sQuery, parameters);

                List<VM_ChartResponse> lst = new List<VM_ChartResponse>();

                for (DateTime i = startdate; i < enddate; i = i.AddDays(1))
                {
                    var r = result.Where(x => Convert.ToDateTime(x.Date) == i.Date).FirstOrDefault();

                    if (r != null)
                        lst.Add(r);
                    else
                        lst.Add(new VM_ChartResponse() { Date = i.ToString("dd-MMM-yyyy"), CompareDate = i, OpenCnt = 0, CancelledCnt = 0, CheckedInCnt = 0, CheckedOutCnt = 0 });
                }

                return lst;

            }
            return new List<VM_ChartResponse>();
            //else if (filter.UserId > 0)
            //    sQuery = sQuery + $" WHERE PersonToVisitID = { filter.UserId }";
            //else if (!string.IsNullOrEmpty(filter.FilterText))
            //    sQuery = sQuery + $" WHERE VisitorName LIKE '%{ filter.FilterText }%' OR U.FirstName + ' ' + U.LastName LIKE '%{ filter.FilterText }%'";




        }

        public async Task<Tuple<string, string>> GetVisitingPersonEmail(int userId)
        {
            var sQuery = @"SELECT EmailID,FirstName + ' ' +LastName as 'FullName' FROM tbl_Users WHERE UserID = @UserID";

            var parameter = new DynamicParameters();
            parameter.Add("@UserID", userId);

            var result = await _genericRepository.GetAsync<CurrentUserDto>(sQuery, parameter);

            string name = result.FirstOrDefault().FullName;
            string EmailId = result.FirstOrDefault().EmailID;

            var tuple = Tuple.Create(name, EmailId);

            return tuple;
        }

        public async Task<int> ClosePreviousAppointments()
        {
            DateTime baseDate = DB.getCurrentIndianDate().Date.AddDays(-1);

            var sQuery = @"UPDATE [dbo].[tbl_VM_Appointment] 
                            SET OutTime = '11.59 PM', Narration = 'System Checked Out', Status = 'Checked Out'
                            where cast(date as date) = @Date  AND Status = 'Checked In'";

            var parameters = new DynamicParameters();
            parameters.Add("@Date", baseDate);

            var result = await _genericRepository.ExecuteCommandAsync(sQuery, parameters);

            return result;
        }

        public async Task<VM> GetVisitorByWPID(int WPID)
        {
            DateTime currentDate = DB.getCurrentIndianDate();


            var sQuery = $@"SELECT AppointmentNo,Replace(convert(char(11),Date,106),' ', '-') as 'strDate', VM.*,U.FirstName + ' ' + U.LastName 'PersonToVisitName',InTime,OutTime,NumberOfPerson,ISNULL(Status,'') 'Status',U.EmailID 'PersonToVisitEmailID',BatchNumber,GatePassNumber,
                            'IsCheckInToday' = CASE WHEN DATEDIFF(DAY, CAST(Date AS DATE), CAST('{currentDate.ToString("dd-MMM-yyyy")}' AS DATE)) = 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END FROM tbl_VM_Appointment VM LEFT JOIN tbl_Users U 
                            ON VM.PersonToVisitID = U.UserID";
            sQuery = sQuery + $" WHERE WPID = { WPID }";

            var result = await _genericRepository.GetAsync<VM>(sQuery, null);

            return result.FirstOrDefault();
        }
    }
}
