CReate table RemoteEmployee
(
Pkey int primary key IDENTITY(1,1) NOT NULL, 
Hcode nvarchar(100),
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
(Hcode,Name,EmailID,CheckinDateTime,CheckOutDateTime,IsVehicalParkedOnPremises,VehicalNumber,Comments)
Select 'dt226537','Sudhir Shinde','sxshinde@sscinc.com',GETDATE(),GETDATE(),1,'MH14LA7460','Coming for office visit'