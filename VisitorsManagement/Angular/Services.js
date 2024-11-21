app.service("myService", function ($http) {

    this.VM_getAllAppointment = function (filter) {

        var response = $http({
            method: "post",
            data: { filter: filter },
            url: baseURL +  "VM/getAllAppointment",
            dataType: "json"
        });
        return response;
    }


    this.VM_SearchAppointment = function (searchParameter) {

        var response = $http({
            method: "get",
            params: { searchParameter: searchParameter },
            url: baseURL + "VM/VM_SearchAppointment",
            dataType: "json"
        });
        return response;
    }

    this.VM_saveAppointment = function (VM_saveAppointment) {
        var response = $http({
            method: 'post',
            url: baseURL +  "VM/raiseAppointment",
            data: { vmmodel: VM_saveAppointment },
            dataType: "json"
        });

        return response;
    }

     this.VM_CheckIn = function (checkInAppointment) {
        var response = $http({
            method: 'post',
            url: baseURL +  "VM/CheckInAppointment",
            data: { filter: checkInAppointment },
            dataType: "json"
        });

        return response;
     }

     this.VM_CheckOut = function (checkOutAppointment) {
         var response = $http({
             method: 'post',
             url: baseURL +  "VM/CheckOutAppointment",
             data: { filter: checkOutAppointment },
             dataType: "json"
         });

         return response;
     }

     this.VM_CancelAppointment = function (cancelAppointment) {
         var response = $http({
             method: 'post',
             url: baseURL +  "VM/CancelAppointment",
             data: { filter: cancelAppointment },
             dataType: "json"
         });

         return response;
     }
     

     this.getUserDropdown = function () {
         var response = $http({
             method: 'post',
             url: baseURL +  "Users/GetUserSelectList",
             dataType: "json"
         });

         return response;
     }

     this.GetUserSelectListOnlyEmployee = function () {
         var response = $http({
             method: 'post',
             url: baseURL +  "Users/GetUserSelectListOnlyEmployee",
             dataType: "json"
         });

         return response;
     }

     this.GenerateChart = function (chartFilter) {
         var response = $http({
             method: 'post',
             data: { filter: chartFilter },
             url: baseURL +  "VM/GenerateChart",
             dataType: "json"
         });

         return response;
     }



     this.ApproveRejectAppointment = function (approveReject) {
         var response = $http({
             method: "post",
             url: baseURL + "VM/ApproveRejectAppointment",
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
             url: baseURL + "WP/getAllWorkPermit",
             dataType: "json"
         });
         return response;
     }

     this.SendReminder = function (filter) {

         var response = $http({
             method: "post",
             data: { filter: filter },
             url: baseURL + "WP/SendReminder",
             dataType: "json"
         });
         return response;
     }

     this.getContractorSelectList = function () {

         var response = $http({
             method: "post",
             url: baseURL + "Contractor/getContractorSelectList",
             dataType: "json"
         });
         return response;
     }

     this.ApproveRejectWP = function (approveReject) {

         var response = $http({
             method: "post",
             data: { approveRejectWP: approveReject },
             url: baseURL + "WP/ApproveRejectWP",
             dataType: "json"
         });
         return response;
     }


     this.getEmployeeFile = function (employeeDetails) {

         var response = $http({
             method: "post",
             data: { filter: employeeDetails },
             url: baseURL + "Contractor/getEmployeeFile",
             dataType: "json"
         });
         return response;
     }

     this.getEmployeeFileWP = function (employeeDetails) {

         var response = $http({
             method: "post",
             data: { details: employeeDetails },
             url: baseURL + "WP/getWPFile",
             dataType: "json"
         });
         return response;
     }







    //==============User Management ================================================================


     this.GetDefaultClains = function () {

         var response = $http({
             method: "post",
             url: baseURL + "Users/GetDefaultClaims",
             dataType: "json"
         });
         return response;
     }


     this.SaveUser = function (userDetails) {

         var response = $http({
             method: "post",
             data: { user: userDetails },
             url: baseURL + "Users/CreateUser",
             dataType: "json"
         });
         return response;
     }



     this.GetUsers = function (filter) {

         var response = $http({
             method: "post",
             data: { filter: filter },
             url: baseURL + "Users/GetUsers",
             dataType: "json"
         });
         return response;
     }


     this.GetAllContractor = function (filter) {

         var response = $http({
             method: "post",
             data: { filter: filter },
             url: baseURL + "Contractor/getAllContractor",
             dataType: "json"
         });
         return response;
     }

     this.GetAllEmployee = function (filter) {

         var response = $http({
             method: "post",
             data: { filter: filter },
             url: baseURL + "Contractor/getAllEmployee",
             dataType: "json"
         });
         return response;
     }

     this.SaveContractor = function (contractorDetails) {

         var response = $http({
             method: "post",
             data: { contractor: contractorDetails },
             url: baseURL + "Contractor/CreateContractor",
             dataType: "json"
         });
         return response;
     }


     this.SaveWorkPermit = function (workPermitModel) {
         debugger;
         workPermitModel.listEmployee = null;
         var response = $http({
             method: "post",
             data: { wp: workPermitModel },
             url: baseURL + "WP/SaveWorkPermit",
             dataType: "json"
         });
         return response;
     }

     this.SaveSafetyTraining = function (safetyTraining) {

         var response = $http({
             method: "post",
             data: { safetyTraining: safetyTraining },
             url: baseURL + "WP/SaveSafetyTraining",
             dataType: "json"
         });
         return response;
     }

     this.removeEmployee = function (employeeDetails) {

         var response = $http({
             method: "post",
             data: { filter: employeeDetails },
             url: baseURL + "WP/removeEmployee",
             dataType: "json"
         });
         return response;
     }

     this.closeWorkPermit = function (filter) {

         var response = $http({
             method: "post",
             data: { filter: filter },
             url: baseURL + "WP/closeWorkPermit",
             dataType: "json"
         });
         return response;
     }


     this.ChangePassword = function (userModel) {
         var response = $http({
             method: 'post',
             data: { change: userModel },
             url: baseURL + "Login/ChangePassword",
             headers: {
                 'Content-Type': 'application/json'
             }
         });
         return response;
     }

     this.UploadEmployeeDetails = function (excelData, ContractorID) {
         var response = $http({
             method: "POST",
             url: "../Contractor/UploadEmployeeDetails",
             data: { items: JSON.stringify(excelData), ContractorId: ContractorID },
             headers: {
                 'Content-Type': 'application/json'
             }
         })
         return response;
     }


     this.downloadVMReport = function (reportModel) {
         var response = $http({
             method: 'post',
             data: { report: reportModel },
             url: baseURL + "Report/DownloadVMReport",
             headers: {
                 'Content-Type': 'application/json'
             }
         });
         return response;
    }

    //----------------Remote Employee-----------------
    this.getAllRemoteEmployee = function (filter) {

        var response = $http({
            method: "post",
            data: { filter: filter },
            url: baseURL + "RemoteEmployee/getRemoteEmployee",
            dataType: "json"
        });
        return response;
    }

    this.SaveRemoteEmployee = function (RemoteEmployeeModel) {
        var response = $http({
            method: 'post',
            url: baseURL + "RemoteEmployee/SaveRemoteEmployee",
            data: { RemoteEmployeeModel: RemoteEmployeeModel },
            dataType: "json"
        });

        return response;
    }

    //----------End Remote employee-------------

});


