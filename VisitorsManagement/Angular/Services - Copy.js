app.service("myService", function ($http) {

    this.VM_getAllAppointment = function (filter) {

        var response = $http({
            method: "post",
            data: { filter: filter },
            url: "/VM/getAllAppointment",
            dataType: "json"
        });
        return response;
    }


    this.VM_SearchAppointment = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "/VM/VM_SearchAppointment",
            dataType: "json"
        });
        return response;
    }

    this.VM_saveAppointment = function (VM_saveAppointment) {
        var response = $http({
            method: 'post',
            url: '/VM/raiseAppointment',
            data: { VM: VM_saveAppointment },
            dataType: "json"
        });

        return response;
    }

     this.VM_CheckIn = function (checkInAppointment) {
        var response = $http({
            method: 'post',
            url: '/VM/CheckInAppointment',
            data: { filter: checkInAppointment },
            dataType: "json"
        });

        return response;
     }

     this.VM_CheckOut = function (checkOutAppointment) {
         var response = $http({
             method: 'post',
             url: '/VM/CheckOutAppointment',
             data: { filter: checkOutAppointment },
             dataType: "json"
         });

         return response;
     }

     this.VM_CancelAppointment = function (cancelAppointment) {
         var response = $http({
             method: 'post',
             url: '/VM/CancelAppointment',
             data: { filter: cancelAppointment },
             dataType: "json"
         });

         return response;
     }
     

     this.getUserDropdown = function () {
         var response = $http({
             method: 'post',
             url: '/Users/GetUserSelectList',
             dataType: "json"
         });

         return response;
     }

     this.GetUserSelectListOnlyEmployee = function () {
         var response = $http({
             method: 'post',
             url: '/Users/GetUserSelectListOnlyEmployee',
             dataType: "json"
         });

         return response;
     }

     this.GenerateChart = function (chartFilter) {
         var response = $http({
             method: 'post',
             data: { filter: chartFilter },
             url: '/VM/GenerateChart',
             dataType: "json"
         });

         return response;
     }



     this.ApproveRejectAppointment = function (approveReject) {
         var response = $http({
             method: "post",
             url: "/VM/ApproveRejectAppointment",
             data: { filter: approveReject },
             dataType: "json"
         })
         return response;
     }



    //=========================================================== Work Permit ===============================

     this.getAllWorkPermit = function (filter) {

         var response = $http({
             method: "post",
             data: { filter: filter },
             url: "/WP/getAllWorkPermit",
             dataType: "json"
         });
         return response;
     }

     this.getContractorSelectList = function () {

         var response = $http({
             method: "post",
             url: "/Contractor/getContractorSelectList",
             dataType: "json"
         });
         return response;
     }

     this.ApproveRejectWP = function (approveReject) {

         var response = $http({
             method: "post",
             data: { approveRejectWP: approveReject },
             url: "/WP/ApproveRejectWP",
             dataType: "json"
         });
         return response;
     }


     this.getEmployeeFile = function (employeeDetails) {

         var response = $http({
             method: "post",
             data: { details: employeeDetails },
             url: "/WP/getWPFile",
             dataType: "json"
         });
         return response;
     }







    //==============User Management ================================================================


     this.GetDefaultClains = function () {

         var response = $http({
             method: "post",
             url: "/Users/GetDefaultClaims",
             dataType: "json"
         });
         return response;
     }


     this.SaveUser = function (userDetails) {

         var response = $http({
             method: "post",
             data: { user: userDetails },
             url: "/Users/CreateUser",
             dataType: "json"
         });
         return response;
     }



     this.GetUsers = function (filter) {

         var response = $http({
             method: "post",
             data: { filter: filter },
             url: "/Users/GetUsers",
             dataType: "json"
         });
         return response;
     }







     this.ChangePassword = function (userModel) {
         var response = $http({
             method: 'post',
             params: { change: userModel },
             url: "/Login/ChangePassword",
             headers: {
                 'Content-Type': 'application/json'
             }
         });
         return response;
     }






























    this.getModuleWiseUserAccess = function (PageID, ModuleName) {
        var response = $http({
            method: "get",
            url: "/Login/getModuleWiseUserAccess",
            params: { PageID: PageID, ModuleName: ModuleName },
            dataType: "json"
        })
        return response;
    }

    //dashboard
    this.GetMeasuringToolChart = function () {
        var response = $http({
            method: "get",
            url: "/Home/GetMeasuringToolChart",
            dataType: "json"
        })
        return response;
    }
    // Tool In use ot scrap chart
    this.GetInUseScrapToolChart = function () {
        var response = $http({
            method: "get",
            url: "/Home/GetInUseScrapToolChart",
            dataType: "json"
        })
        return response;
    }

    //calibration due date chart
    this.getDueForCalibrationChart = function () {
        var response = $http({
            method: "get",
            url: "/Home/getDueForCalibrationChart",
            dataType: "json"
        })
        return response;
    }
    //monthly calibration Chart
    this.MonthlyCalibrationChart = function () {
        var response = $http({
            method: "get",
            url: "/Home/MonthlyCalibrationChart",
            dataType: "json"
        })
        return response;
    }

    //weekly calibration Chart
    this.WeeklyCalibrationChart = function (Date) {
        var response = $http({
            method: "get",
            url: "/Home/WeeklyCalibrationChart",
            params: { Date: Date },
            dataType: "json"
        })
        return response;
    }

    this.getDropDownValue = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "/Home/getDropDownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }


    this.getDropDownValueForArea = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../Home/getDropDownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    //code fr save article details
    this.SaveArticleDetails = function (SaveArticleDetails) {
        var response = $http({
            method: 'post',
            url: '/Home/SaveArticleDetails',
            data: { rec: SaveArticleDetails },
            dataType: "json"
        });

        return response;
    }



    //  code for edit the deatils of articles
    this.getAllArticleDtl = function (Id) {

        var response = $http({
            method: 'get',
            params: { ArticleId: Id },
            url: '/Home/getAllArticleDtl',
            dataType: "json"
        });
        return response;
    }


    // code For ISO Reference
    this.dropdownISOStdRef = function () {
        var response = $http({
            method: 'post',
            url: '/Home/dropdownISOStdRef',
            dataType: "json"
        });
        return response;
    }

    //DropDown For Article Reference
    this.dropdownLocation = function () {
        var response = $http({
            method: 'post',
            url: '/Home/dropdownLocation',
            dataType: "json"
        });

        return response;
    }

    //search article by description and article label

    this.SearchArticle = function (SearchParameter) {
        var response = $http({
            method: "get",
            params: { SearchParameter: SearchParameter },
            url: '/Home/SearchArticle',
            dataType: "json"
        });
        return response;
    }


    //Code for Saving the Covenant Register Details

    this.SaveCovenantRegisterDtl = function (SaveCovenantRegisterDtl) {
        var response = $http({
            method: 'post',
            url: '/Home/SaveCovenantRegisterDtl',
            data: { CR: SaveCovenantRegisterDtl },
            dataType: "json"
        });

        return response;
    }

    this.getCovRegisterDropDwon = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "/Home/getCovRegisterDropDwon",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    //DropDown For Article Reference
    this.dropdownArticleRef = function () {
        var response = $http({
            method: 'post',
            url: '/Home/dropdownArticleRef',
            dataType: "json"
        });

        return response;
    }

    this.dropDowncovCallibratedBy = function () {
        var response = $http({

            method: "get",
            url: "/Home/dropDowncovCallibratedBy",
            dataType: "json"
        });
        return response;
    }


    //dropdown For authorized Person
    this.getAllUsersForDropdown = function () {
        var response = $http({
            method: 'post',
            url: '/Home/getAllUsersForDropdown',
            dataType: "json"
        });

        return response;
    }

    //code For Edit  CovenantRegisterDetails
    this.GetAllCovenantRegisterDetails = function (Id) {

        var response = $http({
            method: 'get',
            params: { CovId: Id },
            url: '/Home/GetAllCovenantRegisterDetails',
            dataType: "json"
        });
        return response;
    }

    //code for search by SrNo, ArticleLabel & Covenant Number

    this.SearchCovenantRegister = function (SearchParameter) {
        var response = $http({
            method: "get",
            params: { SearchParameter: SearchParameter },
            url: '/Home/SearchCovenantRegister',
            dataType: "json"
        });
        return response;
    }


    //code For Change Password
    this.ChangePassword = function (oldPassword, newPassword) {
        var response = $http({
            method: 'post',
            params: { oldPassword: oldPassword, newPassword: newPassword },
            url: "/Login/ChangePassword",
            headers: {
                'Content-Type': 'application/json'
            }
        });
        return response;
    }




    //-------------code For ISO Standards---------------

    // code for Saving details of ISO Standards
    this.SaveISOStandardDetails = function (SaveISOStandardDetails) {
        var response = $http({
            method: 'post',
            url: '/ISOStandard/SaveISOStandardDetails',
            data: { rec: SaveISOStandardDetails },
            dataType: "json"
        });

        return response;
    }

    //code For Edit  ISO Standard
    this.GetAllISOStandardDetails = function (Id) {

        var response = $http({
            method: 'get',
            params: { ISOID: Id },
            url: '/ISOStandard/GetAllISOStandardDetails',
            dataType: "json"
        });
        return response;
    }

    //code For Search ISO standards
    this.SearchISOStandard = function (SearchParameter) {
        var response = $http({
            method: 'get',
            params: { SearchParameter: SearchParameter },
            url: '/ISOStandard/SearchISOStandard',
            dataType: "json"
        });
        return response;
    }

    this.ISO_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "/ISOStandard/ISO_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }



    //------------ Reports---------------


    //tools and guges Report
    this.GenerateToolsAndGugesReport = function (filename, TypeId,  ZoneID) {
        var response = $http({
            method: 'post',
            url: '/Report/GenerateToolsAndGugesReport',
            data: { filename: filename, TypeId: TypeId, ZoneID: ZoneID },
            dataType: "json"
        });

        return response;
    }

    //tools and guges Report
    this.GenerateSupplierGaugeInOutReport = function (filename) {
        var response = $http({
            method: 'post',
            url: '/Report/GenerateSupplierGaugeInOutReport',
            data: { filename: filename },
            dataType: "json"
        });

        return response;
    }

    //Out Registration Report
    this.GenerateCovenantRegistraionOutReport = function (filename) {
        var response = $http({
            method: 'Post',
            url: '/Report/GenerateCovenantRegistraionOutReport',
            data: { filename: filename },
            dataType: "json"
        });

        return response;
    }


    //calibration Report
    this.GenerateCalibratioDueDatesReport = function (filename, fromDate, toDate) {
        var response = $http({
            method: 'post',
            url: '/Report/GenerateCalibratioDueDatesReport',
            data: { filename: filename, fromDate: fromDate, toDate: toDate },
            dataType: "json"
        });

        return response;
    }

    // Calibration Details , Inventory Location , Service Provide and User Details == Code Starts

    this.SaveData = function (Calibrationmodel) {
        var response = $http({
            method: 'post',
            url: '/Home/SaveData',
            data: { Calibrationmodel: SaveData },
            dataType: "json"
        });

        return response;
    }

    this.getAllInventeryLocationDetails = function (Id) {

        var response = $http({
            method: "get",
            params: { LocationId: Id },
            url: "/Home/getAllInventeryLocationDetails",
            dataType: "json"
        });
        return response;
    }

    this.searchInventoryLocation = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "/Home/searchInventoryLocation",
            dataType: "json"
        });
        return response;
    }

    this.SaveCalibrationDetails = function (SaveCalibrationDetails) {
        var response = $http({
            method: 'post',
            url: '/Home/SaveCalibrationDetails',
            data: { CalibrationDetails: SaveCalibrationDetails },
            dataType: "json"
        });

        return response;
    }

    this.dropdownCallibrationdetail = function (promptgroup) {
        var response = $http({

            method: "get",
            params: { promptgroup: promptgroup },
            url: "/Home/dropdownCallibrationdetail",
            dataType: "json"
        })
        return response;
    }

    this.dropdownArticleRef = function () {
        var response = $http({

            method: "get",
            url: "/Home/dropdownArticleRef",
            dataType: "json"
        })
        return response;
    }

    this.dropDownCallibratedBy = function () {
        var response = $http({

            method: "get",
            url: "/Home/dropDownCallibratedBy",
            dataType: "json"
        })
        return response;
    }

    this.getAllCalibrationDetails = function (Id) {

        var response = $http({
            method: "get",
            params: { CalibrationDetailId: Id },
            url: "/Home/getAllCalibrationDetails",
            dataType: "json"
        });
        return response;
    }

    this.searchCalibrationDetails = function (searchparameter) {

        var response = $http({
            method: "get",
            params: { searchparameter: searchparameter },
            url: "/Home/searchCalibrationDetails",
            dataType: "Json"

        });
        return response;
    }

    //code for userprofile

    this.SaveUserDetails = function (UserModel) {
        var response = $http({
            method: 'post',
            url: '/Login/SaveUserDetails',
            data: { UserModel: UserModel },
            dataType: "json"
        });

        return response;
    }


    this.getAllUsers = function (Id) {

        var response = $http({
            method: "get",
            params: { UserID: Id },
            url: "/Login/getAllUsers",
            dataType: "json"
        });
        return response;
    }

    this.DeleteUser = function (Id) {

        var response = $http({
            method: "get",
            params: { UserID: Id },
            url: "/Login/DeleteUser",
            dataType: "json"
        });
        return response;
    }

    this.SearchUser = function (searchparameter) {

        var response = $http({
            method: "get",
            params: { searchparameter: searchparameter },
            url: "/Login/SearchUser",
            dataType: "Json"

        });
        return response;
    }


    this.getNextCalibrationDueDate = function (ArticleId) {

        var response = $http({
            method: "get",
            params: { ArticleId: ArticleId },
            url: "/Home/getNextCalibrationDueDate",
            dataType: "json"
        });
        return response;
    }

    //code for serviceprovider

    this.SaveServiceProvider = function (SaveServiceProvider) {
        var response = $http({
            method: 'post',
            url: '/ServiceProvider/SaveServiceProvider',
            data: { ServiceProvider: SaveServiceProvider },
            dataType: "json"
        });

        return response;
    }

    this.getServiceProviderDropdown = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "/ServiceProvider/getServiceProviderDropdown",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }


    //show list of service provider

    this.getAllServiceProvider = function (Id) {

        var response = $http({
            method: "get",
            params: { ServiceProviderID: Id },
            url: "/ServiceProvider/getAllServiceProvider",
            dataType: "json"
        });
        return response;
    }

    this.searchServiceProvider = function (searchparameter) {

        var response = $http({
            method: "get",
            params: { searchparameter: searchparameter },
            url: "/ServiceProvider/searchServiceProvider",
            dataType: "Json"

        });
        return response;
    }

    // Calibration Details , Inventory Location , Service Provide and User Details == Code End



    //***************************************Wroking Of Area Start Chemical Handling********************************************


    //code for chamical handling
    this.SaveMSDS = function (SaveMSDS) {

        var response = $http({
            method: 'post',
            url: "/MSDS/SaveMSDS",
            data: { MsdsModel: MsdsModel },
            dataType: "Json"
        });
        return response;
    }

    this.CH_getDropdownvalues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "/MSDS/CH_getDropdownvalues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }


    this.getUsersDropdrown = function () {
        var response = $http({
            method: 'post',
            url: "/MSDS/getUsersDropdrown",
            dataType: "Json"

        });
        return response;
    }

    this.dropDownSupplierName = function () {
        var response = $http({
            method: 'post',
            url: "/MSDS/dropDownSupplierName",
            dataType: "Json"

        });
        return response;
    }



    this.getAllMSDS = function (Id) {

        var response = $http({
            method: "get",
            params: { MSDSID: Id },
            url: "/MSDS/getAllMSDS",
            dataType: "json"
        });
        return response;
    }

    this.searchMSDS = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "/MSDS/searchMSDS",
            dataType: "json"
        });
        return response;
    }

    this.SaveChemicalHandlingDetails = function (SaveChemicalHandlingDetails) {

        var response = $http({
            method: 'post',
            url: "/MSDS/SaveChemicalHandlingDetails",
            data: { SaveCHandlingModel: SaveChemicalHandlingDetails },
            dataType: "Json"
        });
        return response;
    }

    this.searchMSDSManage = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "/MSDS/searchMSDSManage",
            dataType: "json"
        });
        return response;
    }

    //****************Operation of chemical handling**************
    //code for chamical handling
    this.SaveMSDS = function (SaveMSDS) {

        var response = $http({
            method: 'post',
            url: "/MSDS/SaveMSDS",
            data: { MsdsModel: MsdsModel },
            dataType: "Json"
        });
        return response;
    }

    this.getUsersDropdrown = function () {
        var response = $http({
            method: 'post',
            url: "/MSDS/getUsersDropdrown",
            dataType: "Json"

        });
        return response;
    }

    this.getAllMSDS = function (Id) {

        var response = $http({
            method: "get",
            params: { MSDSID: Id },
            url: "/MSDS/getAllMSDS",
            dataType: "json"
        });
        return response;
    }

    this.searchMSDS = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "/MSDS/searchMSDS",
            dataType: "json"
        });
        return response;
    }

    this.SaveChemicalHandlingDetails = function (SaveChemicalHandlingDetails) {

        var response = $http({
            method: 'post',
            url: "/MSDS/SaveChemicalHandlingDetails",
            data: { SaveCHandlingModel: SaveChemicalHandlingDetails },
            dataType: "Json"
        });
        return response;
    }

    this.searchMSDSManage = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "/MSDS/searchMSDSManage",
            dataType: "json"
        });
        return response;
    }


    this.CH_ChemicalStatusChart = function () {
        var response = $http({
            method: "get",
            url: "../MSDS/CH_ChemicalStatusChart",
            dataType: "json"
        })
        return response;
    }


    this.CH_NewlyAddedChemicalChart = function () {
        var response = $http({
            method: "get",
            url: "../MSDS/CH_NewlyAddedChemicalChart",
            dataType: "json"
        })
        return response;
    }
    //**************Working ofchemical handling End**********************************************




    //**************Working OF Incident & Accident Area Start*********************************

    this.saveIncidentsAccident = function (saveIncidentsAccident) {
        var response = $http({
            method: 'post',
            url: '../Incidents/saveIncidentsAccident',
            data: { IncidentsModel: saveIncidentsAccident },
            dataType: "json"
        });

        return response;
    }

    this.IA_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../Incidents/IA_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.IA_getDropdownValuesReport = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../IAReport/IA_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.getIAUsersDropdrown = function () {
        var response = $http({
            method: 'post',
            url: "../Incidents/getIAUsersDropdrown",
            dataType: "Json"

        });
        return response;
    }

    this.getAllIncidents = function (Id) {

        var response = $http({
            method: "get",
            params: { IAID: Id },
            url: "../Incidents/getAllIncidents",
            dataType: "json"
        });
        return response;
    }

    this.searchIncidents = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../Incidents/searchIncidents",
            dataType: "json"
        });
        return response;
    }


    this.getAllInvestigation = function (Id) {

        var response = $http({
            method: "get",
            params: { IAID: Id },
            url: "../Incidents/getAllInvestigation",
            dataType: "json"
        });
        return response;
    }

    this.getAllClosure = function (Id) {

        var response = $http({
            method: "get",
            params: { IAID: Id },
            url: "../Incidents/getAllClosure",
            dataType: "json"
        });
        return response;
    }
    //code for charts

    this.IA_MeasureAccidentsReports = function () {
        var response = $http({
            method: "get",
            url: "../Incidents/IA_MeasureAccidentsReports",
            dataType: "json"
        })
        return response;
    }

    this.IA_MeasureIncidentsTypeChart = function () {
        var response = $http({
            method: "get",
            url: "../Incidents/IA_MeasureIncidentsTypeChart",
            dataType: "json"
        })
        return response;
    }

    this.IA_MeasureBodypartsChart = function () {
        var response = $http({
            method: "get",
            url: "../Incidents/IA_MeasureBodypartsChart",
            dataType: "json"
        })
        return response;
    }
    this.IA_MeasureIncidentdays = function () {
        var response = $http({
            method: "get",
            url: "../Incidents/IA_MeasureIncidentdays",
            dataType: "json"
        })
        return response;
    }

    //**************Working OF Incident & Accident Area End*********************************

    //**************Working of Doc Box Area start**********************************************


    // Save DocFile 
    this.SaveDocBoxDetails = function (SaveDocBoxDetails) {
        var response = $http({
            method: 'post',
            url: '../DocBox/SaveDocBoxDetails',
            data: { DBM: SaveDocBoxDetails },
            dataType: "json"
        });

        return response;
    }

    //dropdwon For Prepared By or approved By(User)

    this.DOC_GetUserForDropdown = function () {
        var response = $http({
            method: 'post',
            url: '../DocBox/DOC_GetUserForDropdown',
            dataType: "json"
        });

        return response;
    }

    //DropDown For Doc Status
    this.GetFileGroupDropdown = function (promptGroup) {
        var response = $http({
            method: 'post',
            params: { promptGroup: promptGroup },
            url: '../DocBox/GetFileGroupDropdown',
            dataType: 'json'

        });
        return response;
    }


    // code For file Group Reference
    this.DOC_dropdownFileGroupRef = function () {
        var response = $http({
            method: 'post',
            url: '../DocBox/DOC_dropdownFileGroupRef',
            dataType: "json"
        });
        return response;
    }

    this.DOC_getCutomerDropdown = function () {
        var response = $http({
            method: 'post',
            url: '../DocBox/DOC_getCutomerDropdown',
            dataType: "json"
        });
        return response;
    }


    //Get All Doc File Detail
    this.DOC_getAllDocBox = function (Id) {
        var response = $http({
            method: 'get',
            params: { DocFileID: Id },
            url: '../DocBox/DOC_getAllDocBox',
            dataType: "json"
        });
        return response;
    }

    //search doc box
    this.DOC_SearchDocBox = function (searchparameter) {

        var response = $http({
            method: "get",
            params: { searchparameter: searchparameter },
            url: "../DocBox/DOC_SearchDocBox",
            dataType: "json"

        });
        return response;
    }


    //Move to Archive
    this.MoveToArchive = function (DocFileID, CreateRev) {
        var response = $http({
            method: 'post',
            url: '../DocBox/MoveToArchive',
            data: { DocFileID: DocFileID, CreateRev: CreateRev },
            dataType: "json"
        });
        return response;
    }

    //********************operation of Published Document****************************

    //get all published document(List)

    this.DOC_GetAllPublishedDoc = function (Id) {
        var response = $http({
            method: 'get',
            params: { DocFileID: Id },
            url: '../DocBox/DOC_GetAllPublishedDoc',
            dataType: "json"
        });
        return response;
    }

    //Search For Publish Document 
    this.DOC_SearchForPublishDoc = function (SearchParameter) {

        var response = $http({
            method: "get",
            params: { SearchParameter: SearchParameter },
            url: "../DocBox/DOC_SearchForPublishDoc",
            dataType: "json"

        });
        return response;
    }

    //****************Operation of Archieve Document**************

    this.DOC_getAllDocBox = function (Id) {
        var response = $http({
            method: 'get',
            params: { DocFileID: Id },
            url: '../DocBox/DOC_getAllDocBox',
            dataType: "json"
        });
        return response;
    }


    //edit 
    this.DOC_GetAllArchieveDoc = function (Id) {

        var response = $http({
            method: 'get',
            params: { DocFileID: Id },
            url: '../DocBox/DOC_GetAllArchieveDoc',
            dataType: "json"
        });
        return response;
    }

    //search
    this.DOC_SearchForArchieveDoc = function (searchparameter) {

        var response = $http({
            method: "get",
            params: { searchparameter: searchparameter },
            url: "../DocBox/DOC_SearchForArchieveDoc",
            dataType: "json"

        });
        return response;
    }


    this.DOC_NoOfDocumentsChart = function () {
        var response = $http({
            method: "get",
            url: "../DocBox/DOC_NoOfDocumentsChart",
            dataType: "json"
        })
        return response;
    }


    //**************Working of Doc Box Area End***********************************


    //*************Working of IMS Documents**********************************

    //-----------Editorial Page-------------------------------

    // Save DocFile 
    this.SaveEditorialDetails = function (SaveEditorialDetails) {
        var response = $http({
            method: 'post',
            url: '../IMSDocuments/SaveEditorialDetails',
            data: { IMS: SaveEditorialDetails },
            dataType: "json"
        });
        return response;
    }

    //list of IMSDocument 
    this.IMS_GetAllIMSDocument = function (Id) {
        var response = $http({
            method: 'get',
            params: { IMSDocID: Id },
            url: '../IMSDocuments/IMS_GetAllIMSDocument',
            dataType: "json"
        });
        return response;
    }

    //search on Doc number & title
    this.IMS_SearchEditorialDoc = function (SearchParameter) {
        var response = $http({
            method: "get",
            params: { SearchParameter: SearchParameter },
            url: "../IMSDocuments/IMS_SearchEditorialDoc",
            dataType: "json"

        });
        return response;
    }

    //Move to Archive
    this.MoveToArchive_IMS = function (IMSDocID, CreateRev) {
        var response = $http({
            method: 'post',
            url: '../IMSDocuments/MoveToArchive',
            data: { IMSDocID: IMSDocID, CreateRev: CreateRev },
            dataType: "json"
        });
        return response;
    }

    this.IMS_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../IMSDocuments/IMS_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.dropdownIMS_ISOStdRef = function () {
        var response = $http({
            method: 'post',
            url: "../IMSDocuments/dropdownIMS_ISOStdRef",
            dataType: "Json"
        });
        return response;
    }


    //-------------Dispaly Documents-------------------


    //get all Display Document
    this.IMS_DisplayDoc = function (Id) {
        var response = $http({
            method: 'get',
            params: { DocFileID: Id },
            url: '../IMSDocuments/IMS_DisplayDoc',
            dataType: "json"
        });
        return response;
    }


    this.IMS_SearchDisplayDoc = function (SearchParameter) {

        var response = $http({
            method: "get",
            params: { SearchParameter: SearchParameter },
            url: "../IMSDocuments/IMS_SearchDisplayDoc",
            dataType: "json"

        });
        return response;
    }

    //--------------------Archive Document-------------------

    //get alll Display Doc
    this.IMS_ArchiveDoc = function (Id) {
        var response = $http({
            method: 'get',
            params: { DocFileID: Id },
            url: '../IMSDocuments/IMS_ArchiveDoc',
            dataType: "json"
        });
        return response;
    }

    ////search For Archive Doc
    this.IMS_SearchArchiveDoc = function (SearchParameter) {

        var response = $http({
            method: "get",
            params: { SearchParameter: SearchParameter },
            url: "../IMSDocuments/IMS_SearchArchiveDoc",
            dataType: "json"

        });
        return response;
    }
    // List of archive record
    this.IMS_GetAllArchiveDoc = function (Id) {
        var response = $http({
            method: 'get',
            params: { IMSDocID: Id },
            url: '../IMSDocuments/IMS_GetAllArchiveDoc',
            dataType: "json"
        });
        return response;
    }

    //search For Archive Doc
    this.IMS_SearchArchiveDoc = function (SearchParameter) {

        var response = $http({
            method: "get",
            params: { SearchParameter: SearchParameter },
            url: "../IMSDocuments/IMS_SearchArchiveDoc",
            dataType: "json"

        });
        return response;
    }

    this.DOC_NoOfIMSDocumentsChart = function () {
        var response = $http({
            method: 'get',
            url: '../IMSDocuments/DOC_NoOfIMSDocumentsChart',
            dataType: "json"
        });
        return response;
    }
    //*************Working of IMS Documents end**********************************

    //******************* Interst Group****************************

    this.SaveInterstGroupDetails = function (GroupModel) {
        var response = $http({
            method: 'post',
            url: '/Login/SaveInterstGroupDetails',
            data: { GroupModel: GroupModel },
            dataType: "json"
        });

        return response;
    }

    this.searchInterstGroup = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "/Login/SearchInterstGroup",
            dataType: "json"
        });
        return response;
    }

    this.getAllInterstGroup = function (InterstGroupID) {

        var response = $http({
            method: "get",
            params: { InterstedGroupID: InterstGroupID },
            url: "/Login/getAllInterstGroup",
            dataType: "json"
        });
        return response;
    }

    this.getAllInterstedGroupsDropdown = function () {

        var response = $http({
            method: "get",
            //params: { InterstedGroupID: InterstGroupID },
            url: "/Login/getAllInterstedGroupsDropdown",
            dataType: "json"
        });
        return response;
    }

    this.DeleteInterstGroup = function (Id) {

        var response = $http({
            method: "get",
            params: { InterstedGroupID: Id },
            url: "/Login/DeleteInterstGroup",
            dataType: "json"
        });
        return response;
    }

    //******************* Interest Group********************************

    //*************Working of Inspection Reports start**********************************

    this.SaveInspectionReports = function (SaveInspectionReports) {
        var response = $http({
            method: 'post',
            url: '../InspectionReport/SaveInspectionReports',
            data: { InspectionModel: SaveInspectionReports },
            dataType: "json"
        });

        return response;
    }

    this.IR_getUserDropdown = function () {
        var response = $http({
            method: 'post',
            url: "../InspectionReport/IR_getUserDropdown",
            dataType: "Json"
        });
        return response;
    }

    this.dropDownPartNoName = function () {
        var response = $http({

            method: "get",
            url: "../InspectionReport/dropDownPartNoName",
            dataType: "json"
        })
        return response;
    }

    this.IR_dropDownCustomer = function () {
        var response = $http({

            method: "get",
            url: "../InspectionReport/IR_dropDownCustomer",
            dataType: "json"
        })
        return response;
    }


    this.dropDownReportType = function () {
        var response = $http({

            method: "get",
            url: "../InspectionReport/dropDownReportType",
            dataType: "json"
        })
        return response;
    }



    this.getAllInspectionReport = function (Id) {

        var response = $http({
            method: "get",
            params: { IRID: Id },
            url: "../InspectionReport/getAllInspectionReport",
            dataType: "json"
        });
        return response;
    }

    this.searchInspeactionReports = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../InspectionReport/searchInspeactionReports",
            dataType: "json"
        });
        return response;
    }

    this.IR_InspectionStatusReportChart = function () {
        var response = $http({
            method: "get",
            url: "../InspectionReport/IR_InspectionStatusReportChart",
            dataType: "json"
        })
        return response;
    }

    this.IR_TotalDocumentSubmittedChart = function () {
        var response = $http({
            method: "get",
            url: "../InspectionReport/IR_TotalDocumentSubmittedChart",
            dataType: "json"
        })
        return response;
    }

    this.IR_CustomerWiseDocumentChart = function () {
        var response = $http({
            method: "get",
            url: "../InspectionReport/IR_CustomerWiseDocumentChart",
            dataType: "json"
        })
        return response;
    }
    //*************Working of Inspection Reports End**********************************

    //*************Working of part start**********************************

    this.SaveParts = function (SaveParts) {
        var response = $http({
            method: 'post',
            url: '../PartReport/SaveParts',
            data: { PartModel: SaveParts },
            dataType: "json"
        });

        return response;
    }

    this.IR_getSourceNameDropdown = function () {
        var response = $http({
            method: "get",
            url: "../PartReport/IR_getSourceNameDropdown",
            dataType: "json"
        });
        return response;
    }

    this.getAllParts = function (Id) {

        var response = $http({
            method: "get",
            params: { PartID: Id },
            url: "../PartReport/getAllParts",
            dataType: "json"
        });
        return response;
    }

    this.searchParts = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../PartReport/searchParts",
            dataType: "json"
        });
        return response;
    }

    //*************Working of part End**********************************

    //*************Working of Report start**********************************

    this.SaveReport = function (SaveReport) {
        var response = $http({
            method: 'post',
            url: '../PartReport/SaveReport',
            data: { Report: SaveReport },
            dataType: "json"
        });

        return response;
    }

    this.Report_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../PartReport/Report_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.getAllReports = function (Id) {

        var response = $http({
            method: "get",
            params: { ReportID: Id },
            url: "../PartReport/getAllReports",
            dataType: "json"
        });
        return response;
    }

    this.searchReports = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../PartReport/searchReports",
            dataType: "json"
        });
        return response;
    }
    //*************Working of Report End**********************************


    //*************Working of Nonconformity start**********************************

    this.SaveNonConformity = function (SaveNonConformity) {
        var response = $http({
            method: 'post',
            url: '../Nonconformity/SaveNonConformity',
            data: { NCNModel: SaveNonConformity },
            dataType: "json"
        });

        return response;
    }

    this.NCN_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../Nonconformity/NCN_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.NCN_getUserDropdown = function () {
        var response = $http({
            method: 'post',
            url: "../Nonconformity/NCN_getUserDropdown",
            dataType: "Json"
        });
        return response;
    }
    this.NCN_getSourceNameDropdown = function () {
        var response = $http({

            method: "get",
            url: "../Nonconformity/NCN_getSourceNameDropdown",
            dataType: "json"
        });
        return response;
    }

    this.NCN_getAllNonconformity = function (Id) {

        var response = $http({
            method: "get",
            params: { NCNID: Id },
            url: "../Nonconformity/NCN_getAllNonconformity",
            dataType: "json"
        });
        return response;
    }

    this.NCN_searchNonconformity = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../Nonconformity/NCN_searchNonconformity",
            dataType: "json"
        });
        return response;
    }

    this.getServiceProviderByTypeId = function (TypeID) {

        var response = $http({
            method: "get",
            params: { TypeID: TypeID },
            url: "../Nonconformity/getServiceProviderByTypeId",
            dataType: "json"
        });
        return response;
    }


    this.NCNdropDownPartNoName = function () {
        var response = $http({

            method: "get",
            url: "../Nonconformity/NCNdropDownPartNoName",
            dataType: "json"
        })
        return response;
    }

    //code for charts

    this.NCN_MeasureNonconformityStatus = function () {
        var response = $http({
            method: "get",
            url: "../Nonconformity/NCN_MeasureNonconformityStatus",
            dataType: "json"
        })
        return response;
    }

    this.NCN_MeasureNonconformityTypeChart = function () {
        var response = $http({
            method: "get",
            url: "../Nonconformity/NCN_MeasureNonconformityTypeChart",
            dataType: "json"
        })
        return response;
    }

    this.NCN_MonthwiseNonconformityReports = function () {
        var response = $http({
            method: "get",
            url: "../Nonconformity/NCN_MonthwiseNonconformityReports",
            dataType: "json"
        })
        return response;
    }

    this.NCN_MonthwiseNonconformitycustomerwiseReport = function () {
        var response = $http({
            method: "get",
            url: "../Nonconformity/NCN_MonthwiseNonconformitycustomerwiseReport",
            dataType: "json"
        })
        return response;
    }

    this.NCN_MonthwiseNonconformitysupplierwiseReport = function () {
        var response = $http({
            method: "get",
            url: "../Nonconformity/NCN_MonthwiseNonconformitysupplierwiseReport",
            dataType: "json"
        })
        return response;
    }

    this.NCN_MonthwiseInternalRejectionReports = function () {
        var response = $http({
            method: "get",
            url: "../Nonconformity/NCN_MonthwiseInternalRejectionReports",
            dataType: "json"
        })
        return response;
    }


    this.NCN_getIncrementNumber = function () {
        var response = $http({

            method: "get",
            url: "../Nonconformity/NCN_getIncrementNumber",
            dataType: "json"
        })
        return response;
    }


    //*************Working of Nonconformity End**********************************

    //*************Working of SupplierAction**********************************
    this.SaveSupplierAction = function (SaveSupplierAction) {
        var response = $http({
            method: 'post',
            url: '../Nonconformity/SaveSupplierAction',
            data: { SupplierModel: SaveSupplierAction },
            dataType: "json"
        });
        return response;
    }

    this.getAllSupplierAction = function (Id) {

        var response = $http({
            method: "get",
            params: { NCNID: Id },
            url: "../Nonconformity/getAllSupplierAction",
            dataType: "json"
        });
        return response;
    }
    //*************Working of SupplierAction End**********************************

    //*************Working of ActionApproval**********************************

    this.SaveActionApproval = function (ActionModel) {
        var response = $http({
            method: 'post',
            url: '../Nonconformity/SaveActionApproval',
            data: { ActionModel: ActionModel },
            dataType: "json"
        });
        return response;
    }

    this.getAllActionApproval = function (Id) {

        var response = $http({
            method: "get",
            params: { NCNID: Id },
            url: "../Nonconformity/getAllActionApproval",
            dataType: "json"
        });
        return response;
    }
    //*************Working of ActionApproval End**********************************

    //******************Started Area of Legal Documents*************************************

    // Save Evaluation Compliance
    this.SaveEvaluationCompliance = function (SaveEvaluationCompliance) {
        var response = $http({
            method: 'post',
            url: '../LegalDocument/SaveEvaluationCompliance',
            data: { ECM: SaveEvaluationCompliance },
            dataType: "json"
        });

        return response;
    }
    //dropDown
    this.LD_getDropDownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../LegalDocument/LD_getDropDownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    // get All Evaluation Compliance
    this.getAllEvaluationCompliance = function (Id) {

        var response = $http({
            method: "get",
            params: { EvaluationID: Id },
            url: "../LegalDocument/getAllEvaluationCompliance",
            dataType: "json"
        });
        return response;
    }

    //search Evaluation Compliance
    this.searchEvaluationCompliance = function (SearchParameter) {

        var response = $http({
            method: "get",
            params: { SearchParameter: SearchParameter },
            url: "../LegalDocument/searchEvaluationCompliance",
            dataType: "json"

        });
        return response;
    }

    this.LD_NoApplicableLegalRequirements = function () {
        var response = $http({
            method: "get",
            url: "../LegalDocument/LD_NoApplicableLegalRequirements",
            dataType: "json"
        })
        return response;
    }


    this.LD_MonthlyLegalSubmissionDue = function () {
        var response = $http({
            method: "get",
            url: "../LegalDocument/LD_MonthlyLegalSubmissionDue",
            dataType: "json"
        })
        return response;
    }
    //**************Code For File Group************************************

    // Save File Group
    this.SaveFileGroup = function (SaveFileGroup) {
        var response = $http({
            method: 'post',
            url: '../FileGroup/SaveFileGroup',
            data: { FGM: SaveFileGroup },
            dataType: "json"
        });

        return response;
    }


    // get All Evaluation Compliance
    this.getAllFileGroup = function (Id) {

        var response = $http({
            method: "get",
            params: { FileGroupID: Id },
            url: "../FileGroup/getAllFileGroup",
            dataType: "json"
        });
        return response;
    }

    //search by title
    this.SearchFileGroup = function (searchparameter) {

        var response = $http({
            method: "get",
            params: { searchparameter: searchparameter },
            url: "../FileGroup/SearchFileGroup",
            dataType: "json"

        });
        return response;
    }

    //*********************Legal Requirements*************************************

    // code For file Group Reference
    this.dropdownFileGroupRef = function (Id) {
        var response = $http({
            method: 'post',
            params: { FilegroupTypeID: Id },
            url: '../LegalDocument/dropdownFileGroupRef',
            dataType: "json"
        });
        return response;
    }

    // get All Legal Requirement
    this.getAllLegalRequirement = function (Id) {

        var response = $http({
            method: "get",
            params: { RequiredID: Id },
            url: "../LegalDocument/getAllLegalRequirement",
            dataType: "json"
        });
        return response;
    }

    //search on document number and revision
    this.searchLegalRequirement = function (SearchParameter) {

        var response = $http({
            method: "get",
            params: { SearchParameter: SearchParameter },
            url: "../LegalDocument/searchLegalRequirement",
            dataType: "json"

        });
        return response;
    }

    //*********************** Legal Documents*********************************************

    //// code For file Group Reference
    //    this.dropdownFileGroupRef = function () {
    //        var response = $http({
    //            method: 'post',
    //            url: '../LegalDocument/dropdownFileGroupRef',
    //            dataType: "json"
    //        });
    //        return response;
    //    }


    // get All Legal Documents
    this.getAllLegalDocument = function (Id) {

        var response = $http({
            method: "get",
            params: { LegalDocID: Id },
            url: "../LegalDocument/getAllLegalDocument",
            dataType: "json"
        });
        return response;
    }

    //search on document number and revision
    this.searchLegalDocument = function (SearchParameter) {

        var response = $http({
            method: "get",
            params: { SearchParameter: SearchParameter },
            url: "../LegalDocument/searchLegalDocument",
            dataType: "json"

        });
        return response;
    }


    //*********************** Legal Reports*********************************************

    // get All Legal Requirement
    this.getAllLegalReport = function (Id) {

        var response = $http({
            method: "get",
            params: { ReportID: Id },
            url: "../LegalDocument/getAllLegalReport",
            dataType: "json"
        });
        return response;
    }

    //search on document number and revision
    this.SearchLegalReport = function (SearchParameter) {

        var response = $http({
            method: "get",
            params: { SearchParameter: SearchParameter },
            url: "../LegalDocument/SearchLegalReport",
            dataType: "json"

        });
        return response;
    }

    //******************Area of Legal Documents End*************************************

    //********************Area of Quality Assurance Plan Start*************************************

    //-------------QAP Editorial Page Start--------------------------------------------------

    // Save Parts Details
    //this.SaveQAP_Editorial = function (SaveQAP_Editorial) {
    //    var response = $http({
    //        method: 'post',
    //        url: '../QAP/SaveEditorialDetails',
    //        data: { EM: SaveQAP_Editorial },
    //        dataType: "json"
    //    });

    //    return response;
    //}


    //dropDown
    this.QAP_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../QAP/QAP_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    //user dropdown
    this.getUsersForDropdown = function () {
        var response = $http({
            method: 'post',
            url: '../QAP/getUsersForDropdown',
            dataType: "json"
        });

        return response;
    }

    // code For Parts Reference
    this.dropdownPartsRef = function () {
        var response = $http({
            method: 'post',
            url: '../QAP/dropdownPartsRef',
            dataType: "json"
        });
        return response;
    }

    // code For service Provider Reference
    this.dropdownServiceProviderRef = function () {
        var response = $http({
            method: 'post',
            url: '../QAP/dropdownServiceProviderRef',
            dataType: "json"
        });
        return response;
    }



    //list of QAP Editorial
    this.QAP_getAllEditorial = function (Id) {
        var response = $http({
            method: 'get',
            params: { QAPID: Id },
            url: '../QAP/QAP_getAllEditorial',
            dataType: "json"
        });
        return response;
    }


    //search 
    this.QAP_SearchEditorial = function (SearchParameter) {

        var response = $http({
            method: "get",
            params: { SearchParameter: SearchParameter },
            url: "../QAP/QAP_SearchEditorial",
            dataType: "json"

        });
        return response;
    }
    //-------------QAP Editorial Page End--------------------------------------------------


    //-------------QAP Published Page Start-----------------------------------------------

    //get all published Document

    this.QAP_getPublished = function (Id) {
        var response = $http({
            method: 'get',
            params: { QAPID: Id },
            url: '../QAP/QAP_getPublished',
            dataType: "json"
        });
        return response;
    }

    //Search on QAP number And Revision number
    this.QAP_SearchPublished = function (SearchParameter) {

        var response = $http({
            method: "get",
            params: { SearchParameter: SearchParameter },
            url: "../QAP/QAP_SearchPublished",
            dataType: "json"

        });
        return response;
    }

    this.QAP_getOverallPerformanceChart = function () {
        var response = $http({
            method: "get",
            url: "../QAP/QAP_getOverallPerformanceChart",
            dataType: "json"
        })
        return response;
    }

    this.QAP_MonthwiseApprovedReport = function () {
        var response = $http({
            method: "get",
            url: "../QAP/QAP_MonthwiseApprovedReport",
            dataType: "json"
        })
        return response;
    }


    //-------------QAP Published Page Start-----------------------------------------------
    //********************Area of Quality Assurance Plan End*************************************

    //*************Working of Work permit start**********************************

    this.WP_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../WorkPermit/WP_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.WP_getIAUsersDropdrown = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../WorkPermit/WP_getIAUsersDropdrown",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }


    this.getAllPermissionToWork = function (Id) {

        var response = $http({
            method: "get",
            params: { PermissionID: Id },
            url: "../WorkPermit/getAllPermissionToWork",
            dataType: "json"
        });
        return response;
    }

    this.searchPermissionToWork = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../WorkPermit/searchPermissionToWork",
            dataType: "json"
        });
        return response;
    }

    this.WP_getGroupDropdown = function (promptgroup) {

        var response = $http({
            method: "get",
            params: { promptgroup: promptgroup },
            url: "../WorkPermit/WP_getGroupDropdown",
            dataType: "json"
        });
        return response;
    }

    this.WP_PerformanceChart = function () {
        var response = $http({
            method: "get",
            url: "../WorkPermit/WP_PerformanceChart",
            dataType: "json"
        })
        return response;
    }

    this.WP_MonthlyWorkPermitIssueChart = function () {
        var response = $http({
            method: "get",
            url: "../WorkPermit/WP_MonthlyWorkPermitIssueChart",
            dataType: "json"
        })
        return response;
    }



    //*************Working of Work permit End**********************************

    //*************Working of Machine Maintanence**********************************

    this.getPMStatusChart = function () {
        var response = $http({
            method: "get",
            url: "../Setup/getPMStatusChart",
            dataType: "json"
        })
        return response;
    }

    this.getMonthlyPMPlans = function () {
        var response = $http({
            method: "get",
            url: "../Setup/getMonthlyPMPlans",
            dataType: "json"
        })
        return response;
    }

    this.getMonthwiseBreakDownChart = function () {
        var response = $http({
            method: "get",
            url: "../Setup/getMonthwiseBreakDownChart",
            dataType: "json"
        })
        return response;
    }
    this.getCurrentMonthBreakDownCharts = function () {
        var response = $http({
            method: "get",
            url: "../Setup/getCurrentMonthBreakDownCharts",
            dataType: "json"
        })
        return response;
    }

    this.getMMYears = function () {
        var response = $http({
            method: "get",
            url: "../MachineMaintainanceOne/getYears",
            dataType: "json"
        })
        return response;
    }
    this.getMonthlyBreakdownReport = function (mth, yr, FileName) {
        var response = $http({
            method: "get",
            url: "../MachineMaintainanceOne/getMonthlyBreakdownReport",
            params: { month: mth, year: yr, fileName: FileName },
            dataType: "json"
        })
        return response;
    }
    this.getMonthlyPreventiveMaintenanceReport = function (mth, yr, FileName) {
        var response = $http({
            method: "get",
            url: "../MachineMaintainanceOne/getMonthlyPreventiveMaintenanceReport",
            params: { month: mth, year: yr, fileName: FileName },
            dataType: "json"
        })
        return response;
    }
    this.Audit_getAllMachineDetailsForDropDown = function () {
        var response = $http({
            method: "get",
            url: "../MachineMaintainanceOne/Audit_getAllMachineDetailsForDropDown",
            //params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.SaveMachinePlan = function (machinePlan) {
        var response = $http({
            method: 'post',
            url: '../MachineMaintainanceOne/SaveMachinePlan',
            data: { plan: machinePlan },
            dataType: "json"
        });

        return response;
    }

    this.showMachinePlanDetails = function (selectedServiceProviders, AuditPlanID, operation) {
        var response = $http({
            method: 'post',
            url: '../MachineMaintainanceOne/showMachinePlanDetails',
            data: { selectedServiceProviders: selectedServiceProviders, AuditPlanID: AuditPlanID, NextPre: operation },
            dataType: "json"
        });

        return response;
    }


    this.showMachineMonthlyPlanDetails = function (selectedServiceProviders, AuditPlanID, operation) {
        var response = $http({
            method: 'post',
            url: '../MachineMaintainanceOne/showMachineMonthlyPlanDetails',
            data: { selectedServiceProviders: selectedServiceProviders, AuditPlanID: AuditPlanID, NextPre: operation },
            dataType: "json"
        });

        return response;
    }

    this.MW_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../MachineMaintainanceOne/MW_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.dropDownServicedBy = function () {
        var response = $http({
            method: 'post',
            url: "../MachineMaintainanceOne/dropDownServicedBy",
            dataType: "Json"

        });
        return response;
    }
    this.getWorkOrderSTSClose = function (WorkID) {

        var response = $http({
            method: "POST",
            params: { WorkID: WorkID },
            url: "../MachineMaintainanceOne/getWorkOrderSTSClose",
            dataType: "json"
        });
        return response;
    }

    this.MM_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../MachineMaintainanceOne/MM_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.MM_getMachinetypes = function () {
        var response = $http({
            method: 'post',
            url: "../MachineMaintainanceOne/MM_getMachinetypes",
            dataType: "Json"

        });
        return response;
    }

    //this.dropdownMMLocation = function () {
    //    var response = $http({
    //        method: 'post',
    //        url: "../MachineMaintainanceOne/dropdownWKLocation",
    //        dataType: "Json"

    //    });
    //    return response;
    //}

    this.getMMUsersDropdrown = function () {
        var response = $http({
            method: 'post',
            url: "../MachineMaintainanceOne/getMMUsersDropdrown",
            dataType: "Json"

        });
        return response;
    }

    this.MM_getAlMachineMaster = function (Id) {

        var response = $http({
            method: "get",
            params: { MachineID: Id },
            url: "../MachineMaintainanceOne/MM_getAlMachineMaster",
            dataType: "json"
        });
        return response;
    }

    this.MM_searchMachineMaster = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../MachineMaintainanceOne/MM_searchMachineMaster",
            dataType: "json"
        });
        return response;
    }

    this.MM_getAllMachineType = function (Id) {

        var response = $http({
            method: "get",
            params: { MachineTypeID: Id },
            url: "../Setup/MM_getAllMachineType",
            dataType: "json"
        });
        return response;
    }

    this.MM_searchMachineType = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../Setup/MM_searchMachineType",
            dataType: "json"
        });
        return response;
    }


    this.MM_getAllParameters = function (Id) {

        var response = $http({
            method: "get",
            params: { ParameterID: Id },
            url: "../Setup/MM_getAllParameters",
            dataType: "json"
        });
        return response;
    }

    this.MM_searchParameters = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../Setup/MM_searchParameters",
            dataType: "json"
        });
        return response;
    }


    this.MM_parameters = function () {
        var response = $http({
            method: 'post',
            url: "../Setup/MM_parameters",
            dataType: "Json"

        });
        return response;
    }

    this.MM_getAllChecklist = function (Id) {

        var response = $http({
            method: "get",
            params: { CheckListID: Id },
            url: "../Setup/MM_getAllChecklist",
            dataType: "json"
        });
        return response;
    }

    this.MM_searchCheckList = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../Setup/MM_searchCheckList",
            dataType: "json"
        });
        return response;
    }


    this.dropdownWKLocation = function () {
        var response = $http({
            method: 'post',
            url: "../MachineMaintainanceOne/dropdownWKLocation",
            dataType: "Json"

        });
        return response;
    }

    this.MM_getAllWorkOrder = function (Id) {

        var response = $http({
            method: "get",
            params: { WorkID: Id },
            url: "../MachineMaintainanceOne/MM_getAllWorkOrder",
            dataType: "json"
        });
        return response;
    }


    this.MM_searchWorkorder = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../MachineMaintainanceOne/MM_searchWorkorder",
            dataType: "json"
        });
        return response;
    }


    this.dropDownServicedBy = function () {
        var response = $http({
            method: 'post',
            url: "../MachineMaintainanceOne/dropDownServicedBy",
            dataType: "Json"

        });
        return response;
    }

    this.MM_getAllBreakDown = function (Id) {

        var response = $http({
            method: "get",
            params: { WorkID: Id },
            url: "../MachineMaintainanceOne/MM_getAllBreakDown",
            dataType: "json"
        });
        return response;
    }


    this.MM_dropdownSpareParts = function () {
        var response = $http({
            method: 'post',
            url: "../MachineMaintainanceOne/MM_dropdownSpareParts",
            dataType: "Json"

        });
        return response;
    }


    this.MM_getMacineNoDropdown = function () {
        var response = $http({
            method: 'post',
            url: "../MachineMaintainanceOne/MM_getMacineNoDropdown",
            dataType: "Json"

        });
        return response;
    }


    this.dropDownSupplier = function () {
        var response = $http({
            method: 'post',
            url: "../Setup/dropDownSupplier",
            dataType: "Json"

        });
        return response;
    }

    this.MM_getAllSpareParts = function (Id) {

        var response = $http({
            method: "get",
            params: { SparePartID: Id },
            url: "../Setup/MM_getAllSpareParts",
            dataType: "json"
        });
        return response;
    }


    this.MM_searchSpareParts = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../Setup/MM_searchSpareParts",
            dataType: "json"
        });
        return response;
    }


    this.SavePreventiveMaintainance = function (SavePreventiveMaintainance, ParametersMaintainance) {
        var response = $http({
            method: 'post',
            url: '../PreventiveMaintainance/SavePreventiveMaintainance',
            data: { PreventiveMaintainance: SavePreventiveMaintainance, ParametersMaintainance: ParametersMaintainance },
            dataType: "json"
        });
        return response;
    }

    this.PM_getMachinetypes = function () {
        var response = $http({
            method: 'post',
            url: "../PreventiveMaintainance/PM_getMachinetypes",
            dataType: "Json"

        });
        return response;
    }

    this.PM_getUsersDropdrown = function () {
        var response = $http({
            method: 'post',
            url: "../PreventiveMaintainance/PM_getUsersDropdrown",
            dataType: "Json"

        });
        return response;
    }


    this.PM_dropdownSpareParts = function () {
        var response = $http({
            method: 'post',
            url: "../PreventiveMaintainance/PM_dropdownSpareParts",
            dataType: "Json"

        });
        return response;
    }

    this.MM_getParametersByMachineTypeID = function (MachineTypeID, PMID) {

        var response = $http({
            method: "get",
            params: { MachineTypeID: MachineTypeID, PMID: PMID },
            url: "../PreventiveMaintainance/MM_getParametersByMachineTypeID",
            dataType: "json"
        });
        return response;
    }

    this.MM_getAllPreventiveMaintainance = function (Id) {

        var response = $http({
            method: "get",
            params: { PMID: Id },
            url: "../PreventiveMaintainance/MM_getAllPreventiveMaintainance",
            dataType: "json"
        });
        return response;
    }

    this.MM_searchPreventiveMaintainance = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../PreventiveMaintainance/MM_searchPreventiveMaintainance",
            dataType: "json"
        });
        return response;
    }


    this.PM_getMacineNoDropdown = function () {
        var response = $http({
            method: 'post',
            url: "../PreventiveMaintainance/PM_getMacineNoDropdown",
            dataType: "Json"

        });
        return response;
    }

    this.getTypeLocationFromSrNo = function (MachineID) {

        var response = $http({
            method: "get",
            params: { MachineID: MachineID },
            url: "../MachineMaintainanceOne/getTypeLocationFromSrNo",
            dataType: "json"
        });
        return response;
    }



    //**************Working Of Visitor's Management Start*****************************

    // Save Appointment Details
   

    this.VM_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: '../Appointment/VM_getDropdownValues',
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }



    this.VM_getUsersDropdrown = function (promptgroup) {
        var response = $http({
            method: "get",
            url: '../Appointment/VM_getUsersDropdrown',
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

   


    //**************Visitor's Management Report****************************

    this.GenerateAppointmentReport = function (filename, fromDate, toDate, PersonToVisitID, PurposeToVisitID, DepartmentID) {
        var response = $http({
            method: 'post',
            url: '../VM_Report/GenerateAppointmentReport',
            data: { filename: filename, fromDate: fromDate, toDate: toDate, PersonToVisitID: PersonToVisitID, PurposeToVisitID: PurposeToVisitID, DepartmentID: DepartmentID },
            dataType: "json"
        });

        return response;
    }

    this.getDropdownValuesVM = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../VM_Report/getDropdownValuesVM",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }


    //dropdown For user
    this.getUsersDropdrownVM = function () {
        var response = $http({
            method: 'post',
            url: '../VM_Report/getUsersDropdrownVM',
            dataType: "json"
        });

        return response;
    }




    //**************** working of Audit Area Start******************************

    //------------------Audit---------------------------------t


    //code For Audit Details
    this.Audit_GetAllAudit = function (Id) {

        var response = $http({
            method: 'get',
            params: { AuditID: Id },
            url: '../Audit/Audit_GetAllAudit',
            dataType: "json"
        });
        return response;
    }

    ////Search On Audit Number
    this.Audit_SearchAudit = function (SearchParameter) {
        var response = $http({
            method: 'get',
            params: { SearchParameter: SearchParameter },
            url: '../Audit/Audit_SearchAudit',
            dataType: "json"
        });
        return response;
    }

    //Get Radar chart
    this.Audit_getRadarCharts = function () {
        var response = $http({
            method: "get",
            url: "../Audit/Audit_getRadarCharts",
            dataType: "json"
        })
        return response;
    }



    //------------Audit Setup---------------------------
    this.SaveAuditDetails = function (SaveAuditDetails, AuditFindings) {
        var response = $http({
            method: 'post',
            url: '../Audit/SaveAuditDetails',
            data: { AuditM: SaveAuditDetails, lstAF: AuditFindings },
            dataType: "json"
        });
        return response;
    }

    //DropDwon Audit
    this.GetAuditDropdown = function (promptGroup) {
        var response = $http({
            method: 'post',
            params: { promptGroup: promptGroup },
            url: '../Audit/GetAuditDropdown',
            dataType: 'json'

        });
        return response;
    }

    this.Audit_GetUserDropdown = function () {
        var response = $http({
            method: 'post',
            url: "../Audit/Audit_GetUserDropdown",
            dataType: "Json"

        });
        return response;
    }

    //dropdown user 
    this.Audit_getAuditSetCriteriaDropdown = function () {
        var response = $http({
            method: 'post',
            url: "../Audit/Audit_getAuditSetCriteriaDropdown",
            dataType: "Json"

        });
        return response;
    }

    //code For Audit Details
    this.Audit_GetAllDetails = function (Id) {

        var response = $http({
            method: 'get',
            params: { AuditID: Id },
            url: '../Audit/Audit_GetAllDetails',
            dataType: "json"
        });
        return response;
    }

    //Search On Audit Number
    this.Audit_SearchAuditSetUp = function (SearchParameter, isConfirmed) {
        var response = $http({
            method: 'get',
            params: { SearchParameter: SearchParameter, isConfirmed: isConfirmed },
            url: '../Audit/Audit_SearchAuditSetUp',
            dataType: "json"
        });
        return response;
    }

    // Onchange DropDown
    this.Audit_OnchangeDropdwon = function (SectionID, AuditID) {

        var response = $http({
            method: 'get',
            params: { SectionID: SectionID, AuditID: AuditID },
            url: '../Audit/Audit_OnchangeDropdwon',
            dataType: "json"
        });
        return response;
    }

    this.dropdownAuditServiceProvider = function () {
        var response = $http({
            method: 'post',
            url: "../Audit/dropdownAuditServiceProvider",
            dataType: "Json"

        });
        return response;
    }

    this.getPrevDetails = function (Organization) {

        var response = $http({
            method: "get",
            params: { Organization: Organization },
            url: "../Audit/getPrevDetails",
            dataType: "json"
        });
        return response;
    }

    //----------- Audit Report----------------------------

    //this.GetAuditDropdown = function (promptgroup) {
    //    var response = $http({
    //        method: "get",
    //        url: "../Audit/GetAuditDropdown ",
    //        params: { promptgroup: promptgroup },
    //        dataType: "json"
    //    });
    //    return response;
    //}

    // Save Audit Report 
    this.SaveAuditReportDetail = function (SaveAuditReportDetail) {
        var response = $http({
            method: 'post',
            url: '../Audit/SaveAuditReportDetail',
            data: { AR: SaveAuditReportDetail },
            dataType: "json"
        });
        return response;
    }


    //code For Show Audit setup Details On Audit Report
    this.Audit_GetAllAuditDetails = function (Id, IsConfirmed) {

        var response = $http({
            method: 'get',
            params: { AuditID: Id, isConfirmed: IsConfirmed },
            url: '../Audit/Audit_GetAllAuditDetails',
            dataType: "json"
        });
        return response;
    }

    this.getPreviousAuditDateAndScore = function (OrganizationID, AuditType, AuditTypeID) {

        var response = $http({
            method: 'get',
            params: { OrganizationID: OrganizationID, AuditType: AuditType, AuditTypeID: AuditTypeID },
            url: '../Audit/getPreviousAuditDateAndScore',
            dataType: "json"
        });
        return response;
    }


    //Get List of audit report
    this.Audit_getAllAuditDetailsDashboard = function () {
        var response = $http({
            method: 'get',
            //params: { AuditID: Id },
            url: '../Audit/Audit_getAllAuditDetailsDashboard',
            dataType: "json"
        });
        return response;
    }

    this.Audit_getAllAuditChartDetails = function (AuditID) {
        var response = $http({
            method: 'get',
            params: { AuditID: AuditID },
            url: '../Audit/Audit_getAllAuditChartDetails',
            dataType: "json"
        });
        return response;
    }

    //Get List of audit report
    this.GetAllAuditReport = function (Id) {
        var response = $http({
            method: 'get',
            params: { AuditID: Id },
            url: '../Audit/GetAllAuditReport',
            dataType: "json"
        });
        return response;
    }

    //--------------Audit Varification--------------------------

    // Save Audit Report 
    this.SaveVarificationDetails = function (SaveVarificationDetails, verificationModel) {
        var response = $http({
            method: 'post',
            url: '../Audit/SaveVarificationDetails',
            data: { lstVR: SaveVarificationDetails, auditVerification: verificationModel },
            dataType: "json"
        });
        return response;
    }


    //code For Show Audit setup Details On varification
    this.Audit_GetAllAuditSetUpDetails = function (Id, IsConfirmed) {

        var response = $http({
            method: 'get',
            params: { AuditID: Id, isConfirmed: IsConfirmed },
            url: '../Audit/Audit_GetAllAuditSetUpDetails',
            dataType: "json"
        });
        return response;
    }

    //Get List of audit varification
    this.GetAllVarificationDetails = function (Id) {
        var response = $http({
            method: 'get',
            params: { AuditID: Id },
            url: '../Audit/GetAllVarificationDetails',
            dataType: "json"
        });
        return response;
    }

    //*****Code For Audit Questions

    this.getAllAuditQuestions = function (Id) {

        var response = $http({
            method: 'get',
            params: { QuestionID: Id },
            url: '../SetupAudit/getAllAuditQuestions',
            dataType: "json"
        });
        return response;
    }

    this.searchAuditQuestion = function (SearchParameter) {
        var response = $http({
            method: 'get',
            params: { SearchParameter: SearchParameter },
            url: '../SetupAudit/searchAuditQuestion',
            dataType: "json"
        });
        return response;
    }

    this.Audit_getAuditCriteriaDropdown = function (AuditGroup) {
        var response = $http({
            method: 'post',
            params: { auditGroup: AuditGroup },
            url: "../SetupAudit/Audit_getAuditCriteriaDropdown",
            dataType: "Json"
        });
        return response;
    }
    //*****Code For Audit Group

    this.AuditSetup_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../SetupAudit/AuditSetup_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.getAllAuditGroup = function (Id) {

        var response = $http({
            method: 'get',
            params: { AuditGroupID: Id },
            url: '../SetupAudit/getAllAuditGroup',
            dataType: "json"
        });
        return response;
    }

    this.searchAuditGroup = function (SearchParameter) {
        var response = $http({
            method: 'get',
            params: { SearchParameter: SearchParameter },
            url: '../SetupAudit/searchAuditGroup',
            dataType: "json"
        });
        return response;
    }




    //Code for charts

    this.Audit_getNoOfAuditsChart = function () {
        var response = $http({
            method: "get",
            url: "../Audit/Audit_getNoOfAuditsChart",
            dataType: "json"
        })
        return response;
    }

    this.Audit_MonthlyNoOfAudits = function () {
        var response = $http({
            method: "get",
            url: "../Audit/Audit_MonthlyNoOfAudits",
            dataType: "json"
        })
        return response;
    }





    //****************Working of Audit Area End******************************



    //****************Working of Audit Area End******************************

    this.getAllPromptDetails = function (promptGroup, promptID) {
        var response = $http({
            method: 'get',
            params: { promptGroup: promptGroup, PromptId: promptID },
            url: '/Prompt/getAllPromptDetails',
            dataType: "json"
        });
        return response;
    }

    this.SavePromptDetails = function (PromptDetails) {
        var response = $http({
            method: 'post',
            url: '/Prompt/SavePromptDetails',
            data: { promptDetails: PromptDetails },
            dataType: "json"
        });
        return response;
    }

    //****************Working of Audit Area End******************************

    //*****************Working Of PPC Start***********************************


    this.SaveExcelData = function (excelData) {
        var response = $http({
            method: "POST",
            url: "../PPC/SaveData",
            data: { items: JSON.stringify(excelData) },
            headers: {
                'Content-Type': 'application/json'
            }
        })
        return response;
    }

    this.ImportRawMaterialData = function (excelData) {
        var response = $http({
            method: "POST",
            url: "../PPC/ImportRawMaterialData",
            data: { items: JSON.stringify(excelData) },
            headers: {
                'Content-Type': 'application/json'
            }
        })
        return response;
    }

    this.ImportOTData = function (excelData) {
        var response = $http({
            method: "POST",
            url: "../PPC/ImportOTData",
            data: { items: JSON.stringify(excelData) },
            headers: {
                'Content-Type': 'application/json'
            }
        })
        return response;
    }

    this.ImportPDData = function (excelData) {
        var response = $http({
            method: "POST",
            url: "../PPC/ImportPDData",
            data: { items: JSON.stringify(excelData) },
            headers: {
                'Content-Type': 'application/json'
            }
        })
        return response;
    }
    this.ImportQAData = function (excelData) {
        var response = $http({
            method: "POST",
            url: "../PPC/ImportQAData",
            data: { items: JSON.stringify(excelData) },
            headers: {
                'Content-Type': 'application/json'
            }
        })
        return response;
    }

    this.getAllItemMasterDetails = function (User) {
        var response = $http({
            method: "get",
            url: "../PPC/getAllItemMasterDetails",
            //data: JSON.stringify(User),
            dataType: "json"
        });
        return response;
    }

    this.getAllFilteredItemMasterDetails = function (keyword, startdate, enddate) {
        var response = $http({
            method: "get",
            url: "../PPC/getAllFilteredItemMasterDetails",
            params: { keyword: keyword, startDate: startdate, endDate: enddate },
            dataType: "json"
        });
        return response;
    }

    this.getAllDispatchedItemMasterDetails = function (User) {
        var response = $http({
            method: "get",
            url: "../PPC/getAllDispatchedItemMasterDetails",
            //data: JSON.stringify(User),
            dataType: "json"
        });
        return response;
    }

    this.getAllFilteredDispatchedItemMasterDetails = function (keyword, startdate, enddate) {
        var response = $http({
            method: "get",
            url: "../PPC/getAllFilteredDispatchedItemMasterDetails",
            params: { keyword: keyword, startDate: startdate, endDate: enddate },
            dataType: "json"
        });
        return response;
    }

    this.getItemDetailsbyId = function (Id) {
        var response = $http({
            method: "get",
            url: "../PPC/getItemDetailsbyId",
            params: { id: Id },
            dataType: "json"
        });
        return response;
    }

    this.SaveItemMasterDetails = function (itemMasterDetails) {

        var itemDetails = {
            ItemId: itemMasterDetails.ItemId,
            PartyName: itemMasterDetails.PartyName,
            PartyPONum: itemMasterDetails.PartyPONum,
            PartyPODate: itemMasterDetails.PartyPODate,
            OADate: itemMasterDetails.OADate,
            OANum: itemMasterDetails.OANum,
            ScheduleDate: itemMasterDetails.ScheduleDate,
            DeliveryDate: itemMasterDetails.DeliveryDate,
            ItemName: itemMasterDetails.ItemName,
            ItemDescription: itemMasterDetails.ItemDescription,
            OrderQTY: itemMasterDetails.OrderQTY,
            DispatchQTY: itemMasterDetails.DispatchQTY,
            ShortCloseQTY: itemMasterDetails.ShortCloseQTY,
            InDispatchDate: itemMasterDetails.InDispatchDate,
            AcDispatchDate: itemMasterDetails.AcDispatchDate,
            RWDT: itemMasterDetails.RWDT,
            RWCMT: itemMasterDetails.RWCMT,
            RWGrade: itemMasterDetails.RWGrade,
            RWIsCompleted: itemMasterDetails.RWIsCompleted,
            OTDT: itemMasterDetails.OTDT,
            OTCMT: itemMasterDetails.OTCMT,
            OTIsCompleted: itemMasterDetails.OTIsCompleted,
            PDDT: itemMasterDetails.PDDT,
            PDCMT: itemMasterDetails.PDCMT,
            PDIsCompleted: itemMasterDetails.PDIsCompleted,
            QADT: itemMasterDetails.QADT,
            QACMT: itemMasterDetails.QACMT,
            QAIsCompleted: itemMasterDetails.QAIsCompleted,
            Comments: itemMasterDetails.Comments
        }

        var response = $http({
            method: 'post',
            url: '../PPC/SaveitemeDetails',
            data: JSON.stringify(itemDetails),
            dataType: "json"
        });
        return response;
    }

    this.SaveItemMasterComments = function (itemMasterDetails) {

        var itemDetails = {
            ItemId: itemMasterDetails.ItemId,
            PartyName: itemMasterDetails.PartyName,
            PartyPONum: itemMasterDetails.PartyPONum,
            PartyPODate: itemMasterDetails.PartyPODate,
            OANum: itemMasterDetails.OANum,
            ScheduleDate: itemMasterDetails.ScheduleDate,
            DeliveryDate: itemMasterDetails.DeliveryDate,
            ItemName: itemMasterDetails.ItemName,
            ItemDescription: itemMasterDetails.ItemDescription,
            OrderQTY: itemMasterDetails.OrderQTY,
            DispatchQTY: itemMasterDetails.DispatchQTY,
            ShortCloseQTY: itemMasterDetails.ShortCloseQTY,
            InDispatchDate: itemMasterDetails.InDispatchDate,
            AcDispatchDate: itemMasterDetails.AcDispatchDate,
            RWDT: itemMasterDetails.RWDT,
            RWCMT: itemMasterDetails.RWCMT,
            RWIsCompleted: itemMasterDetails.RWIsCompleted,
            OTDT: itemMasterDetails.OTDT,
            OTCMT: itemMasterDetails.OTCMT,
            OTIsCompleted: itemMasterDetails.OTIsCompleted,
            PDDT: itemMasterDetails.PDDT,
            PDCMT: itemMasterDetails.PDCMT,
            PDIsCompleted: itemMasterDetails.PDIsCompleted,
            QADT: itemMasterDetails.QADT,
            QACMT: itemMasterDetails.QACMT,
            QAIsCompleted: itemMasterDetails.QAIsCompleted,
            Comments: itemMasterDetails.Comments
        }

        var response = $http({
            method: 'post',
            url: '../PPC/SaveitemComments',
            data: JSON.stringify(itemDetails),
            dataType: "json"
        });
        return response;
    }

    this.CompleteOrderStatus = function (Id) {

        var response = $http({
            method: 'get',
            url: '../PPC/CompleteOrderStatus',
            params: { Id: Id },
            dataType: "json"
        });
        return response;
    }

    this.getUsers = function (User) {
        var response = $http({
            method: "get",
            url: "../PPC/getUsers",
            dataType: "json"
        });
        return response;
    }

    this.getDropdownValues = function (ddlType) {
        //alert(ddlType);
        var response = $http({
            method: "get",
            url: "../PPC/Get_DropDownValues",
            params: { ddlType: ddlType },
            dataType: "json"
        });
        return response;
    }

    this.getUserDetailsbyId = function (Id) {
        var response = $http({
            method: "get",
            url: "../Login/getuserDetailsbyId",
            params: { id: Id },
            dataType: "json"
        });
        return response;
    }

    this.getItemTrackerDetailsbyId = function (Id) {
        var response = $http({
            method: "get",
            url: "../PPC/getItemTrackerDetailsbyId",
            params: { id: Id },
            dataType: "json"
        });
        return response;
    }

    //this.SaveUserDetails = function (user) {

    //    var response = $http({
    //        method: 'post',
    //        url: '../Login/SaveUserDetails',
    //        data: JSON.stringify(user),
    //        dataType: "json"
    //    });
    //    return response;
    //}

    //Oil And Gas Starts

    this.getAllOilAndGasItemMasterDetails = function (isOilAndGas) {
        var response = $http({
            method: "get",
            params: { isOilAndGas: isOilAndGas },
            url: "../OilAndGas/getAllOilAndGasItemMasterDetails",
            dataType: "json"
        });
        return response;
    }

    this.SaveOilAndGasItemMasterDetails = function (itemMasterDetails) {

        var response = $http({
            method: 'post',
            url: '../OilAndGas/SaveOilAndGasDetails',
            data: JSON.stringify(itemMasterDetails),
            dataType: "json"
        });
        return response;
    }

    this.SaveNonOilAndGasItemMasterDetails = function (itemMasterDetails) {

        var response = $http({
            method: 'post',
            url: '../OilAndGas/SaveNonOilAndGasDetails',
            data: JSON.stringify(itemMasterDetails),
            dataType: "json"
        });
        return response;
    }

    this.getOilAndGasItemDetailsbyId = function (Id) {
        var response = $http({
            method: "get",
            url: "../OilAndGas/getOilAndGasItemDetailsbyId",
            params: { id: Id },
            dataType: "json"
        });
        return response;
    }

    this.UploadOilAndGasMaster = function (excelData) {
        var response = $http({
            method: "POST",
            url: "../OilAndGas/SaveData",
            data: { items: JSON.stringify(excelData) },
            headers: {
                'Content-Type': 'application/json'
            }
        })
        return response;
    }

    this.UploadNonOilAndGasMaster = function (excelData) {
        var response = $http({
            method: "POST",
            url: "../OilAndGas/SaveDataNonOilAndGas",
            data: { items: JSON.stringify(excelData) },
            headers: {
                'Content-Type': 'application/json'
            }
        })
        return response;
    }

    this.getAllFilteredItemMasterDetailsForOilAndGas = function (keyword, startdate, enddate, IsDispatched) {
        var response = $http({
            method: "get",
            url: "../OilAndGas/getAllFilteredItemMasterDetailsForOilAndGas",
            params: { keyword: keyword, startDate: startdate, endDate: enddate, IsDispatched: IsDispatched },
            dataType: "json"
        });
        return response;
    }

    this.getAllFilteredItemMasterDetailsForNonOilAndGas = function (keyword, startdate, enddate, IsDispatched) {
        var response = $http({
            method: "get",
            url: "../OilAndGas/getAllFilteredItemMasterDetailsForNonOilAndGas",
            params: { keyword: keyword, startDate: startdate, endDate: enddate, IsDispatched: IsDispatched },
            dataType: "json"
        });
        return response;
    }

    this.CompleteOrderStatusForOilAndGas = function (Id) {

        var response = $http({
            method: 'get',
            url: '../OilAndGas/CompleteOrderStatusForOilAndGas',
            params: { Id: Id },
            dataType: "json"
        });
        return response;
    }

    this.getOilAndGasItemTrackerDetailsbyId = function (Id) {
        var response = $http({
            method: "get",
            url: "../OilAndGas/getOilAndGasItemTrackerDetailsbyId",
            params: { id: Id },
            dataType: "json"
        });
        return response;
    }

    //Oil And Gas Ends

    //*****************Working Of PPC End***********************************

    //*****************Working Of SPC Start***********************************

    this.dropDownitem = function () {
        var response = $http({
            method: "get",
            url: "../SPCQAP/dropDownitem",
            dataType: "json"
        })
        return response;
    }

    this.getQAPItemsDropdownForSPC = function () {
        var response = $http({
            method: "get",
            url: "../SPC/getQAPItemsDropdownForSPC",
            dataType: "json"
        })
        return response;
    }

    this.getAssignedQAPItems = function (Id) {

        var response = $http({
            method: "get",
            params: { QAPID: Id },
            url: "../SPCQAP/getAssignedQAPItems",
            dataType: "json"
        });
        return response;
    }


    this.SPC_getAllWorkorder = function (Id) {

        var response = $http({
            method: "get",
            params: { WorkOrderID: Id },
            url: "../SPCQAP/SPC_getAllWorkorder",
            dataType: "json"
        });
        return response;
    }

    this.SPC_searchWorkorder = function (SearchParameter) {
        var response = $http({
            method: 'get',
            params: { SearchParameter: SearchParameter },
            url: '../SPCQAP/SPC_searchWorkorder',
            dataType: "json"
        });
        return response;
    }


    this.SPC_getDropdownValues = function (promptGroup) {
        var response = $http({
            method: 'post',
            params: { promptGroup: promptGroup },
            url: '../SPCQAP/SPC_getDropdownValues',
            dataType: 'json'

        });
        return response;
    }

    this.SPC_getAllQAP = function (Id) {

        var response = $http({
            method: "get",
            params: { QAPID: Id },
            url: "../SPCQAP/SPC_getAllQAP",
            dataType: "json"
        });
        return response;
    }

    this.SPC_searchQAP = function (SearchParameter) {
        var response = $http({
            method: 'get',
            params: { SearchParameter: SearchParameter },
            url: '../SPCQAP/SPC_searchQAP',
            dataType: "json"
        });
        return response;
    }


    this.SPC_getMachinetypes = function () {
        var response = $http({
            method: 'post',
            url: "../SPC/SPC_getMachinetypes",
            dataType: "Json"

        });
        return response;
    }
    //************************Code for SPC page*****************************

    this.getSPCUsersDropdrown = function () {
        var response = $http({
            method: 'post',
            url: "../SPC/getSPCUsersDropdrown",
            dataType: "Json"

        });
        return response;
    }

    this.SPC_getSPCDropdownValues = function (promptGroup) {
        var response = $http({
            method: 'post',
            params: { promptGroup: promptGroup },
            url: '../SPC/SPC_getSPCDropdownValues',
            dataType: 'json'

        });
        return response;
    }


    this.dropDownSpitem = function () {
        var response = $http({
            method: "get",
            url: "../SPC/dropDownSpitem",
            dataType: "json"
        })
        return response;
    }


    this.getWorkOrderForDropDown = function () {
        var response = $http({
            method: "get",
            url: "../SPC/getWorkOrderForDropDown",
            dataType: "json"
        })
        return response;
    }

    this.getMacineMasterDropdown = function () {
        var response = $http({
            method: "get",
            url: "../SPC/getMacineMasterDropdown",
            dataType: "json"
        })
        return response;
    }

    this.SPC_getMachinetypes = function () {
        var response = $http({
            method: "get",
            url: "../SPC/SPC_getMachinetypes",
            dataType: "json"
        })
        return response;
    }



    //*****************Working Of SPC End***********************************

    //************Working Of RM Quality*********************

    //Save Purchase
    this.SavePurchase = function (purchaseModel) {
        var response = $http({
            method: 'post',
            url: '../RM/SavePurchase',
            data: { purchase: purchaseModel },
            dataType: "json"
        });

        return response;
    }

    // save Procurement
    //this.SaveQuality1 = function (SaveQuality1) {
    //    var response = $http({
    //        method: 'post',
    //        url: '../RM/SaveQuality1',
    //        data: { procurement: SaveQuality1 },
    //        dataType: "json"
    //    });

    //    return response;
    //}


    this.getYears = function () {
        var response = $http({
            method: 'post',
            url: '../Audit/getAllYears',
            dataType: "json"
        });

        return response;
    }


    this.getServiceProviderDetails = function () {
        var response = $http({
            method: 'post',
            url: '../Audit/getServiceProviderDetails',
            dataType: "json"
        });

        return response;
    }

    this.showPlanDetails = function (year, selectedServiceProviders, AuditPlanID, operation) {
        var response = $http({
            method: 'post',
            url: '../Audit/showPlanDetails',
            data: { year: year, selectedServiceProviders: selectedServiceProviders, AuditPlanID: AuditPlanID, NextPre: operation },
            dataType: "json"
        });

        return response;
    }


    this.SaveAuditPlan = function (auditPlan) {
        var response = $http({
            method: 'post',
            url: '../Audit/SaveAuditPlan',
            data: { plan: auditPlan },
            dataType: "json"
        });

        return response;
    }

    this.Audit_getAllAuditPlanDetails = function () {
        var response = $http({
            method: 'post',
            url: '../Audit/Audit_getAllAuditPlanDetails',
            dataType: "json"
        });

        return response;
    }

    //*****************Working Of ProRA Start***********************************


    this.ProRA_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../ProRASetup/ProRA_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.getAllIntrestedParty = function (Id) {

        var response = $http({
            method: "get",
            params: { IPID: Id },
            url: "../ProRASetup/getAllIntrestedParty",
            dataType: "json"
        });
        return response;
    }

    this.searchIntrestedParty = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../ProRASetup/searchIntrestedParty",
            dataType: "json"
        });
        return response;
    }

    this.ProRa_getAllLegalRequirement = function (Id) {

        var response = $http({
            method: "get",
            params: { LegalRequirementID: Id },
            url: "../ProRASetup/ProRa_getAllLegalRequirement",
            dataType: "json"
        });
        return response;
    }

    this.searchLegalRequirement = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../ProRASetup/searchLegalRequirement",
            dataType: "json"
        });
        return response;
    }

    this.getAllProcess = function (Id) {

        var response = $http({
            method: "get",
            params: { ProcessID: Id },
            url: "../ProRASetup/getAllProcess",
            dataType: "json"
        });
        return response;
    }

    this.searchProcess = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../ProRASetup/searchProcess",
            dataType: "json"
        });
        return response;
    }

    //********Process Mapping Page********

    this.getProcessSubProcess = function () {
        var response = $http({

            method: "get",
            url: "../ProRAPM/getProcessSubProcess",
            dataType: "json"
        })
        return response;
    }

    this.PM_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../ProRAPM/PM_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.ProRA_getSupplierDropdown = function () {
        var response = $http({

            method: "get",
            url: "../ProRAPM/ProRA_getSupplierDropdown",
            dataType: "json"
        })
        return response;
    }

    this.Pro_getUserDropdown = function () {
        var response = $http({
            method: 'post',
            url: "../ProRAPM/Pro_getUserDropdown",
            dataType: "Json"
        });
        return response;
    }

    this.getAllProcessMapping = function (Id) {

        var response = $http({
            method: "get",
            params: { PMID: Id },
            url: "../ProRAPM/getAllProcessMapping",
            dataType: "json"
        });
        return response;
    }

    this.searchProcessMapping = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../ProRAPM/searchProcessMapping",
            dataType: "json"
        });
        return response;
    }

    this.getProcessFromMprocess = function (MainProcess) {

        var response = $http({
            method: "get",
            params: { MainProcess: MainProcess },
            url: "../ProRAPM/getProcessFromMprocess",
            dataType: "json"
        });
        return response;
    }
    //******************RiskTypeQuality Page*********************

    this.RiskType_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../ProRARiskType/RiskType_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.getIntrestedPartyDropdown = function (PMID) {
        var response = $http({

            method: "get",
            params: { PMID: PMID },
            url: "../ProRARiskType/getIntrestedPartyDropdown",
            dataType: "json"
        })
        return response;
    }

    this.getRiskProcessDropdown = function () {
        var response = $http({

            method: "get",
            url: "../ProRARiskType/getRiskProcessDropdown",
            dataType: "json"
        })
        return response;
    }

    this.Risk_getUserDropdown = function () {
        var response = $http({
            method: 'post',
            url: "../ProRARiskType/Risk_getUserDropdown",
            dataType: "Json"
        });
        return response;
    }

    this.getAllRiskTypeQuality = function (Id) {

        var response = $http({
            method: "get",
            params: { PMID: Id },
            url: "../ProRARiskType/getAllRiskTypeQuality",
            dataType: "json"
        });
        return response;
    }

    //******************RiskTypeEnvironment Page*********************

    this.getLegalRequirementDropdown = function () {
        var response = $http({

            method: "get",
            url: "../ProRARiskType/getLegalRequirementDropdown",
            dataType: "json"
        })
        return response;
    }

    this.getAllRiskTypeEnvironment = function (Id) {

        var response = $http({
            method: "get",
            params: { PMID: Id },
            url: "../ProRARiskType/getAllRiskTypeEnvironment",
            dataType: "json"
        });
        return response;
    }

    //******************RiskTypeHS Page*********************

    this.getAllRiskTypeHS = function (Id) {

        var response = $http({
            method: "get",
            params: { PMID: Id },
            url: "../ProRARiskType/getAllRiskTypeHS",
            dataType: "json"
        });
        return response;
    }

    this.searchRiskTypeAssessment = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../ProRARiskType/searchRiskTypeAssessment",
            dataType: "json"
        });
        return response;
    }


    //******************RiskTypeInjuries Page*************************

    this.getAllRiskTypeInjury = function (Id) {

        var response = $http({
            method: "get",
            params: { InjuryID: Id },
            url: "../ProRASetup/getAllRiskTypeInjury",
            dataType: "json"
        });
        return response;
    }

    this.searchRiskTypeInjury = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../ProRASetup/searchRiskTypeInjury",
            dataType: "json"
        });
        return response;
    }


    //************************Department Page**************

    this.getAllDepartment = function (Id) {

        var response = $http({
            method: "get",
            params: { DeptID: Id },
            url: "../ProRASetup/getAllDepartment",
            dataType: "json"
        });
        return response;
    }

    this.searchDepartment = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../ProRASetup/searchDepartment",
            dataType: "json"
        });
        return response;
    }

    //**********Code For RiskAssesment **********************

    this.SaveRiskAssesment = function (SaveRiskAssesment, RiskQualityModel) {
        var response = $http({
            method: 'post',
            url: '../ProRARiskType/SaveRiskAssesment',
            data: { RiskModel: SaveRiskAssesment, lstQA: RiskQualityModel },
            dataType: "json"
        });
        return response;
    }

    this.fetchFields = function () {

        var response = $http({
            method: "get",
            // params: { SubProcess: SubProcess },
            url: "../ProRARiskType/fetchFields",
            dataType: "json"
        });
        return response;
    }

    this.getDetailsByRAIDAndIntrestId = function (RiskQualityID, RAID) {
        var response = $http({
            method: "get",
            url: "../ProRARiskType/getDetailsByRAIDAndIntrestId",
            params: { RiskQualityID: RiskQualityID, RAID: RAID },
            dataType: "json"
        });
        return response;
    }

    this.getEnvironmentDetails = function (RiskEnvironmentID, RAID) {
        var response = $http({
            method: "get",
            url: "../ProRARiskType/getEnvironmentDetails",
            params: { RiskEnvironmentID: RiskEnvironmentID, RAID: RAID },
            dataType: "json"
        });
        return response;
    }

    this.getHealthSafetyDetails = function (HSID, RAID) {
        var response = $http({
            method: "get",
            url: "../ProRARiskType/getHealthSafetyDetails",
            params: { HSID: HSID, RAID: RAID },
            dataType: "json"
        });
        return response;
    }

    //**********Code For RiskTypeEnvironment **********************

    this.SaveRiskTypeEnvironment = function (SaveRiskTypeEnvironment, RiskEnvironmentModel) {
        var response = $http({
            method: 'post',
            url: '../ProRARiskType/SaveRiskTypeEnvironment',
            data: { RiskModel: SaveRiskTypeEnvironment, lstEn: RiskEnvironmentModel },
            dataType: "json"
        });
        return response;
    }


    //**********Code For RiskTypeHs **********************

    this.SaveRiskTypeHS = function (SaveRiskTypeHS, RiskTypeHSModel) {
        var response = $http({
            method: 'post',
            url: '../ProRARiskType/SaveRiskTypeHS',
            data: { RiskModel: SaveRiskTypeHS, lstHs: RiskTypeHSModel },
            dataType: "json"
        });
        return response;
    }

    this.getAllRiskAssesment = function (PMID, RAID) {

        var response = $http({
            method: "get",
            params: { PMID: PMID, RAID: RAID },
            url: "../ProRARiskType/getAllRiskAssesment",
            dataType: "json"
        });
        return response;
    }




    //****************Code For Management Program**********************

    this.Mg_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../ProRAManagement/Mg_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.Mg_getUserDropdown = function () {
        var response = $http({
            method: 'post',
            url: "../ProRAManagement/Mg_getUserDropdown",
            dataType: "Json"
        });
        return response;
    }


    this.SaveManagement = function (MPModel) {
        var response = $http({
            method: 'post',
            url: '../ProRAManagement/SaveManagement',
            data: { MPModel: MPModel },
            dataType: "json"
        });

        return response;
    }


    //this.SaveManagement = function (SaveManagement, ActionModel) {
    //    var response = $http({
    //        method: 'post',
    //        url: '../ProRAManagement/SaveManagement',
    //        data: { MPModel: SaveManagement, lstAM: ActionModel },
    //        dataType: "json"
    //    });
    //    return response;
    //}


    this.getAllManagementProgram = function (Id) {

        var response = $http({
            method: "get",
            params: { RAID: Id },
            url: "../ProRAManagement/getAllManagementProgram",
            dataType: "json"
        });
        return response;
    }

    this.searchManagementProgram = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: "../ProRAManagement/searchManagementProgram",
            dataType: "json"
        });
        return response;
    }

    this.getAllActionsReview = function (ActionID, ReferenceID, ACRiskType) {

        var response = $http({
            method: "POST",
            params: { ActionID: ActionID, ReferenceID: ReferenceID, ACRiskType: ACRiskType, },
            url: "../ProRAManagement/getAllActionsReview",
            dataType: "json"
        });
        return response;
    }

    this.getAllRiskReview = function (ReviewID, ReferenceID, RWRiskType) {

        var response = $http({
            method: "POST",
            params: { ReviewID: ReviewID, ReferenceID: ReferenceID, RWRiskType: RWRiskType, },
            url: "../ProRAManagement/getAllRiskReview",
            dataType: "json"
        });
        return response;
    }

    //this.getAllActions = function (Id) {

    //    var response = $http({
    //        method: "get",
    //        params: { MPID: Id },
    //        url: "../ProRAManagement/getAllActions",
    //        dataType: "json"
    //    });
    //    return response;
    //}

    this.getMgDetailsByRAIDAndIntrestId = function (RiskQualityID, RAID) {
        var response = $http({
            method: "get",
            url: "../ProRAManagement/getMgDetailsByRAIDAndIntrestId",
            params: { RiskQualityID: RiskQualityID, RAID: RAID },
            dataType: "json"
        });
        return response;
    }

    this.getMgEnvironmentDetails = function (RiskEnvironmentID, RAID) {
        var response = $http({
            method: "get",
            url: "../ProRAManagement/getMgEnvironmentDetails",
            params: { RiskEnvironmentID: RiskEnvironmentID, RAID: RAID },
            dataType: "json"
        });
        return response;
    }

    this.getMgHealthSafetyDetails = function (HSID, RAID) {
        var response = $http({
            method: "get",
            url: "../ProRAManagement/getMgHealthSafetyDetails",
            params: { HSID: HSID, RAID: RAID },
            dataType: "json"
        });
        return response;
    }

    //*****************Code For RiskCharts*********************


    this.RiskTyp_getDropdownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "../RiskCharts/RiskTyp_getDropdownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.getRiskChartProcessDropdown = function () {
        var response = $http({

            method: "get",
            url: "../RiskCharts/getRiskChartProcessDropdown",
            dataType: "json"
        })
        return response;
    }


    this.SP_getSipocDetailsForCharts = function (PMID, RiskType) {
        var response = $http({
            method: "get",
            url: "../RiskCharts/SP_getSipocDetailsForCharts",
            params: { PMID: PMID, RiskType: RiskType },
            dataType: "json"
        });
        return response;
    }

    this.getAllRiskDetailsWithAllInstPArties = function (PMID) {
        var response = $http({
            method: "get",
            url: "../RiskCharts/getAllRiskDetailsWithAllInstPArties",
            params: { PMID: PMID },
            dataType: "json"
        });
        return response;
    }

    this.getAllSubProcesses = function (PMID) {
        var response = $http({
            method: "get",
            url: "../RiskCharts/Get_AllSubProcesses",
            params: { PMID: PMID },
            dataType: "json"
        });
        return response;
    }

    this.getAllHiraDetailsBySipocId = function (PMID) {
        var response = $http({
            method: "get",
            url: "../RiskCharts/Get_AllHIRADetailsBySipocId",
            params: { PMID: PMID },
            dataType: "json"
        });
        return response;
    }


    this.getSipocDetailsForTurtleDiagramById = function (Id) {
        var response = $http({
            method: "get",
            url: "../RiskCharts/Get_SipocDetailsForTurtleDiagramById",
            params: { id: Id },
            dataType: "json"
        });
        return response;
    }

    this.getOverAllRiskChart = function () {
        var response = $http({
            method: "get",
            url: "../ProRASetup/getOverAllRiskChart",
            dataType: "json"
        })
        return response;
    }


    //*****************Working Of ProRA End***********************************



    //********************************************* FMEA Start************************************************


    //Save Planning
    this.SaveFMEAPlanning = function (PlanningModel) {
        var response = $http({
            method: 'post',
            url: '/FMEAPlanning/SaveFMEAPlanning',
            data: { PM: PlanningModel },
            dataType: "json"
        });

        return response;
    }

    this.FMEA_getDropDownValues = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "/FMEAPlanning/FMEA_getDropDownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    // dropdown Parts Reference
    this.FMEA_dropdownPartsRef = function () {
        var response = $http({
            method: 'post',
            url: '/FMEAPlanning/FMEA_dropdownPartsRef',
            dataType: "json"
        });
        return response;
    }

    this.FMEA_getUserDropdown = function () {
        var response = $http({
            method: 'post',
            url: "/FMEAPlanning/FMEA_getUserDropdown",
            dataType: "Json"
        });
        return response;
    }

    // FMEA All Planning
    this.FMEA_getAllPlanning = function (Id) {

        var response = $http({
            method: "get",
            params: { FMEAPlanningID: Id },
            url: "/FMEAPlanning/FMEA_getAllPlanning",
            dataType: "json"
        });
        return response;
    }

    this.FMEA_searchPlanning = function (SearchParameter) {
        var response = $http({
            method: 'get',
            params: { SearchParameter: SearchParameter },
            url: '../FMEAPlanning/FMEA_searchPlanning',
            dataType: "json"
        });
        return response;
    }


    //---------------Requirement Page-------------------------------------

    //Save Requirement
    this.SaveFMEARequirement = function (FMEARequirementModel) {
        var response = $http({
            method: 'post',
            url: '/FMEAPlanning/SaveFMEARequirement',
            data: { RM: FMEARequirementModel },
            dataType: "json"
        });

        return response;
    }


    this.FMEA_getAllRequirement = function (Id) {
        var response = $http({
            method: "get",
            params: { FMEAPlanningID: Id },
            url: "/FMEAPlanning/FMEA_getAllRequirement",
            dataType: "json"
        });
        return response;
    }

    //-------------------Potential FME------------------------------------------

    //Save PotentialFME
    this.SavePotentialFME = function (PotentialFMEModel) {
        var response = $http({
            method: 'post',
            url: '/FMEAPlanning/SavePotentialFME',
            data: { PFM: PotentialFMEModel },
            dataType: "json"
        });

        return response;
    }

    this.FMEA_getAllPotentialFME = function (Id) {

        var response = $http({
            method: "get",
            params: { FMEAPlanningID: Id },
            url: "/FMEAPlanning/FMEA_getAllPotentialFME",
            dataType: "json"
        });
        return response;
    }


    //---------------Analysis-----------------------

    //Save Analysis
    this.SaveAnalysis = function (AnalysisModel) {
        var response = $http({
            method: 'post',
            url: '/FMEAPlanning/SaveAnalysis',
            data: { lstAnalysis: AnalysisModel },
            dataType: "json"
        });

        return response;
    }


    this.FMEA_getAllAnalysis = function (Id) {
        var response = $http({
            method: "get",
            params: { FMEAPlanningID: Id },
            url: "/FMEAPlanning/FMEA_getAllAnalysis",
            dataType: "json"
        });
        return response;
    }

    //fetch row
    this.getDetailsOfAnalysis = function (Id) {
        var response = $http({
            method: "get",
            params: { FMEAPlanningID: Id },
            url: "/FMEAPlanning/getDetailsOfAnalysis",
            dataType: "json"
        });
        return response;
    }


    //------------------Action Result-------------------------------------------------

    //Save Action Result
    this.SaveActionResult = function (ActionResultModel) {
        var response = $http({
            method: 'post',
            url: '/FMEAPlanning/SaveActionResult',
            data: { lstActionResult: ActionResultModel },
            dataType: "json"
        });

        return response;
    }

    this.FMEA_getAllActionResult = function (Id) {
        var response = $http({
            method: "get",
            params: { FMEAPlanningID: Id },
            url: "/FMEAPlanning/FMEA_getAllActionResult",
            dataType: "json"
        });
        return response;
    }

    this.getDetailsActionResult = function (Id) {
        var response = $http({
            method: "get",
            params: { FMEAPlanningID: Id },
            url: "/FMEAPlanning/getDetailsActionResult",
            dataType: "json"
        });
        return response;
    }

    //********************************************* FMEA End************************************************


    //********************************************* InOut Register Start************************************************

    //Code for Saving the Covenant Register Details

    this.SaveInOutRegisterDtl = function (SaveInOutRegisterDtl) {
        var response = $http({
            method: 'post',
            url: '/Home/SaveInOutRegisterDtl',
            data: { inOut: SaveInOutRegisterDtl },
            dataType: "json"
        });

        return response;
    }

    this.checkInGaugeFromSupplier = function (checkInGaugeDetails) {
        var response = $http({
            method: 'post',
            url: '/Home/checkInGaugeFromSupplier',
            data: { inOut: checkInGaugeDetails },
            dataType: "json"
        });

        return response;
    }

    //code For Edit  CovenantRegisterDetails
    this.GetAllInOutRegisterDetails = function (Id) {

        var response = $http({
            method: 'get',
            params: { InOutID: Id },
            url: '/Home/GetAllInOutRegisterDetails',
            dataType: "json"
        });
        return response;
    }


    this.searchGaugeInOutDetails = function (SearchParameter) {
        var response = $http({
            method: "get",
            params: { SearchParameter: SearchParameter },
            url: '/Home/searchGaugeInOutDetails',
            dataType: "json"
        });
        return response;
    }

    //********************************************* InOut Register End *************************************************
    this.getServiceProviderNameDropdown = function () {
        var response = $http({
            method: "get",
            url: "/Login/getServiceProviderNameDropdown",
            dataType: "json"
        });
        return response;
    }

    this.loadTestData = function () {
        var response = $http({
            method: "get",
            url: "/CMCharts/CM_getEmployeeNamesDropDownValues",
            dataType: "json"
        })
        return response;
    }

    this.getMTTRMTBFYears = function () {
        var response = $http({
            method: "get",
            url: "/MttrAndMtbf/getAllYears",
            dataType: "json"
        })
        return response;
    }

    this.getMTTR_MTBF_Standards = function (Id) {

        var response = $http({
            method: 'get',
            params: { standardsID: Id },
            url: '/MttrAndMtbf/getMttrMtbfStandards',
            dataType: "json"
        });
        return response;
    }

    this.searchMTTRMTBFStandards = function (SearchParameter) {
        var response = $http({
            method: "get",
            params: { SearchParameter: SearchParameter },
            url: '/MttrAndMtbf/searchMTTRMTBFStandards',
            dataType: "json"
        });
        return response;
    }

    this.generateMTTR_MTBF_Chart = function (yr) {

        var response = $http({
            method: 'get',
            params: { year: yr },
            url: '/MttrAndMtbf/generateMTTR_MTBF_Chart',
            dataType: "json"
        });
        return response;
    }


    this.getTraqceabilityMaster = function (Id) {

        var response = $http({
            method: 'get',
            params: { TraceID: Id },
            url: '/DocPackage/getTraqceabilityMaster',
            dataType: "json"
        });
        return response;
    }

    this.getCheckListStatusByTraceId = function (TraceId,ChecklistId) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceId, ChecklistId: ChecklistId },
            url: '/DocPackage/getCheckListStatusByTraceId',
            dataType: "json"
        });
        return response;
    }

    this.getCustomer = function () {

        var response = $http({
            method: 'get',
            //params: { TraceID: Id },
            url: '/DocPackage/getCustomers',
            dataType: "json"
        });
        return response;
    }

    this.getCheckListDetailsByTraceID = function (Id) {

        var response = $http({
            method: 'get',
            params: { TraceID: Id },
            url: '/DocPackage/getCheckListDetailsByTraceID',
            dataType: "json"
        });
        return response;
    }

    this.getSavedAttachment = function (TraceID, checklistID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID, ChecklistID: checklistID },
            url: '/DocPackage/getSavedAttachment',
            dataType: "json"
        });
        return response;
    }

    this.generateChecklistNo = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID },
            url: '/DocPackage/generateChecklistNo',
            dataType: "json"
        });
        return response;
    }

    this.generateCOCNo = function (TraceID,PartNo) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID, PartNo: PartNo },
            url: '/DocPackage/generateCOCNo',
            dataType: "json"
        });
        return response;
    }

    this.generateCOINo = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID},
            url: '/DocPackage/generateCOINo',
            dataType: "json"
        });
        return response;
    }

    this.generateHardnessCertNo = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID},
            url: '/DocPackage/generateHardnessCertNo',
            dataType: "json"
        });
        return response;
    }

    this.getCOCdetails = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID},
            url: '/DocPackage/getCOCdetails',
            dataType: "json"
        });
        return response;
    }

    this.getCOIdetails = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID },
            url: '/DocPackage/getCOIdetails',
            dataType: "json"
        });
        return response;
    }

    this.getHardnessReportdetails = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID },
            url: '/DocPackage/getHardnessReportdetails',
            dataType: "json"
        });
        return response;
    }

    this.DeleteAttachment = function (FileId,fileName) {

        var response = $http({
            method: 'post',
            data: { fileId: FileId, fileName: fileName },
            url: '../DocPackage/DeleteAttachment',
            dataType: "json"
        });

        //this.UploadBalloningReadings = function (excelData, TraceID) {
        //    var response = $http({
        //        method: "POST",
        //        url: "../DocPackage/UploadBalloningReadings",
        //        data: { items: JSON.stringify(excelData), TraceID: TraceID },
        //        headers: {
        //            'Content-Type': 'application/json'
        //        }
        //    })
        //    return response;
        //}

        return response;
    }

    this.getHardnessReadingdetails = function (TraceID,hardnessType) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID, HardnessType: hardnessType },
            url: '/DocPackage/getHardnessReadingdetails',
            dataType: "json"
        });
        return response;
    }

    this.DP_getDropDownValue = function (promptgroup) {
        var response = $http({
            method: "get",
            url: "/DocPackage/getDropDownValues",
            params: { promptgroup: promptgroup },
            dataType: "json"
        });
        return response;
    }

    this.generateDryHonedCertNo = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID},
            url: '/DocPackage/generateDryHonedCertNo',
            dataType: "json"
        });
        return response;
    }

    this.getDryHonedDetails = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID},
            url: '/DocPackage/getDryHonedDetails',
            dataType: "json"
        });
        return response;
    }

    this.generatePhosphatingCertNo = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID },
            url: '/DocPackage/generatePhosphatingCertNo',
            dataType: "json"
        });
        return response;
    }

    this.getPhosphatingDetails = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID },
            url: '/DocPackage/getPhosphatingDetails',
            dataType: "json"
        });
        return response;
    }

    
    this.getITRMasterDetails = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID },
            url: '/DocPackage/getITRMasterDetails',
            dataType: "json"
        });
        return response;
    }

    this.getITR_MaterialReviewDetails = function (TraceID, Section) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID, Section : Section },
            url: '/DocPackage/getITR_MaterialReviewDetails',
            dataType: "json"
        });
        return response;
    }

    this.UploadBalloningReadings = function (excelData,TraceID) {
        var response = $http({
            method: "POST",
            url: "../DocPackage/UploadBalloningReadings",
            data: { items: JSON.stringify(excelData) ,TraceID: TraceID},
            headers: {
                'Content-Type': 'application/json'
            }
        })
        return response;
    }

    this.getITR_BallooningReadings = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID },
            url: '/DocPackage/getITR_BallooningReadings',
            dataType: "json"
        });
        return response;
    }

    this.generateLPCertNo = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID },
            url: '/DocPackage/generateLPCertNo',
            dataType: "json"
        });
        return response;
    }

    this.getLPdetails = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID },
            url: '/DocPackage/getLPdetails',
            dataType: "json"
        });
        return response;
    }

    this.generateMPICertNo = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID },
            url: '/DocPackage/generateMPICertNo',
            dataType: "json"
        });
        return response;
    }

    this.getMPIdetails = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID },
            url: '/DocPackage/getMPIdetails',
            dataType: "json"
        });
        return response;
    }

    this.getAllsignatures = function (SignatureID) {

        var response = $http({
            method: 'get',
            params: { SignatureID: SignatureID },
            url: '/Signature/getAllSignatureDetails',
            dataType: "json"
        });
        return response;
    }

    this.getCompletedDocumentList = function (TraceId) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceId },
            url: '/DocPackage/getCompletedDocumentList',
            dataType: "json"
        });
        return response;
    }

    this.convertotoPDF = function (html) {

        var response = $http({
            method: 'get',
            params: { html: html },
            url: '/DocPackage/htmlTopdf',
            dataType: "json"
        });
        return response;
    }

    this.selectPDFPrint = function () {

        var response = $http({
            method: 'get',
            //params: { html: html },
            url: '/DocPackage/htmlTopdfRocket',
            dataType: "json"
        });
        return response;
    }

    this.getChecklistParentDetails = function (TraceID) {

        var response = $http({
            method: 'get',
            params: { TraceID: TraceID },
            url: '/DocPackage/getChecklistParentDetails',
            dataType: "json"
        });
        return response;
    }

    this.GenerateIAReport = function (FileName,fromDate, toDate, ReportTypeID, ShiftID) {
        var response = $http({
            method: 'post',
            url: '/IAReport/GenerateIAReport',
            data: {FileName: FileName, fromDate: fromDate, toDate: toDate, ReportTypeId: ReportTypeID, ShiftID : ShiftID },
            dataType: "json"
        });

        return response;
    }
   

});


