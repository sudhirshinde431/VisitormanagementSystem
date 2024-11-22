CReate table RemoteEmployee
(
Pkey int primary key IDENTITY(1,1) NOT NULL, 
Guest nvarchar(100),
Name nvarchar(400),
EmailID nvarchar(400),
CheckinDateTime DateTime,
CheckOutDateTime DateTime,
IsVehicalParkedOnPremises BIT,
VehicalNumber  nvarchar(400),
Comments  nvarchar(4000),
CreatedBy BIGINT,
UpdatedBy BIGINT,
CreatedDate  nvarchar(20),
UpdatedDate  nvarchar(20),
)

insert  into RemoteEmployee
(Hcode,Name,EmailID,CheckinDateTime,CheckOutDateTime,IsVehicalParkedOnPremises,VehicalNumber,Comments,CreatedBy,UpdatedBy)
Select 'dt226537','Sudhir Shinde','sxshinde@sscinc.com',GETDATE(),GETDATE(),1,'MH14LA7460','Coming for office visit',1,1




Insert into tbl_UserAccess
(UserID,Claim,IsDeleted)
Select 1,'ReadRE',1
UNION ALL
Select 1,'UpdateRE',1

UNION ALL
Select 1,'ReadSC',1
UNION ALL
Select 1,'UpdateSC',1