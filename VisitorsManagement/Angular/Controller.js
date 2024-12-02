
app.controller("VisitorsMgmtCtrl", function ($scope, $http, myService, $timeout, Upload, NgTableParams, $window, constants, toaster) {


    $scope.showTable = true;
    $scope.AppointmentModel = {};
    $scope.StatusMedia = constants.VMStatus;
    $scope.VisitPurpose = [{ text: 'Official', value: 'Official' }, { text: 'Personal', value: 'Personal' }];
    $scope.isSecurityRole = false;
    $scope.CheckinModel = {};
    $scope.CheckOutModel = {};
    $scope.VistorMobiles = [];


    // retrieves the min 'id' of a collection, used for the group ordering.
    // you can use lodash instead. e.g: _.min(arr, 'id') 
    $scope.min = function (arr) {
        return $filter('min')
            ($filter('map')(arr, 'id'));
    }


    if (localStorage.getItem("RoleName") == "Security") {
        $scope.isSecurityRole = true;
    }




    $scope.AddNewRecord = function () {
        //$scope.showTable = false;
        $scope.AppointmentModel = {};
        $scope.AppointmentModel.UserFullName = localStorage.getItem("UserFullName");

        if (localStorage.getItem("RoleName") == "Security") {
            $scope.AppointmentModel.Status = 'Waiting for approval';
        }
        else {
            $scope.AppointmentModel.Status = constants.VMStatus.Open;
            $scope.AppointmentModel.PersonToVisitID = localStorage.getItem("UserID");
        }
        $("#divVisitorPhoneNumber_value").val("").trigger('change');
        $("#divVisitorVisitorName_value").val("").trigger('change');

        $('#ddlPurpose').selectpicker('val', '').trigger("change");
    }

    $scope.downloadReport = function (AppointmentID) {

        var response = $http({
            method: 'get',
            params: { AppointmentId: AppointmentID },
            url: '../VM/downloadReport',
            dataType: "json"
        });

        response.then(function (d) {
            if (d.data.isSuccessful == true) {
                //window.location.href = d.data.message;

                var file_path = d.data.message;
                //var a = document.createElement('a');
                //a.href = file_path;
                //a.download = file_path.substr(file_path.lastIndexOf('/') + 1);
                //document.body.appendChild(a);
                //a.click();
                //document.body.removeChild(a);

                printJS(file_path);

            }
            else {
                alert(d.data.message);
            }

        }, function () {
            console.log('Error Occured');
        })
    }

    $scope.$watch(function () {
        $('#ddlPurpose').selectpicker('refresh');
        $('#ddlEmployee').selectpicker('refresh');
    });

    //SAVE Appointment details
    $scope.VM_saveAppointment = function () {

        $scope.AppointmentModel.PurposeToVisit = $('#ddlPurpose').val();
        $scope.AppointmentModel.PersonToVisitID = localStorage.getItem("UserID");
        if (localStorage.getItem("RoleName") == "Security") {
            $scope.AppointmentModel.PersonToVisitID = $('#ddlEmployee').val();
        }
        $scope.AppointmentModel.VisitorPhoneNumber = $("#divVisitorPhoneNumber_value").val();
        $scope.AppointmentModel.VisitorName = $("#divVisitorVisitorName_value").val();

        //$scope.AppointmentModel.Address = "test";
        //$scope.AppointmentModel.Date = "16-Dec-2021";
        //$scope.AppointmentModel.NumberOfPerson = 1;
        //$scope.AppointmentModel.PersonToVisitID = "6";
        //$scope.AppointmentModel.PurposeToVisit = "Official";
        //$scope.AppointmentModel.Remark = "NA";
        //$scope.AppointmentModel.RepresentingCompany = "test";
        //$scope.AppointmentModel.Status = "Open";
        //$scope.AppointmentModel.UserFullName = "Suresh Magadum";
        //$scope.AppointmentModel.VisitorName = "Test 34";
        //$scope.AppointmentModel.VisitorPhoneNumber = "2342342342";
        //$scope.AppointmentModel.VisitorsEmails = "prashantkurde12@gmail.com";

        var response = myService.VM_saveAppointment($scope.AppointmentModel); // get call from service.js
        response.then(function (d) {

            if (d.data.isSuccessful) {
                $scope.AppointmentModel = null;
                $scope.showTable = true;
                $scope.VM_getAllAppointment();
                $scope.VM_getAllAppointmentAutoComplte("ForAutocomplete");
                $('#exampleModal').modal('hide');
                toaster.success("Visitor's Management", "Appointment Raised Successfully.");
            }
            else {
                toaster.error("Visitor's Management", d.data.message);
            }

        }).catch(function (error) {
            console.log(error);
            toaster.error("Visitor's Management", "Error Occured.");
        })

    }

    $scope.approveAppointment = function (Id) {
        var approve = { Status: constants.VMStatus.Open, AppointmentID: Id }
        var response = myService.ApproveRejectAppointment(approve); // get call from service.js
        response.then(function (d) {
            if (d.data.isSuccessful) {
                $scope.VM_getAllAppointment();
                toaster.success("Visitor's Management", "Visitor Appointment Approved Successfully.");
            }
            else {
                toaster.error("Visitor's Management", d.data.message);
            }
        }), function (error) {
            toaster.success("Visitor's Management", "Error Occured.");
        }
    }

    $scope.rejectAppointment = function (Id) {
        var reject = { Status: constants.VMStatus.Reject, AppointmentID: Id }
        var response = myService.ApproveRejectAppointment(reject); // get call from service.js
        response.then(function (d) {
            if (d.data.isSuccessful) {
                $scope.VM_getAllAppointment();
                toaster.success("Visitor's Management", "Visitor Appointment Rejected Successfully.");
            }
            else {
                toaster.error("Visitor's Management", d.data.message);
            }
        }), function () {
            toaster.success("Visitor's Management", "Error Occured.");
        }
    }

    $scope.CheckInAppointment = function (appointmentID, visitorsName, numberOfPerson) {
        $scope.CheckinModel = {};
        $scope.CheckinModel.VisitorsName = visitorsName;
        $scope.CheckinModel.AppointmentID = appointmentID;
        $scope.CheckinModel.NumberOfPerson = numberOfPerson;
        $('#checkinModal').modal('show');
    }

    $scope.CheckOut = function (appointmentID, visitorsName) {
        $scope.CheckOutModel.AppointmentID = appointmentID;
        $scope.CheckOutModel.VisitorsName = visitorsName;
        $('#checkOutModal').modal('show');
    }

    $scope.CheckIn = function () {
        $scope.CheckinModel.Status = constants.VMStatus.CheckIn;
        var time = new Date();
        $scope.CheckinModel.Time = time.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
        var response = myService.VM_CheckIn($scope.CheckinModel); // get call from service.js
        response.then(function (d) {
            if (d.data.isSuccessful) {
                $scope.CheckinModel = null;
                $scope.VM_getAllAppointment();
                $('#checkinModal').modal('hide');
                toaster.success("Visitor's Management", "Visitor Checked In Successfully.");
            }
            else {
                toaster.error("Visitor's Management", d.data.message);
            }

        }), function () {
            toaster.success("Visitor's Management", "Error Occured.");
        }
    }

    $scope.CheckOutAppointment = function () {
        var time = new Date();
        var checkOutTime = time.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
        var checkOut = { Status: constants.VMStatus.CheckOut, AppointmentID: $scope.CheckOutModel.AppointmentID, Time: checkOutTime, Narration: $scope.CheckOutModel.Narration };

        var response = myService.VM_CheckOut(checkOut); // get call from service.js
        response.then(function (d) {
            if (d.data.isSuccessful) {
                toaster.success("Visitor's Management", "Visitor Checked Out Successfully.");
                $('#checkOutModal').modal('hide');
                $scope.VM_getAllAppointment();
            }
            else {
                toaster.error("Visitor's Management", d.data.message);
            }

        }), function () {
            toaster.success("Visitor's Management", "Error Occured.");
        }
    }

    $scope.CancelAppointment = function (appointmentId) {

        var cancelAppointment = { Status: constants.VMStatus.Cancel, AppointmentID: appointmentId }

        var response = myService.VM_CancelAppointment(cancelAppointment); // get call from service.js
        response.then(function (d) {
            if (d.data.isSuccessful) {
                $scope.VM_getAllAppointment();
                toaster.success("Visitor's Management", "Visitor Appointment Cancelled Successfully.");
            }
            else {
                toaster.error("Visitor's Management", d.data.message);
            }

        }), function () {
            toaster.success("Visitor's Management", "Error Occured.");
        }
    }


    //Cancel Edit
    $scope.CancelEdit = function () {
        $scope.showTable = true;
    }
    //list of details
    $scope.VM_getAllAppointment = function () {

        var filter = { 'AppointmentId': 0, FilterText: $scope.searchFilter };
        var response = myService.VM_getAllAppointment(filter);

        response.then(function (res) {
            //-----Bind Data Table----------

            $scope.data = res.data;
            $scope.searchFilter = '';
            $scope.TotalRecords = $scope.data.length;
            $scope.showTable = true;


            $scope.tableParams = new NgTableParams({
                page: 1,
                count: 200,
                total: res.data.length,
                group: {
                    DateInGlobalFormate: "desc"
                },
                // group: "strDate ,GroupSort:desc",               
                getData: function ($defer, params) {
                    $defer.resolve($scope.data.slice((params.page() - 1) * params.count(), params.page() * params.count())
                    );
                }
            }, {
                dataset: res.data,
                groupOptions: {
                    isExpanded: true
                }
            });



            $timeout(function () {
                freezeTable.update();
            }, 1000);

            //-------Bind Data Table---------------
        }, function () {
            console.log('Error Occured');
        })
    }
    $scope.VM_getAllAppointment();
    $scope.VM_getAllAppointmentAutoComplte = function (ForAutocomplete) {

        var filter = { 'AppointmentId': 0, FilterText: $scope.searchFilter, "ForAutocomplete": ForAutocomplete };
        var response = myService.VM_getAllAppointment(filter);

        response.then(function (res) {
            $scope.VistorMobiles = res.data;

            //-------Bind Data Table---------------
        }, function () {
            console.log('Error Occured');
        })
    }

    $scope.VM_getAllAppointmentAutoComplte("ForAutocomplete");



    $scope.switchGroup = function (group, groups) {
        //if (group.$hideRows) {
        //    angular.forEach(groups, function (g) {
        //        if (g !== group) {
        //            g.$hideRows = true;
        //        }
        //    });
        //}

        group.$hideRows = !group.$hideRows;
    };

    //Edit
    $scope.EditDetails = function (Id) {
        //$scope.showTable = false;
        //$window.scrollTo(0, 0);
        var filter = { 'AppointmentId': Id, FilterText: '' };
        var response = myService.VM_getAllAppointment(filter);
        response.then(function (response) {
            $scope.AppointmentModel = response.data[0];
            $scope.AppointmentModel.Date = $scope.AppointmentModel.strDate;
            //$('#ddlPrepared').selectpicker('val', $scope.AppointmentModel.PersonToVisitID).trigger("change");
            $('#exampleModal').modal('show');

            $('#ddlPurpose').selectpicker('val', $scope.AppointmentModel.PurposeToVisit).trigger("change");


        }, function () {
            console.log("Error Occurs");
        });
    }
    $scope.SelectedVisitorForAutocomplete = function (selected) {
        try {


            if (selected != undefined) {
                if (selected.originalObject.AppointmentID != undefined) {
                    var filter = { 'AppointmentId': selected.originalObject.AppointmentID, FilterText: '' };
                    var response = myService.VM_getAllAppointment(filter);
                    response.then(function (response) {

                        var OrogenalDate = $scope.AppointmentModel.Date;
                        $scope.AppointmentModel = {};
                        $scope.AppointmentModel = response.data[0];
                        $scope.AppointmentModel.AppointmentID = null;
                        $scope.AppointmentModel.AppointmentNo = null;
                        $scope.AppointmentModel.Date = OrogenalDate;
                        $scope.AppointmentModel.Status = "Open";
                        $scope.AppointmentModel.NumberOfPerson = null;
                        $scope.AppointmentModel.Remark = null;

                        $("#divVisitorPhoneNumber_value").val($scope.AppointmentModel.VisitorPhoneNumber).trigger('change');
                        $("#divVisitorVisitorName_value").val($scope.AppointmentModel.VisitorName).trigger('change');;
                        $('#ddlPurpose').selectpicker('val', $scope.AppointmentModel.PurposeToVisit).trigger("change");
                        $scope.AppointmentModel.UserFullName = localStorage.getItem("UserFullName");
                        if (localStorage.getItem("RoleName") == "Security") {
                            $scope.AppointmentModel.Status = 'Waiting for approval';
                        }
                        else {
                            $scope.AppointmentModel.Status = constants.VMStatus.Open;
                            $scope.AppointmentModel.PersonToVisitID = localStorage.getItem("UserID");
                        }

                    }, function () {
                        console.log("Error Occurs");
                    });

                }
                else {
                    //  $scope.AppointmentModel.VisitorName = selected.originalObject;
                }
            }
            else {
                // $scope.AppointmentModel.VisitorName = selected.originalObject;
            }
        }
        catch (err) {

        }

    }
    $scope.getUserDropdown = function () {
        var response = myService.getUserDropdown();

        response.then(function (res) {
            $scope.employeeMaster = res.data;
        }, function () {
            console.log('Error Occured');
        })
    }

    $scope.getUserDropdown();

    $scope.VM_SearchAppointment = function () {
        var filter = { 'AppointmentId': 0, FilterText: $scope.searchFilter };
        var response = myService.VM_getAllAppointment(filter)
        response.then(function (res) {
            $scope.data = res.data;
            $scope.TotalRecords = $scope.data.length;
            $scope.tableParams = new NgTableParams({
                page: 1,
                count: 25,
                total: $scope.data.length,
                getData: function ($defer, params) {
                    $defer.resolve($scope.data.slice((params.page() - 1) * params.count(), params.page() * params.count())
                    );
                }
            }, { dataset: res.data });

        }, function () {

            console.error('error');
        })
    }

    // User Access Validation Start
    //$scope.PageID = constants.VisitorsManagement.Appointment;
    //$scope.validateUserAccess = function () {
    //    var response = $http({
    //        method: "post",
    //        url: "/Login/validateUserAccess",
    //        params: { PageID: $scope.PageID },
    //        dataType: "json"
    //    });
    //    response.then(function (res) {
    //        $scope.Edit = (/true/i).test(res.data);
    //    }, function () {
    //        console.log('Error Occured');
    //    })
    //}
    //$scope.validateUserAccess();

    // User Access Validation End
});
app.controller("ctrlAppointmentApproval", function ($scope, $http, myService, $timeout, Upload, NgTableParams, $window, constants, toaster) {
    //Edit

    $scope.StatusMedia = constants.VMStatus;

    $scope.EditDetails = function (Id) {
        //$scope.showTable = false;
        //$window.scrollTo(0, 0);
        var filter = { 'AppointmentId': Id, FilterText: '' };
        var response = myService.VM_getAllAppointment(filter);
        response.then(function (response) {
            $scope.visitorModel = response.data[0];
            $scope.visitorModel.Date = $scope.visitorModel.strDate;
        }, function () {
            console.log("Error Occurs");
        });
    }

    var value = $("#appointmentId").val();
    $scope.EditDetails(value);


    $scope.approveAppointment = function () {
        var approve = { Status: constants.VMStatus.Open, AppointmentID: value }
        var response = myService.ApproveRejectAppointment(approve); // get call from service.js
        response.then(function (d) {
            if (d.data.isSuccessful) {
                $scope.EditDetails(value);
                toaster.success("Visitor's Management", "Visitor Appointment Approved Successfully.");
            }
            else {
                toaster.error("Visitor's Management", d.data.message);
            }
        }), function () {
            toaster.success("Visitor's Management", "Error Occured.");
        }
    }

    $scope.rejectAppointment = function () {
        var reject = { Status: constants.VMStatus.Reject, AppointmentID: value }
        var response = myService.ApproveRejectAppointment(reject); // get call from service.js
        response.then(function (d) {
            if (d.data.isSuccessful) {
                $scope.EditDetails(value);
                toaster.success("Visitor's Management", "Visitor Appointment Rejected Successfully.");
            }
            else {
                toaster.error("Visitor's Management", d.data.message);
            }
        }), function () {
            toaster.success("Visitor's Management", "Error Occured.");
        }
    }


})

app.controller("WorkPermitCtrl", function ($scope, $http, myService, $timeout, Upload, NgTableParams, $window, constants, toaster) {

    $scope.showTable = true;
    $scope.WorkPermitModel = {};
    $scope.selectedSafetyTrainings = [];
    $scope.workPermitType = ['Hot Working(Electrical Welding,Flame cutting,Grinding etc.)', 'Working at Height', 'Excavation(digging,trenching,excavation etc.)', 'Working in Confined Space(trench,tank,vessel etc.)', 'Electrical Permit(Work on any electric installation,panels,transformers etc.)'];
    $scope.contractorList = [];
    $scope.userList = [];
    $scope.employeeDetails = [];
    $scope.EmployeeModel = [];
    $scope.employeeCount = 0;
    $scope.SafetyTraining = [
        { id: 1, type: "Use of PPE" },
        { id: 2, type: "Log Out / Tag Out" },
        { id: 3, type: "Lifting Tool & Tackles" },
        { id: 4, type: "Unsafe Conditions" }
    ];

    $scope.WPStatus = constants.WPStatus;

    $scope.isHRApprovalEnabled = false;
    $scope.isIMSApprovalEnabled = false;
    $scope.isFinalpprovalEnabled = false;
    $scope.isSafetyTrainingEnabled = false;

    $scope.downloadWPType = function () {
        var type = $('#ddlWPType').val();

        if (type == "" || type == undefined) {
            alert('Please Select Work Permit Type');
            return;
        }

        var fileName = '';
        if (type == 'Hot Working(Electrical Welding,Flame cutting,Grinding etc.)')
            fileName = 'Hot Work Instruction.pdf';
        else if (type == 'Working at Height')
            fileName = 'Working At Height Instruction.pdf';
        else if (type == 'Excavation(digging,trenching,excavation etc.)')
            fileName = 'Instruction for Excavation Work.pdf';
        else if (type == 'Working in Confined Space(trench,tank,vessel etc.)')
            fileName = 'Working At Confined Space Instruction.pdf';
        else if (type == 'Electrical Permit(Work on any electric installation,panels,transformers etc.)')
            fileName = 'Electrification Instruction.pdf';

        if (fileName == '') {
            alert('Attachment not available');
            return;
        }

        var file_path = 'Attachments/' + fileName;
        var a = document.createElement('a');
        a.href = file_path;
        a.download = fileName;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    }


    $scope.isSuperAdmin = false;
    if (localStorage.getItem('RoleName') == 'Super Admin') {
        $scope.isSuperAdmin = true;
    }


    $scope.CheckApproval = function () {
        $scope.isHRApprovalEnabled = false;
        $scope.isIMSApprovalEnabled = false;
        $scope.isFinalpprovalEnabled = false;

        if ($scope.WorkPermitModel.HRId == localStorage.getItem("UserID") && $scope.WorkPermitModel.Status == $scope.WPStatus.WaitingforHRAction) {
            $scope.isHRApprovalEnabled = true;
        }
        else if ($scope.WorkPermitModel.FinalId == localStorage.getItem("UserID") && $scope.WorkPermitModel.Status == $scope.WPStatus.WaitingforManagerApproval) {
            $scope.isFinalpprovalEnabled = true;
        }
        else if ($scope.WorkPermitModel.IMSId == localStorage.getItem("UserID") && $scope.WorkPermitModel.Status == $scope.WPStatus.WaitingforSafetyTrainingEHSApproval) {
            $scope.isIMSApprovalEnabled = true;
            $scope.isSafetyTrainingEnabled = true;
            setTimeout(function () {
                $('#sandbox-containerTraining .input-append.date').datepicker({
                    format: "dd-M-yyyy",
                    todayBtn: true,
                    autoclose: true,
                    todayHighlight: true,
                    calendarWeeks: true,
                    startDate: new Date($scope.WorkPermitModel.strWPDate),
                    endDate: new Date($scope.WorkPermitModel.strWorkStartDate),
                });
            }, 2000);
        }



    }



    $scope.downloadReport = function () {

        var response = $http({
            method: 'get',
            params: { WPID: $scope.WorkPermitModel.WPID },
            url: '../WP/downloadReport',
            dataType: "json"
        });

        response.then(function (d) {
            if (d.data.isSuccessful == true) {
                //window.location.href = d.data.message;

                var file_path = d.data.message;
                //var a = document.createElement('a');
                //a.href = file_path;
                //a.download = file_path.substr(file_path.lastIndexOf('/') + 1);
                //document.body.appendChild(a);
                //a.click();
                //document.body.removeChild(a);
                printJS(file_path);
            }
            else {
                alert(d.data.message);
            }

        }, function () {
            console.log('Error Occured');
        })
    }

    //$scope.getEmployeeFile = function (employeeId, type) {
    //    var filter = { EmployeeId: employeeId, fileType: type };
    //    var response = myService.getEmployeeFile(filter);
    //    response.then(function (response) {
    //        if (response.data.isSuccessful) {
    //            var element = document.createElement('a');
    //            element.setAttribute('href', response.data.data);
    //            element.setAttribute('download', response.data.message);

    //            element.style.display = 'none';
    //            document.body.appendChild(element);

    //            element.click();

    //            document.body.removeChild(element);
    //        }
    //        else {
    //            toaster.error("Visitor's Management", "Error Occured");
    //        }
    //    }, function () {
    //        console.log("Error Occurs");
    //    });
    //}
    $scope.getEmployeeFile = function (employeeId, type) {
        var filter = { EmployeeId: employeeId, ContractorId: $('#ddlContractor').val(), fileType: type };
        var response;
        if ($scope.WorkPermitModel.WPID == undefined || $scope.WorkPermitModel.WPID == 0)
            response = myService.getEmployeeFile(filter);
        else
            response = myService.getEmployeeFileWP(filter);


        response.then(function (response) {
            if (response.data.isSuccessful) {
                $('#iframeModal').modal('show');
                document.getElementById('iframeShowFile').src = response.data.data;
            }
            else {
                toaster.error("Work Permit", "Error Occured");
            }
        }, function () {
            console.log("Error Occurs");
        });
    }


    $scope.$watch(function () {
        $('#ddlContractor').selectpicker('refresh');
        $('#ddlHRApprover').selectpicker('refresh');
        $('#ddlIMSApprover').selectpicker('refresh');
        $('#ddlmgrApprover').selectpicker('refresh');
        $('#ddlEmployee').selectpicker('refresh');

    });

    $scope.setLicenseDetails = function () {
        //$scope.employeeDetails = [];
        var licensedetails = $("#ddlContractor").find(':selected').data('licensedetails');
        if (!$scope.$$phase) {
            $scope.$apply(function () {
                $scope.WorkPermitModel.LicenseDetails = licensedetails;
                $scope.getEmployeeDetailsByContractorId($("#ddlContractor").val());
            });
        }
        else {
            $scope.WorkPermitModel.LicenseDetails = licensedetails;
            $scope.getEmployeeDetailsByContractorId($("#ddlContractor").val());
        }



    }

    $scope.ScopeRefreshContractor = function () {
        $scope.getContractorSelectList();

    }
    $scope.ScopeRefreshEmployee = function () {
        $scope.getContractorSelectList();

    }



    $scope.getEmployeeDetailsByContractorId = function (ContractorId) {
        var filter = { 'ContractorId': ContractorId, FilterText: '' };
        var response = myService.GetAllEmployee(filter);

        response.then(function (res) {
            $scope.ddlEmployees = res.data;
        }, function (error) {
            console.log(error);
        })
    }


    $scope.getCheckedTrainings = function () {
        var checkedTrainings = '';
        $scope.SafetyTraining.forEach(function (training) {

            if (training.selected) {
                if (checkedTrainings != '') {
                    checkedTrainings += " , ";
                }
                checkedTrainings += training.type;
            }
        });
        $scope.WorkPermitModel.SafetyTraining = checkedTrainings;

    }

    $scope.AddNewRecord = function () {
        $scope.showTable = false;
        $scope.WorkPermitModel = {};
        $scope.WorkPermitModel.WPID = 0;
        $scope.WorkPermitModel.WPNO = '';
        $scope.WorkPermitModel.Status = constants.WPStatus.Open;
        $scope.WorkPermitModel.InitiatedByName = localStorage.getItem("UserFullName");
        $scope.WorkPermitModel.InitiatedById = localStorage.getItem("UserID");
        $scope.WorkPermitModel.WPDate = new Date().toShortFormat();
        $scope.employeeDetails = [];
        $scope.SafetyTraining.forEach(function (training) {
            training.selected = false;
        });

        $('#ddlWPType').selectpicker('val', '');
        $('#ddlContractor').selectpicker('val', '');
        $('#ddlEmployee').selectpicker('val', '');
        //$('#ddlHRApprover').selectpicker('val', '');
        //$('#ddlIMSApprover').selectpicker('val', '');
        $('#ddlmgrApprover').selectpicker('val', '');


        var hrApprover = $scope.userList.filter(function (itm) {
            return itm.Text == 'girish shinde' || itm.Text == 'Girish Shinde';
        });

        if (hrApprover.length > 0) {
            $scope.WorkPermitModel.HRApproverName = hrApprover[0].Text;
            $scope.WorkPermitModel.HRId = hrApprover[0].ValueInt;
            $('#ddlHRApprover').selectpicker('val', $scope.WorkPermitModel.HRId);
        }

        var imsApprover = $scope.userList.filter(function (itm) {
            return itm.Text == 'Rupesh Baviskar';
        });

        if (imsApprover.length > 0) {
            $scope.WorkPermitModel.IMSApproverName = imsApprover[0].Text;
            $scope.WorkPermitModel.IMSId = imsApprover[0].ValueInt;
            $('#ddlIMSApprover').selectpicker('val', $scope.WorkPermitModel.IMSId);
        }


    }

    $scope.CancelSave = function () {
        $scope.showTable = true;
        $scope.getAllWorkPermit();
    }

    $scope.tableParamsChild = new NgTableParams({
        page: 1,   // show first page
        count: 5  // count per page
    }, {
        counts: [], // hide page counts control
        total: 1,  // value less than count hide pagination
        getData: function ($defer, params) {
            $defer.resolve(data);
        }
    });


    //new ngTableParams({
    //    page: 1,
    //    count: 3,
    //    noPager: true,
    //    counts: [] 
    //}

    $scope.getContractorSelectList = function () {
        var response = myService.getContractorSelectList();

        response.then(function (res) {
            $scope.contractorList = res.data;
        });
    };

    $scope.getContractorSelectList();

    $scope.GetUserSelectListOnlyEmployee = function () {
        var response = myService.GetUserSelectListOnlyEmployee();

        response.then(function (res) {
            $scope.userList = res.data;
        });
    };

    $scope.GetUserSelectListOnlyEmployee();

    $scope.SaveEmployee = function () {
        if ($scope.EmployeeModel.SrNo == null || $scope.EmployeeModel.SrNo == 'undefined') {
            $scope.employeeCount++;
            $scope.EmployeeModel.SrNo = $scope.employeeCount;
            $scope.employeeDetails.push($scope.EmployeeModel);
        }
        else {
            for (i in $scope.employeeDetails) {
                if ($scope.employeeDetails[i].SrNo == $scope.EmployeeModel.SrNo) {
                    $scope.employeeDetails[i] = $scope.EmployeeModel;
                }
            }
        }
        $scope.EmployeeModel = [];
        $('input[type="file"]').each(function (index) {
            $(this).val('');
        });
        $('#employeeModal').modal('hide');
    }

    $scope.EditEmployee = function (SrNo) {
        for (i in $scope.employeeDetails) {
            if ($scope.employeeDetails[i].SrNo == SrNo) {
                $scope.EmployeeModel = $scope.employeeDetails[i];
                $('#employeeModal').modal('show');
                break;
            }
        }
    }

    $scope.removeEmployee = function (employeeId) {
        for (var i = $scope.employeeDetails.length - 1; i >= 0; --i) {
            if ($scope.employeeDetails[i].EmployeeId == employeeId) {
                $scope.employeeDetails.splice(i, 1);
            }
        }
        for (var i = 0; i < $scope.employeeDetails.length; i++) {
            $scope.employeeDetails[i].SrNo = i + 1;
        }
        if ($scope.WorkPermitModel.WPID && $scope.WorkPermitModel.WPID != "0") {
            var filter = { 'EmployeeId': employeeId };
            var response = myService.removeEmployee(filter);
            response.then(function (d) {
                if (d.data.isSuccessful) {
                    toaster.success("Work Permit", "Employee Removed Successfully.");
                }
            }, function () {
                toaster.error("Work Permit", d.data.message);
            });
        }
    }

    $scope.closeWorkPermit = function () {
        var filter = { 'WPID': $scope.WorkPermitModel.WPID };
        var response = myService.closeWorkPermit(filter);
        response.then(function (d) {
            if (d.data.isSuccessful) {
                toaster.success("Work Permit", "Work Permit Closed Successfully.");
                $scope.EditWorkPermit($scope.WorkPermitModel.WPID);
            }
        }, function () {
            toaster.error("Work Permit", d.data.message);
        });
    }

    $scope.EditWorkPermit = function (WPID) {
        var filter = { 'WPID': WPID, FilterText: '' };
        var response = myService.getAllWorkPermit(filter);
        response.then(function (response) {
            $scope.WorkPermitModel = response.data[0];
            $scope.employeeDetails = response.data[0].listEmployee;

            $scope.WorkPermitModel.WPDate = $scope.WorkPermitModel.strWPDate;
            $scope.WorkPermitModel.WorkStartDate = $scope.WorkPermitModel.strWorkStartDate;
            $scope.WorkPermitModel.WorkEndDate = $scope.WorkPermitModel.strWorkEndDate;
            //$scope.WorkPermitModel.TrainedDate = $scope.WorkPermitModel.strTrainedDate;

            $('#ddlWPType').selectpicker('val', $scope.WorkPermitModel.WPType);
            $('#ddlContractor').selectpicker('val', $scope.WorkPermitModel.ContractorId);
            //alert($scope.WorkPermitModel.HRId);
            $('#ddlHRApprover').selectpicker('val', $scope.WorkPermitModel.HRId);
            $('#ddlIMSApprover').selectpicker('val', $scope.WorkPermitModel.IMSId);
            $('#ddlmgrApprover').selectpicker('val', $scope.WorkPermitModel.FinalId);

            if ($scope.WorkPermitModel.HRApproval)
                $scope.WorkPermitModel.HRApproval = 1;
            else
                $scope.WorkPermitModel.HRApproval = 0;

            if ($scope.WorkPermitModel.IMSApproval)
                $scope.WorkPermitModel.IMSApproval = 1;
            else
                $scope.WorkPermitModel.IMSApproval = 0;

            if ($scope.WorkPermitModel.FinalApproval)
                $scope.WorkPermitModel.FinalApproval = 1;
            else
                $scope.WorkPermitModel.FinalApproval = 0;

            if ($scope.WorkPermitModel.SafetyTraining) {
                $scope.SafetyTraining.forEach(function (training) {
                    training.selected = false;
                    if ($scope.WorkPermitModel.SafetyTraining.includes(training.type)) {
                        training.selected = true;
                    }
                });
            }

            $scope.showTable = false;
            $scope.CheckApproval();

        }, function () {
            console.log("Error Occurs");
        });
    }
    $scope.SendReminder = function (WPID) {
        var filter = { 'WPID': WPID, FilterText: '' };
        var response = myService.SendReminder(filter);
        response.then(function (response) {
            toaster.success("Work Permit", "Reminder sent");

        }, function () {
            console.log("Error Occurs");
        });
    }


    $scope.UploadESICFile = function (files) {
        $scope.EmployeeModel.EmployeeModel = files;
        $scope.EmployeeModel.PFInsuranceFileName = files[0].name;
    }
    $scope.UploadPFFile = function (files) {
        $scope.EmployeeModel.ESICFile = files;
        $scope.EmployeeModel.ESICFileName = files[0].name;
    }
    $scope.AddNewEmployee = function () {
        $scope.EmployeeModel = [];
        $('input[type="file"]').each(function (index) {
            $(this).val('');
        });
    }

    $scope.SaveSafetyTraining = function () {
        $scope.getCheckedTrainings();

        $scope.WorkPermitModel.StatusNew = $scope.WPStatus.WaitingforIMSApproval;

        var response = myService.SaveSafetyTraining($scope.WorkPermitModel);

        response.then(function (d) {

            if ($scope.WorkPermitModel.IMSApproval == '1')
                status = $scope.WPStatus.Approved;
            else
                status = $scope.WPStatus.Rejected;

            var approveRejectDetails = {
                WPID: $scope.WorkPermitModel.WPID,
                Status: status,
                ApproveReject: $scope.WorkPermitModel.IMSApproval == "1" ? true : false,
                Comment: $scope.WorkPermitModel.IMSComment,
                Approver: 'IMS'
            }

            var response = myService.ApproveRejectWP(approveRejectDetails);

            response.then(function (res) {
                if (res.data.isSuccessful) {
                    if ($scope.WorkPermitModel.IMSApproval == '1') {
                        toaster.success("Work Permit", "Work Permit Approved Successfully.");
                        $scope.EditWorkPermit($scope.WorkPermitModel.WPID);
                    }
                    else
                        toaster.success("Work Permit", "Work Permit Rejected Successfully.");
                    $scope.EditWorkPermit($scope.WorkPermitModel.WPID);
                }

            }, function () {
                console.log('Error Occured');
            });
        }), function () {
            console.log('Error Occured');
        }
    }


    $scope.SaveWorkPermit = function (isSubmitted) {

        $scope.getCheckedTrainings();
        $scope.WorkPermitModel.listEmployee = $scope.employeeDetails;
        $scope.WorkPermitModel.WPType = $('#ddlWPType').val();
        $scope.WorkPermitModel.ContractorId = $('#ddlContractor').val();
        //$scope.WorkPermitModel.HRId = $('#ddlHRApprover').val();
        //$scope.WorkPermitModel.IMSId = $('#ddlIMSApprover').val();
        $scope.WorkPermitModel.FinalId = $('#ddlmgrApprover').val();
        $scope.WorkPermitModel.ContractorId = $('#ddlContractor').val();
        var selectedEmployees = '';

        if ($scope.employeeDetails) {
            $scope.WorkPermitModel.SelectedEmployees = $scope.employeeDetails.map(function (e) {
                return e.EmployeeId;
            }).join(',');
        }

        //if (selectedEmployees)
        //    $scope.WorkPermitModel.SelectedEmployees = selectedEmployees.join(',');

        if (isSubmitted) {
            $scope.WorkPermitModel.IsSubmitted = true;
            $scope.WorkPermitModel.StatusNew = $scope.WPStatus.WaitingforManagerApproval;
        }
        else {
            $scope.WorkPermitModel.IsSubmitted = false;
            $scope.WorkPermitModel.StatusNew = $scope.WPStatus.Open;
        }
        var response = myService.SaveWorkPermit($scope.WorkPermitModel);

        response.then(function (d) {

            if (d.data.isSuccessful) {
                //$scope.WorkPermitModel = null;
                //$scope.showTable = true;
                //$scope.getAllWorkPermit();

                //if (isSubmitted) {
                //    $scope.WorkPermitModel.Status = $scope.WPStatus.WaitingforHRAction;
                //}
                //else {
                //    $scope.WorkPermitModel.Status = $scope.WPStatus.Open;
                //}
                $scope.EditWorkPermit(d.data.PrimaryKeyID);
                toaster.success("Work Permit", "Record Saved Successfully.");
            }
            else {
                $scope.WorkPermitModel.IsSubmitted = false;
                toaster.error("Work Permit", d.data.message);
            }
        }), function () {
            console.log('Error Occured');
        }
    }

    $scope.ApproveRejectWorkPermit = function () {

        if ($scope.WorkPermitModel.Status == $scope.WPStatus.WaitingforHRAction) {

            var status;

            if ($scope.WorkPermitModel.HRApproval == '1')
                status = $scope.WPStatus.WaitingforSafetyTrainingEHSApproval;
            else
                status = $scope.WPStatus.RejectedByHR;

            var approveRejectDetails = {
                WPID: $scope.WorkPermitModel.WPID,
                Status: status,
                ApproveReject: $scope.WorkPermitModel.HRApproval == "1" ? true : false,
                Comment: $scope.WorkPermitModel.HRComment,
                Approver: 'HR'
            }

            var response = myService.ApproveRejectWP(approveRejectDetails);

            response.then(function (res) {
                if (res.data.isSuccessful) {
                    if ($scope.WorkPermitModel.HRApproval == '1') {
                        toaster.success("Work Permit", "Work Permit Approved Successfully.");
                        $scope.EditWorkPermit($scope.WorkPermitModel.WPID);
                    }
                    else
                        toaster.success("Work Permit", "Work Permit Rejected Successfully.");
                    $scope.EditWorkPermit($scope.WorkPermitModel.WPID);
                }

            }, function () {
                console.log('Error Occured');
            });


        } else if ($scope.WorkPermitModel.Status == $scope.WPStatus.WaitingforSafetyTrainingEHSApproval) {

            var status;
            $scope.SaveSafetyTraining()

        } else if ($scope.WorkPermitModel.Status == $scope.WPStatus.WaitingforManagerApproval) {

            var status;

            if ($scope.WorkPermitModel.FinalApproval == '1')
                status = $scope.WPStatus.WaitingforHRAction;
            else
                status = $scope.WPStatus.RejectedByManager;

            var approveRejectDetails = {
                WPID: $scope.WorkPermitModel.WPID,
                Status: status,
                ApproveReject: $scope.WorkPermitModel.FinalApproval == "1" ? true : false,
                Comment: $scope.WorkPermitModel.FinalComment,
                Approver: 'Manager'
            }

            var response = myService.ApproveRejectWP(approveRejectDetails);

            response.then(function (res) {
                if (res.data.isSuccessful) {
                    if ($scope.WorkPermitModel.FinalApproval == '1') {
                        toaster.success("Work Permit", "Work Permit Approved Successfully.");
                        $scope.EditWorkPermit($scope.WorkPermitModel.WPID);
                    }
                    else
                        toaster.success("Work Permit", "Work Permit Rejected Successfully.");
                    $scope.EditWorkPermit($scope.WorkPermitModel.WPID);
                }

            }, function () {
                console.log('Error Occured');
            });
        }

    }

    $scope.IsAccessable = function (claim) {

        var strClaims = localStorage.getItem('Claims');
        var claims = strClaims.split(',');

        if (claims.includes(claim))
            return true;
        else
            return false;
    }
    $scope.IsAccessableSendReminder = function (CreatedBy, Status) {
        debugger;
        var UserID = localStorage.getItem("UserID");
        if (CreatedBy == UserID && Status != "Approved" && Status != "Closed" && Status.indexOf("Rejected") == -1)
            return true;
        else
            return false;
    }



    $scope.AddEmployees = function () {
        var selectedEmployees = $('#ddlEmployee').val();

        if (selectedEmployees == null)
            return;

        if (selectedEmployees)
            selectedEmployees = selectedEmployees.join(',');

        var oldEmployees = $scope.employeeDetails.map(function (e) {
            return e.EmployeeId;
        }).join(',');

        if (oldEmployees)
            selectedEmployees = selectedEmployees + "," + oldEmployees;

        var filter = { 'ContractorId': $('#ddlContractor').val(), 'EmployeeId': selectedEmployees, FilterText: '' };
        var response = myService.GetAllEmployee(filter);

        response.then(function (res) {
            $scope.employeeDetails = res.data;
            $('#ddlEmployee').selectpicker('val', '');
        }, function (error) {
            console.log(error);
        })
    }

    $scope.getAllWorkPermit = function () {

        var filter = { 'WPID': 0, FilterText: '' };
        var response = myService.getAllWorkPermit(filter);

        response.then(function (res) {
            //-----Bind Data Table----------
            $scope.data = res.data;
            $scope.searchFilter = '';
            $scope.TotalRecords = $scope.data.length;
            $scope.showTable = true;

            $scope.tableParams = new NgTableParams({
                page: 1,
                count: 25,
                total: $scope.data.length,
                getData: function ($defer, params) {
                    $defer.resolve($scope.data.slice((params.page() - 1) * params.count(), params.page() * params.count())
                    );
                }
            }, {
                dataset: res.data
            });

            $timeout(function () {
                freezeTable.update();
            }, 1500);

            //-------Bind Data Table---------------
        }, function () {
            console.log('Error Occured');
        })
    }
    $scope.getAllWorkPermit();
});

app.controller("CtrlNavBar", function ($scope, $http, myService, $timeout, Upload, NgTableParams, $window, constants, toaster) {

    $scope.isSuperAdmin = false;

    if (localStorage.getItem('RoleName') == 'Super Admin') {
        $scope.isSuperAdmin = true;
    }

    $scope.IsAccessable = function (claim) {

        var strClaims = localStorage.getItem('Claims');
        var claims = strClaims.split(',');

        if (claims.includes(claim))
            return true;
        else
            return false;
    }
});

app.controller("DashboardCtrl", function ($scope, $http, myService, $timeout, Upload, NgTableParams, $window, constants, toaster) {
    $scope.todayData = [];
    $scope.weeklyDataLables = [];
    $scope.weeklyDataOpenValues = [];
    $scope.weeklyDataCheckInValues = [];
    $scope.weeklyDataCheckOutValues = [];
    $scope.weeklyDataCancelledValues = [];
    $scope.weeklyDataRejectedValues = [];

    $scope.monthlyDataLables = [];
    $scope.monthlyDataOpenValues = [];
    $scope.monthlyDataCheckInValues = [];
    $scope.monthlyDataCheckOutValues = [];
    $scope.monthlyDataCancelledValues = [];
    $scope.monthlyDataRejectedValues = [];

    $scope.dailyDataValues = [];

    var dailyChart;
    var weeklyChart;
    var monthlyChart;

    $scope.IsAccessable = function (claim) {
        var strClaims = localStorage.getItem('Claims');
        console.log(strClaims);
        var claims = strClaims.split(',');

        if (claims.includes(claim))
            return true;
        else
            return false;
    }


    $scope.generateTodayChart = function () {
        var filter = { type: 'Daily' };
        var response = myService.GenerateChart(filter);
        var dailyData = [];
        response.then(function (res) {

            if (res.data.isSuccessful) {


                for (var i = 0; i < res.data.data.length; i++) {
                    dailyData.push({ name: res.data.data[i].Date, y: res.data.data[i].OpenCnt });

                }

                //$scope.dailyDataValues = res.data.data;

                // Radialize the colors
                Highcharts.setOptions({
                    colors: Highcharts.map(Highcharts.getOptions().colors, function (color) {
                        return {
                            radialGradient: {
                                cx: 0.5,
                                cy: 0.3,
                                r: 0.7
                            },
                            stops: [
                                [0, color],
                                [1, Highcharts.color(color).brighten(-0.3).get('rgb')] // darken
                            ]
                        };
                    })
                });

                // Build the chart
                dailyChart = Highcharts.chart('divToday', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'Today'
                    },
                    credits: {
                        enabled: false
                    },
                    exporting: {
                        buttons: {
                            contextButton: {
                                menuItems: ['downloadPDF'],
                            },
                        },
                    },
                    //tooltip: {
                    //    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                    //},
                    accessibility: {
                        //point: {
                        //    valueSuffix: '%'
                        //}
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.y}',
                                connectorColor: 'silver'
                            }
                        }
                    },
                    series: [{
                        name: '',
                        data: dailyData
                    }]
                });

            }
        });
    }
    $scope.generateTodayChart();

    $scope.generateWeeklyChart = function () {
        var filter = { type: 'Weekly' };
        var response = myService.GenerateChart(filter);

        response.then(function (res) {
            if (res.data.isSuccessful) {
                for (var i = 0; i < res.data.data.length; i++) {
                    $scope.weeklyDataLables.push(res.data.data[i].Date);
                    $scope.weeklyDataOpenValues.push(res.data.data[i].OpenCnt);
                    $scope.weeklyDataCheckInValues.push(res.data.data[i].CheckedInCnt);
                    $scope.weeklyDataCheckOutValues.push(res.data.data[i].CheckedOutCnt);
                    $scope.weeklyDataCancelledValues.push(res.data.data[i].CancelledCnt);
                    $scope.weeklyDataRejectedValues.push(res.data.data[i].RejectedCnt);
                }
                weeklyChart = Highcharts.chart('divWeekly', {
                    chart: {
                        zoomType: 'xy'
                    },
                    title: {
                        text: 'Weekly',
                        style: {
                            fontSize: '20px'
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    exporting: {
                        buttons: {
                            contextButton: {
                                menuItems: ['downloadPDF'],
                            },
                        },
                    },
                    //subtitle: {
                    //    text: 'Source: WorldClimate.com'
                    //},
                    xAxis: [{
                        categories: $scope.weeklyDataLables,
                        crosshair: true
                    }],
                    yAxis: [{
                        id: "y_axis_0",
                        title: {
                            text: '',
                        }
                    }, {
                        title: {
                            text: ''
                        },
                        opposite: true
                    }],
                    tooltip: {
                        shared: true
                    },
                    legend: {
                        layout: 'horizontal',
                        align: 'left',
                        //x: 120,
                        verticalAlign: 'top',
                        y: 15,
                        floating: true,
                        backgroundColor:
                            Highcharts.defaultOptions.legend.backgroundColor || // theme
                            'rgba(255,255,255,0.25)'
                    },
                    plotOptions: {
                        series: {
                            events: {
                                legendItemClick: function (e) {
                                    e.preventDefault();
                                }
                            },
                            states: {
                                inactive: {
                                    opacity: 1
                                }
                            }
                        },
                        column: {
                            dataLabels: {
                                enabled: true,
                                crop: false,
                                overflow: 'none',
                                formatter: function () {
                                    if (this.y > 0) {
                                        return this.y;
                                    }
                                }
                            }
                        }
                    },
                    series: [{
                        color: "#66ff66",
                        name: 'Open',
                        type: 'column',
                        data: $scope.weeklyDataOpenValues,
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    }, {
                        color: "#3366ff",
                        name: 'Checked In',
                        type: 'column',
                        data: $scope.weeklyDataCheckInValues,
                        yAxis: "y_axis_0",
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    }, {
                        color: "#ff6600",
                        name: 'Checked Out',
                        type: 'column',
                        data: $scope.weeklyDataCheckOutValues,
                        yAxis: "y_axis_0",
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    }, {
                        color: "#ff0000",
                        name: 'Cancelled',
                        type: 'column',
                        data: $scope.weeklyDataCancelledValues,
                        yAxis: "y_axis_0",
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    }, {
                        color: "#f30909",
                        name: 'Rejected',
                        type: 'column',
                        data: $scope.weeklyDataRejectedValues,
                        yAxis: "y_axis_0",
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    }

                    ]
                });
            }
        });




    }
    $scope.generateWeeklyChart();

    $scope.generateMonthlyChart = function () {
        var filter = { type: 'Monthly' };
        var response = myService.GenerateChart(filter);
        console.log('Monthly');
        response.then(function (res) {
            if (res.data.isSuccessful) {
                for (var i = 0; i < res.data.data.length; i++) {
                    $scope.monthlyDataLables.push(res.data.data[i].Date.substring(0, 2));
                    $scope.monthlyDataOpenValues.push(res.data.data[i].OpenCnt);
                    $scope.monthlyDataCheckInValues.push(res.data.data[i].CheckedInCnt);
                    $scope.monthlyDataCheckOutValues.push(res.data.data[i].CheckedOutCnt);
                    $scope.monthlyDataCancelledValues.push(res.data.data[i].CancelledCnt);
                    $scope.monthlyDataRejectedValues.push(res.data.data[i].RejectedCnt);
                }
                monthlyChart = Highcharts.chart('divMonthly', {
                    chart: {
                        zoomType: 'xy'
                    },
                    title: {
                        text: 'Monthly',
                        style: {
                            fontSize: '20px'
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    exporting: {
                        buttons: {
                            contextButton: {
                                menuItems: ['downloadPDF'],
                            },
                        },
                    },
                    //subtitle: {
                    //    text: 'Source: WorldClimate.com'
                    //},
                    xAxis: [{
                        categories: $scope.monthlyDataLables,
                        crosshair: true
                    }],
                    yAxis: [{
                        id: "y_axis_0",
                        title: {
                            text: '',
                        }
                    }, {
                        title: {
                            text: ''
                        },
                        opposite: true
                    }],
                    tooltip: {
                        shared: true
                    },
                    legend: {
                        layout: 'horizontal',
                        align: 'left',
                        //x: 120,
                        verticalAlign: 'top',
                        y: 15,
                        floating: true,
                        backgroundColor:
                            Highcharts.defaultOptions.legend.backgroundColor || // theme
                            'rgba(255,255,255,0.25)'
                    },
                    plotOptions: {
                        series: {
                            events: {
                                legendItemClick: function (e) {
                                    e.preventDefault();
                                }
                            },
                            states: {
                                inactive: {
                                    opacity: 1
                                }
                            }
                        },
                        column: {
                            dataLabels: {
                                enabled: true,
                                crop: false,
                                overflow: 'none',
                                formatter: function () {
                                    if (this.y > 0) {
                                        return this.y;
                                    }
                                }
                            }
                        }
                    },
                    series: [{
                        color: "#66ff66",
                        name: 'Open',
                        type: 'column',
                        data: $scope.monthlyDataOpenValues,
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    }, {
                        color: "#3366ff",
                        name: 'Checked In',
                        type: 'column',
                        data: $scope.monthlyDataCheckInValues,
                        yAxis: "y_axis_0",
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    }, {
                        color: "#ff6600",
                        name: 'Checked Out',
                        type: 'column',
                        data: $scope.monthlyDataCheckOutValues,
                        yAxis: "y_axis_0",
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    }, {
                        color: "#ff0000",
                        name: 'Cancelled',
                        type: 'column',
                        data: $scope.monthlyDataCancelledValues,
                        yAxis: "y_axis_0",
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    }, {
                        color: "#f30909",
                        name: 'Rejected',
                        type: 'column',
                        data: $scope.monthlyDataRejectedValues,
                        yAxis: "y_axis_0",
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    }

                    ]
                });
            }
        });
    }
    $scope.generateMonthlyChart();

    //setInterval(function () {
    //    dailyChart.destroy();
    //    $scope.generateTodayChart();
    //    $scope.generateWeeklyChart();
    //    $scope.generateMonthlyChart();
    //}, 60000) //120000
});

app.controller("UserMgmtCtrl", function ($scope, $http, myService, $timeout, Upload, NgTableParams, $window, constants, toaster) {

    $scope.showTable = true;
    $scope.UserModel = {};
    $scope.UserRoles = [{ text: 'Super Admin', value: 'Super Admin' }, { text: 'Employee', value: 'Employee' }, { text: 'Security', value: 'Security' }];

    $scope.AddNewRecord = function () {
        $scope.UserModel = {};
        $('#ddlRole').selectpicker('val', '').trigger("change");
        $scope.GetDefaultClaims();
    }

    $scope.GetDefaultClaims = function () {
        var response = myService.GetDefaultClains();

        response.then(function (res) {
            $scope.userAccessList = res.data;
        });
    }

    $scope.PreventUncheck = function (row) {
        if (row.CanCreate || row.CanUpdate) {
            row.CanRead = true;
        }
    }

    $scope.CheckDefaultReadOption = function (row, colName) {

        if (colName == 'Create') {
            if (row.CanCreate) {
                row.CanRead = true;
            }
        }
        else if (colName == 'Update') {
            if (row.CanUpdate) {
                row.CanRead = true;
            }
        }
        else if (colName == 'Disable') {
            if (row.Disable) {
                row.Disable = true;
            }
        }
    }

    $scope.$watch(function () {
        $('#ddlRole').selectpicker('refresh');
    });


    $scope.SaveUser = function () {
        $scope.UserModel.Role = $('#ddlRole').val();
        $scope.UserModel.UserClaim = $scope.userAccessList;

        if ($("#chkisdisable").is(":checked") == true) {
            $scope.UserModel.Disable = true;
        }
        else {
            $scope.UserModel.Disable = false;
        }

        var response = myService.SaveUser($scope.UserModel); // get call from service.js
        response.then(function (d) {

            if (d.data.isSuccessful) {
                $scope.UserModel = null;
                $scope.showTable = true;
                $scope.getAllUsers();
                $('#exampleModal').modal('hide');
                toaster.success("Visitor's Management", "Record Saved Successfully.");
            }
            else {
                toaster.error("Visitor's Management", d.data.message);
            }

        }), function () {
            toaster.success("Visitor's Management", "Error Occured.");
        }


    }

    $scope.getAllUsers = function () {

        var filter = { 'UserId': 0, FilterText: '' };
        var response = myService.GetUsers(filter);

        response.then(function (res) {
            //-----Bind Data Table----------
            $scope.data = res.data;
            $scope.searchFilter = '';
            $scope.TotalRecords = $scope.data.length;
            $scope.showTable = true;

            $scope.tableParams = new NgTableParams({
                page: 1,
                count: 25,
                total: $scope.data.length,
                getData: function ($defer, params) {
                    $defer.resolve($scope.data.slice((params.page() - 1) * params.count(), params.page() * params.count())
                    );
                }
            }, { dataset: res.data });
            //-------Bind Data Table---------------
        }, function () {
            console.log('Error Occured');
        })
    }
    $scope.getAllUsers();

    $scope.EditDetails = function (Id) {
        //$scope.showTable = false;
        //$window.scrollTo(0, 0);
        var filter = { 'UserId': Id, FilterText: '' };
        var response = myService.GetUsers(filter);
        response.then(function (response) {

            $scope.UserModel = response.data[0];
            if ($scope.UserModel.Disable == true) {
                $("#chkisdisable").prop("checked", true)
            }
            else {
                $("#chkisdisable").prop("checked", false)
            }
            //$scope.AppointmentModel.Date = $scope.AppointmentModel.strDate;
            //$('#ddlPrepared').selectpicker('val', $scope.AppointmentModel.PersonToVisitID).trigger("change");
            $('#exampleModal').modal('show');

            $('#ddlRole').selectpicker('val', $scope.UserModel.Role).trigger("change");
            $scope.userAccessList = $scope.UserModel.UserClaim;

        }, function () {
            console.log("Error Occurs");
        });
    }
});

app.controller("ctrlChangePass", function ($scope, $http, myService, $timeout, Upload, NgTableParams, $window, constants, toaster) {

    $scope.UserModel = {
        newpassword: "",
        confirmPassword: "",
        oldpassword: ""
    };

    $scope.Compare = function () {
        if ($scope.UserModel.newpassword != $scope.UserModel.confirmPassword) {
            $scope.IsVisible = true;
        } else {
            $scope.IsVisible = false;
        }
    }

    $scope.ChangePassword = function () {

        if ($scope.UserModel.newpassword == '' || $scope.UserModel.newpassword == undefined || $scope.UserModel.oldpassword == '' || $scope.UserModel.oldpassword == undefined) {
            return;
        }

        var specialCharacters = ['%', '$', '#', '@', '!'];
        if ($scope.UserModel.newpassword.match(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z]{8,}$/)) {
            var userModel = { 'NewPassword': $scope.UserModel.newpassword, 'OldPassword': $scope.UserModel.oldpassword }

            var getData = myService.ChangePassword(userModel);
            getData.then(function (response) {

                if (response.data.isSuccessful) {
                    $scope.UserModel = null;
                    toaster.success("Visitor's Management", response.data.message);
                }
                else {
                    toaster.error("Visitor's Management", response.data.message);
                }
            });
        }
        else {
            alert('Password must contain minumum 8 characters, One Upper Case character, One Lower Case character & One Digit. For Ex. Password123');
        }


    }
});

app.controller("ctrlCheckIn", function ($scope, $http, myService, $timeout, Upload, NgTableParams, $window, constants, toaster) {

    $scope.StatusMedia = constants.VMStatus;

    $scope.EditDetails = function (Id) {
        var filter = { 'AppointmentId': Id, FilterText: '' };
        var response = myService.VM_getAllAppointment(filter);
        response.then(function (response) {
            $scope.CheckinModel = response.data[0];
            $scope.CheckinModel.Date = $scope.CheckinModel.strDate;
        }, function () {
            console.log("Error Occurs");
        });
    }

    var value = $("#appointmentId").val();
    if (value) {
        $scope.EditDetails(value);
    };
    localStorage.setItem('previosPage', 'CheckIn');
    localStorage.setItem('appointmentId', value);
    $scope.checkInAppointment = function () {
        $scope.CheckinModel.Status = constants.VMStatus.CheckIn;
        var time = new Date();
        $scope.CheckinModel.Time = time.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
        var response = myService.VM_CheckIn($scope.CheckinModel); // get call from service.js
        response.then(function (d) {
            if (d.data.isSuccessful) {
                $scope.EditDetails(value);
                toaster.success("Visitor's Management", "Visitor Checked In Successfully.");
            }
            else {
                toaster.error("Visitor's Management", d.data.message);
            }

        }), function () {
            toaster.success("Visitor's Management", "Error Occured.");
        }
    }

    $scope.checkOutAppointment = function () {
        $scope.CheckinModel.Status = constants.VMStatus.CheckOut;
        var time = new Date();
        $scope.CheckinModel.Time = time.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
        var response = myService.VM_CheckOut($scope.CheckinModel); // get call from service.js
        response.then(function (d) {
            if (d.data.isSuccessful) {
                $scope.EditDetails(value);
                toaster.success("Visitor's Management", "Visitor Checked Out Successfully.");
            }
            else {
                toaster.error("Visitor's Management", d.data.message);
            }

        }), function () {
            toaster.success("Visitor's Management", "Error Occured.");
        }
    }
});

app.controller("ContractorCtrl", function ($scope, $http, myService, $timeout, Upload, NgTableParams, $window, constants, toaster) {

    $scope.isSuperAdmin = false;
    $scope.showEployeeSection = false;
    if (localStorage.getItem('RoleName') == 'Super Admin') {
        $scope.isSuperAdmin = true;
    }

    $scope.showTable = true;
    $scope.ContractorModel = {};
    $scope.EmployeeModel = {};
    $scope.UserRoles = [{ text: 'Super Admin', value: 'Super Admin' }, { text: 'Employee', value: 'Employee' }, { text: 'Security', value: 'Security' }];
    $scope.employeeDetails = [];
    $scope.EmployeeDetailsFileForUpload = [];


    $scope.IsAccessable = function (claim) {

        var strClaims = localStorage.getItem('Claims');
        var claims = strClaims.split(',');

        if (claims.includes(claim))
            return true;
        else
            return false;
    }


    $scope.AddNewRecord = function () {
        $scope.ContractorModel = {};
        $scope.showTable = false;
        $scope.showEployeeSection = false;
        $scope.employeeDetails = [];
    }

    $scope.CloseContractor = function () {
        $scope.getAllContractor();
    }

    $scope.$watch(function () {
        $('#ddlRole').selectpicker('refresh');
    });


    $scope.SaveContractor = function () {
        //$scope.UserModel.UserClaim = $scope.userAccessList

        var response = myService.SaveContractor($scope.ContractorModel); // get call from service.js
        response.then(function (d) {

            if (d.data.isSuccessful) {
                $scope.showEployeeSection = true;
                $scope.ContractorModel.ContractorId = d.data.PrimaryKeyID;
                //$scope.showTable = true;
                //$scope.getAllContractor();
                //$('#contractorModal').modal('hide');
                toaster.success("Contractor Management", "Record Saved Successfully.");
            }
            else {
                toaster.error("Contractor Management", d.data.message);
            }

        }), function () {
            toaster.success("Contractor Management", "Error Occured.");
        }


    }

    $scope.getAllContractor = function () {

        var filter = { 'ContractorId': 0, FilterText: '' };
        var response = myService.GetAllContractor(filter);

        response.then(function (res) {
            //-----Bind Data Table----------
            $scope.data = res.data;
            $scope.searchFilter = '';
            $scope.TotalRecords = $scope.data.length;
            $scope.showTable = true;

            $scope.tableParams = new NgTableParams({
                page: 1,
                count: 25,
                total: $scope.data.length,
                getData: function ($defer, params) {
                    $defer.resolve($scope.data.slice((params.page() - 1) * params.count(), params.page() * params.count())
                    );
                }
            }, { dataset: res.data });

            $timeout(function () {
                freezeTable.update();
            }, 1000);
            //-------Bind Data Table---------------
        }, function () {
            console.log('Error Occured');
        })
    }
    $scope.getAllContractor();

    $scope.getAllEmployee = function (Id) {
        var filter = { 'ContractorId': Id, FilterText: '' };
        var response = myService.GetAllEmployee(filter);

        response.then(function (res) {
            //-----Bind Data Table----------
            //$scope.dataEmplopyee = res.data;
            //$scope.searchFilter = '';
            //$scope.TotalRecords = $scope.dataEmplopyee.length;
            //$scope.showTable = true;
            $scope.employeeDetails = res.data;

            //$scope.tableParamsEmployee = new NgTableParams({
            //    page: 1,
            //    count: 25,
            //    total: res.data.length,
            //    getData: function ($defer, params) {
            //        $defer.resolve(res.data.slice((params.page() - 1) * params.count(), params.page() * params.count())
            //        );
            //    }
            //}, { dataset: res.data });
            //-------Bind Data Table---------------ContractorCtrl
        }, function (error) {
            console.log(error);
        })
    }

    $scope.EditDetails = function (Id) {
        //$scope.showTable = false;
        //$window.scrollTo(0, 0);
        var filter = { 'ContractorId': Id, FilterText: '' };
        var response = myService.GetAllContractor(filter);
        response.then(function (response) {
            $scope.ContractorModel = response.data[0];
            $scope.showTable = false;
            $scope.getAllEmployee(Id);
            $scope.showEployeeSection = true;
            //$('#ddlRole').selectpicker('val', $scope.ContractorModel.Role).trigger("change");
            //$scope.userAccessList = $scope.ContractorModel.UserClaim;

        }, function () {
            console.log("Error Occurs");
        });
    }

    $scope.UploadPFFile = function (files) {
        if (files[0].type != 'application/pdf') {
            toaster.error("Contractor Management", 'Please select pdf file only.');
            $('#sourcePF').val('');
            return;
        }
        $scope.EmployeeModel.PFInsuranceFile = files;
        $scope.EmployeeModel.PFInsuranceFileName = files[0].name;
    }
    $scope.UploadESICFile = function (files) {
        if (files[0].type != 'application/pdf') {
            toaster.error("Contractor Management", 'Please select pdf file only.');
            $('#sourceESIC').val('');
            return;
        }
        $scope.EmployeeModel.ESICFile = files;
        $scope.EmployeeModel.ESICFileName = files[0].name;
    }
    $scope.AddNewEmployee = function () {
        $scope.EmployeeModel = [];
        $('input[type="file"]').each(function (index) {
            $(this).val('');
        });
    }

    $scope.EditEmployeeDetails = function (Id) {
        var filter = { 'EmployeeId': Id, ContractorId: $scope.ContractorModel.ContractorId, FilterText: '' };
        var response = myService.GetAllEmployee(filter);
        response.then(function (response) {
            $scope.EmployeeModel = response.data[0];
            $('#employeeModal').modal('show');
        }, function () {
            console.log("Error Occurs");
        });
    }

    $scope.SaveEmployee = function () {

        $scope.EmployeeModel.ContractorId = $scope.ContractorModel.ContractorId;


        if ($scope.EmployeeModel.ESICFile || $scope.EmployeeModel.PFInsuranceFile) {
            Upload.upload({
                method: 'POST',
                url: '../Contractor/CreateEmployee',
                data: { employee: $scope.EmployeeModel }
            }).then(function (d) {

                if (d.data.isSuccessful) {
                    $scope.EmployeeModel = null;
                    $('#employeeModal').modal('hide');
                    $scope.getAllEmployee($scope.ContractorModel.ContractorId);
                    toaster.success("Contractor Management", "Record Saved Successfully.");
                }
                else {
                    toaster.error("Contractor Management", d.data.message);
                }
            }), function () {
                console.log('Error Occured');
            }
        }
        else {
            $http({
                method: 'POST',
                url: '../Contractor/CreateEmployee',
                data: { employee: $scope.EmployeeModel },
                dataType: "json"
            }).then(function (d) {

                if (d.data.isSuccessful) {
                    $scope.EmployeeModel = null;
                    $('#employeeModal').modal('hide');
                    $scope.getAllEmployee($scope.ContractorModel.ContractorId);
                    toaster.success("Contractor Management", "Record Saved Successfully.");
                }
                else {
                    toaster.error("Contractor Management", d.data.message);
                }
            }), function () {
                console.log('Error Occured');
            };
        }
    }

    $scope.getEmployeeFile = function (employeeId, type) {
        var filter = { EmployeeId: employeeId, ContractorId: $scope.ContractorModel.ContractorId, fileType: type };
        var response = myService.getEmployeeFile(filter);
        response.then(function (response) {
            if (response.data.isSuccessful) {
                $('#iframeModal').modal('show');
                document.getElementById('iframeShowFile').src = response.data.data;
            }
            else {
                toaster.error("Visitor's Management", "Error Occured");
            }
        }, function () {
            console.log("Error Occurs");
        });
    }

    $scope.ParseExcelDataAndSaveEmployeeDetails = function () {
        var file = $scope.EmployeeDetailsFileForUpload;
        $('#fileUpload').val('');
        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var data = e.target.result;
                //XLSX from js-xlsx library , which I will add in page view page
                var workbook = XLSX.read(data, {
                    type: 'binary'
                });
                var sheetName = workbook.SheetNames[0];
                var excelData = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
                if (excelData.length > 0) {
                    //Save data 
                    var response;
                    response = myService.UploadEmployeeDetails(excelData, $scope.ContractorModel.ContractorId);

                    response.then(function (data) {

                        if (data.data.isSuccessful) {
                            toaster.success("Contractor Management", "Employee Details Uploaded Successfully.");
                            $('#fileUpload').val('');
                            $scope.EmployeeDetailsFileForUpload = '';
                            $scope.getAllEmployee($scope.ContractorModel.ContractorId);
                        }
                        else {
                            toaster.error("Contractor Management", data.data.message);
                        }


                    }, function (error) {
                        $scope.Message = "Error";
                    })
                }
                else {
                    $scope.Message = "No data found";
                }
            }
            reader.onerror = function (ex) {
                console.log(ex);
            }

            reader.readAsBinaryString(file);
        }
    }

    $scope.UploadFile = function (files) {
        $scope.$apply(function () {
            $scope.EmployeeDetailsFileForUpload = files[0];

        })
    }

});

app.controller("CtrlReports", function ($scope, $http, myService, $timeout, Upload, NgTableParams, $window, constants, toaster) {

    $scope.ReportModel = {};
    $scope.isAllowedToShow = false;

    if (localStorage.getItem("RoleName") == "Security" || localStorage.getItem("RoleName") == "Super Admin") {
        $scope.isAllowedToShow = true;
    }

    $scope.getUserDropdown = function () {
        var response = myService.GetUserSelectListOnlyEmployee();

        response.then(function (res) {
            $scope.employeeMaster = res.data;
        }, function () {
            console.log('Error Occured');
        })
    }

    $scope.getUserDropdown();

    $scope.$watch(function () {
        $('#ddlEmployee').selectpicker('refresh');
    });


    $scope.downloadReport = function () {
        if ($scope.ReportModel.FromDate == '' || $scope.ReportModel.FromDate == undefined || $scope.ReportModel.ToDate == '' || $scope.ReportModel.ToDate == undefined) {
            toaster.error("Visitor's Management", "Please select the date range.");
        }

        if ($scope.isAllowedToShow) {
            $scope.ReportModel.EmployeeId = $('#ddlEmployee').val();
        }
        else {
            $scope.ReportModel.EmployeeId = localStorage.getItem("UserID");
        }
        var fromDate = new Date($scope.ReportModel.FromDate).toShortFormat();
        var toDate = new Date($scope.ReportModel.ToDate).toShortFormat();
        var FileName = "VisitorsReport_" + fromDate + '_To_' + toDate;
        $scope.ReportModel.FileName = FileName;
        var response = myService.downloadVMReport($scope.ReportModel);

        response.then(function (res) {
            window.open("/DownloadReports/" + FileName + ".CSV?" + +(new Date()).toString(), "_self");
        }, function () {
            console.log('Error Occured');
        })
    }
});

app.controller("RemoteEmployeeCtrl", function ($scope, $http, myService, $timeout, Upload, NgTableParams, $window, constants, toaster) {
    $scope.showTable = true;
    $scope.RemoteEmployeeModel = {};
    $scope.HcodeAutoCompleteData = [];
    $scope.DisableCOntrolOnEdit = false;
    $scope.DisableCOntrolOnEditAdmin = false;
    $scope.RemoteEmployeeSecurityCheck = {};
    $scope.AccessCardIssueList = [{ text: 'Yes', value: 'Yes' }, { text: 'No', value: 'No' }];
    $scope.AccessCardCollectedList = [{ text: 'Yes', value: 'Yes' }, { text: 'No', value: 'No' }];

    $scope.getAllRemoteEmployee = function () {

        var filter = { 'Hcode': '', FilterText: '' };
        var response = myService.getAllRemoteEmployee(filter);

        response.then(function (res) {
            console.log(res);
            //-----Bind Data Table----------
            $scope.data = res.data;
            $scope.searchFilter = '';
            $scope.TotalRecords = $scope.data.length;
            $scope.showTable = true;

            $scope.tableParams = new NgTableParams({
                page: 1,
                count: 25,
                total: $scope.data.length,
                getData: function ($defer, params) {
                    $defer.resolve($scope.data.slice((params.page() - 1) * params.count(), params.page() * params.count())
                    );
                }
            }, {
                dataset: res.data
            });

            $timeout(function () {
                freezeTable.update();
            }, 1500);

            //-------Bind Data Table---------------
        }, function () {
            console.log('Error Occured');
        })
    }

    $scope.clearForm = function () {
        $scope.RemoteEmployeeModel = {};
        $scope.RemoteEmployeeModel.Pkey = "";
        $scope.RemoteEmployeeModel.Hcode = "";
        $scope.RemoteEmployeeModel.Name = "";
        $scope.RemoteEmployeeModel.EmailID = "";
        $scope.RemoteEmployeeModel.CheckOutDateTime = "";
        $scope.RemoteEmployeeModel.CheckinDateTime = "";
        $scope.RemoteEmployeeModel.IsVehicalParkedOnPremises = "";
        $scope.RemoteEmployeeModel.VehicalNumber = "";
        $scope.RemoteEmployeeModel.Comments = "";
        $("#Hcode_value").val("");
        $("#Hcode_value").prop("disabled", false);
        $("#Pkey").val("");
        $("#IsVehicalParkedOnPremises").prop("checked", false);
        $("#BookForWeekend").prop("checked", false);
    }
    $scope.clearFormSecurityCheck = function () {
        $scope.RemoteEmployeeSecurityCheck = {};
        $scope.RemoteEmployeeSecurityCheck.Pkey = "";
        $scope.RemoteEmployeeSecurityCheck.Hcode = "";
        $scope.RemoteEmployeeSecurityCheck.Name = "";
        $scope.RemoteEmployeeSecurityCheck.EmailID = "";
        $scope.RemoteEmployeeSecurityCheck.CheckOutDateTime = "";
        $scope.RemoteEmployeeSecurityCheck.CheckinDateTime = "";
        $scope.RemoteEmployeeSecurityCheck.IsVehicalParkedOnPremises = "";
        $scope.RemoteEmployeeSecurityCheck.VehicalNumber = "";
        $scope.RemoteEmployeeSecurityCheck.Comments = "";
        $("#Hcode_value").val("");
        $("#Hcode_value").prop("disabled", false);
        $("#Pkey").val("");
        $("#IsVehicalParkedOnPremises").prop("checked", false);
    }
    $scope.AddNewRecord = function () {
        $scope.DisableCOntrolOnEdit = false;
        $scope.clearForm();
        $scope.RemoteEmployeeSecurityCheck = {};
        $scope.RemoteEmployeeModel =
        $scope.RemoteEmployeeModel.Status = "Open";
        $('#exampleModal').modal('show');
        $("#DvRENumber").css("display", "none");
        
        

        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var DateTimeNow = new Date();
        var day = DateTimeNow.getDate();
        var month = monthNames[DateTimeNow.getMonth()];
        var year = DateTimeNow.getFullYear();
        var hours = DateTimeNow.getHours();
        var minutes = DateTimeNow.getMinutes();
        
        var formattedDay = day < 10 ? '0' + day : day;
        var formattedHours = hours < 10 ? '0' + hours : hours;
        var formattedMinutes = minutes < 10 ? '0' + minutes : minutes;
        var FinlDate = formattedDay + "-" + month + "-" + year + " " + formattedHours + ":" + formattedMinutes;
        $("#CheckinDateTime").datepicker("setDate", FinlDate);
        $("#CheckinDateTime").val(FinlDate);

        DateTimeNow.setHours(DateTimeNow.getHours() + 2);
        day = DateTimeNow.getDate();
        month = monthNames[DateTimeNow.getMonth()];
        year = DateTimeNow.getFullYear();
        hours = DateTimeNow.getHours();
        minutes = DateTimeNow.getMinutes();

        formattedDay = day < 10 ? '0' + day : day;
        formattedHours = hours < 10 ? '0' + hours : hours;
        formattedMinutes = minutes < 10 ? '0' + minutes : minutes;
        var FinlEndDate = formattedDay + "-" + month + "-" + year + " " + formattedHours + ":" + formattedMinutes;


        $("#CheckOutDateTime").datepicker("setDate", FinlEndDate);
        $("#CheckOutDateTime").val(FinlEndDate);
        

        $('#RemoteEmployStatus').html('Open');
        
    }
    $scope.EditRemoteEmployee = function (Pkey) {
        $scope.DisableCOntrolOnEdit = true;
        $scope.DisableCOntrolOnEditAdmin = false;
        $("#DvRENumber").css("display", "");
        $scope.RemoteEmployeeSecurityCheck = {};
        $scope.clearForm();
        try {


            if (Pkey != "") {
                var filter = { 'Pkey': Pkey, FilterText: '' };
                var response = myService.getAllRemoteEmployee(filter);
                response.then(function (response) {


                    $scope.RemoteEmployeeModel = {};
                    $scope.RemoteEmployeeModel = response.data[0];
                    $scope.RemoteEmployeeModel.Pkey = response.data[0].Pkey;
                    $("#Hcode_value").val(response.data[0].Hcode);
                    $("#Pkey").val(response.data[0].Pkey);
                    $scope.RemoteEmployeeModel.Hcode = response.data[0].Hcode;
                    $scope.RemoteEmployeeModel.Name = response.data[0].Name;
                    $scope.RemoteEmployeeModel.EmailID = response.data[0].EmailID;
                    $scope.RemoteEmployeeModel.CheckinDateTime = response.data[0].CheckinDateTime;
                    $scope.RemoteEmployeeModel.CheckOutDateTime = response.data[0].CheckOutDateTime;

                   
                    $("#CheckOutDateTime").val(response.data[0].CheckOutDateTime);
                    $("#CheckinDateTime").val(response.data[0].CheckinDateTime);

                    $scope.RemoteEmployeeModel.IsVehicalParkedOnPremises = response.data[0].IsVehicalParkedOnPremises;
                    if (response.data[0].IsVehicalParkedOnPremises == "True" || response.data[0].IsVehicalParkedOnPremises == "true") {
                        $("#IsVehicalParkedOnPremises").prop("checked", true);
                        $("#DvVichalNumber").css("display", "");
                    }
                    else {
                        $("#IsVehicalParkedOnPremises").prop("checked", false);
                        $("#DvVichalNumber").css("display", "none");
                    }
                    if (response.data[0].Status == "Checked Out") {
                        $scope.DisableCOntrolOnEditAdmin = true;
                    }
                    $scope.RemoteEmployeeModel.VehicalNumber = response.data[0].VehicalNumber;
                    $scope.RemoteEmployeeModel.Comments = response.data[0].Comments;
                    $("#Hcode_value").prop("disabled", true);
                }, function () {
                    console.log("Error Occurs");
                });


            }
            else {
                // $scope.AppointmentModel.VisitorName = selected.originalObject;
            }
        }
        catch (err) {

        }
        $('#exampleModal').modal('show');
    }



    $scope.SaveRemoteEmployee = function () {
        $scope.RemoteEmployeeModel = {};
        $scope.RemoteEmployeeModel.Pkey = $('#Pkey').val();
        $scope.RemoteEmployeeModel.Hcode = $('#Hcode_value').val();
        $scope.RemoteEmployeeModel.Name = $("#Name").val();
        $scope.RemoteEmployeeModel.EmailID = $("#EmailID").val();
        $scope.RemoteEmployeeModel.CheckOutDateTime = $("#CheckOutDateTime").val();
        $scope.RemoteEmployeeModel.CheckinDateTime = $("#CheckinDateTime").val();
        $scope.RemoteEmployeeModel.IsVehicalParkedOnPremises = $("#IsVehicalParkedOnPremises").is(':checked');
        $scope.RemoteEmployeeModel.VehicalNumber = $("#VehicalNumber").val();
        $scope.RemoteEmployeeModel.Comments = $("#Comments").val();
        $scope.RemoteEmployeeModel.Status = $("#TxtStatus").val();
        $scope.RemoteEmployeeModel.BookForWeekend = $("#BookForWeekend").is(':checked');
        
        
      

        var response = myService.SaveRemoteEmployee($scope.RemoteEmployeeModel); // get call from service.js
        response.then(function (d) {

            if (d.data.isSuccessful) {
                $scope.RemoteEmployeeModel = null;
                $scope.showTable = true;
                $('#exampleModal').modal('hide');
                $scope.getAllRemoteEmployee();
                $scope.HcodeAutoCompleteDataCall("ForAutocomplete");
                toaster.success("Visitor's Management", "Remote Employee Request Added Successfully.");
            }
            else {
                toaster.error("Visitor's Management", d.data.message);
            }

        }).catch(function (error) {
            console.log(error);
            toaster.error("Visitor's Management", "Error Occured.");
        })

    }
    $scope.HcodeAutoCompleteDataCall = function (ForAutocomplete) {

        var filter = { FilterText: $scope.searchFilter, "ForAutocomplete": ForAutocomplete };
        var response = myService.getAllRemoteEmployee(filter);

        response.then(function (res) {
            $scope.HcodeAutoCompleteData = res.data;

            //-------Bind Data Table---------------
        }, function () {
            console.log('Error Occured');
        })
    }

    $scope.SelectedRemoteEmployeeForAutocomplete = function (selected) {
        try {


            if (selected != undefined) {
                if (selected.originalObject != "") {
                    var filter = { 'Hcode': selected.originalObject.Hcode, FilterText: 'ForAutotCompleteSelect' };
                    var response = myService.getAllRemoteEmployee(filter);
                    response.then(function (response) {


                        $scope.RemoteEmployeeModel = {};
                        $scope.RemoteEmployeeModel = response.data[0];
                        $scope.RemoteEmployeeModel.Pkey = null;
                        $scope.RemoteEmployeeModel.Hcode = response.data[0].Hcode;
                        $scope.RemoteEmployeeModel.Name = response.data[0].Name;
                        $scope.RemoteEmployeeModel.EmailID = response.data[0].EmailID;

                       

                        $scope.RemoteEmployeeModel.CheckinDateTime = null;
                        $scope.RemoteEmployeeModel.CheckOutDateTime = null;
                        $scope.RemoteEmployeeModel.VehicalNumber = null;
                        $scope.RemoteEmployeeModel.Comments = null;
                        $scope.RemoteEmployeeModel.IsVehicalParkedOnPremises = false;
                        $scope.RemoteEmployeeModel.Status = 'Open';
                        

                    }, function () {
                        console.log("Error Occurs");
                    });

                }
                else {
                    $scope.RemoteEmployeeModel = {};
                    $scope.RemoteEmployeeModel.Status = 'Open';
                }
            }
            else {
                // $scope.AppointmentModel.VisitorName = selected.originalObject;
            }

          

        }
        catch (err) {

        }

    }
    $scope.IsAccessable = function (claim) {

        var strClaims = localStorage.getItem('Claims');
        var claims = strClaims.split(',');

        if (claims.includes(claim))
            return true;
        else
            return false;
    }
    $scope.ViewSecurityCheck = function (Pkey) {
        $scope.RemoteEmployeeSecurityCheck = {};
        $scope.DisableSecuirtyCheck = true;
        $scope.DisableSecuirtyCheckMainCOntrols = true;
        $("#Hcode_value").prop("disabled", true);
        $("#DvRENumber").css("display", "");
        $scope.clearFormSecurityCheck();
        try {


            if (Pkey != "") {
                var filter = { 'Pkey': Pkey, FilterText: '' };
                var response = myService.getAllRemoteEmployee(filter);
                response.then(function (response) {

                    var GuestAccessCardIssuevalue = response.data[0].GuestAccessCardIssue;
                    var AccessCardCollectionStatusValue = response.data[0].AccessCardCollectionStatus;
                    $scope.RemoteEmployeeSecurityCheck = {};
                    $scope.RemoteEmployeeSecurityCheck = response.data[0];
                    $scope.RemoteEmployeeSecurityCheck.Pkey = response.data[0].Pkey;
                    $("#HcodeSecurityCheckModel_value").val(response.data[0].Hcode);
                    $("#HcodeSecurityCheckModel_Pkey").val(response.data[0].Pkey);
                    $scope.RemoteEmployeeSecurityCheck.Hcode = response.data[0].Hcode;
                    $scope.RemoteEmployeeSecurityCheck.Name = response.data[0].Name;
                    $scope.RemoteEmployeeSecurityCheck.EmailID = response.data[0].EmailID;
                    $scope.RemoteEmployeeSecurityCheck.CheckinDateTime = response.data[0].CheckinDateTime;
                    $scope.RemoteEmployeeSecurityCheck.CheckOutDateTime = response.data[0].CheckOutDateTime;
                    $("#CheckOutDateTimeSc").val(response.data[0].CheckOutDateTime);
                    $("#CheckinDateTimeSc").val(response.data[0].CheckinDateTime);
               


                    $scope.RemoteEmployeeSecurityCheck.IsVehicalParkedOnPremises = response.data[0].IsVehicalParkedOnPremises;
                    //$scope.RemoteEmployeeSecurityCheck.GuestAccessCardIssue = response.data[0].GuestAccessCardIssue;

                    if (response.data[0].GuestAccessCardIssue == "Yes") {
                        $("#dvAccessCardCollected").css("display", "");
                        $("#DvDefaultAccessCardNumber").css("display", "");
                

                    }
                    else {
                        
                        $("#dvAccessCardCollected").css("display", "none");
                        $("#DvDefaultAccessCardNumber").css("display", "none");
                    }
                    
                    $scope.DisableSecuirtyCheckMainCOntrols = false;
                    if (response.data[0].Status == "Checked Out") {
                        $scope.DisableSecuirtyCheckMainCOntrols = true;
                    }
                    //$scope.RemoteEmployeeSecurityCheck.AccessCardCollectionStatus = response.data[0].AccessCardCollectionStatus;

                    if (response.data[0].AccessCardCollectionStatus == "No") {
                        $("#DvEscalation").css("display", "");
                    }
                    else {
                        $("#DvEscalation").css("display", "none");

                    }

                    $scope.RemoteEmployeeSecurityCheck.Escalation = response.data[0].Escalation;
                    $scope.RemoteEmployeeSecurityCheck.DeafultGuestCardNumber = response.data[0].DeafultGuestCardNumber;
                    if (response.data[0].IsVehicalParkedOnPremises == "True" || response.data[0].IsVehicalParkedOnPremises == "true") {
                        $("#IsVehicalParkedOnPremisesSc").prop("checked", true);
                        $("#DvVichalNumbersc").css("display", "");
                    }
                    else {
                        $("#IsVehicalParkedOnPremisesSc").prop("checked", false);
                        $("#DvVichalNumbersc").css("display", "none");
                    }
                    $scope.RemoteEmployeeSecurityCheck.VehicalNumber = response.data[0].VehicalNumber;
                    $scope.RemoteEmployeeSecurityCheck.Comments = response.data[0].Comments;
                    $("#HcodeSecurityCheckModel_value").prop("disabled", true);

                    $("#ddlGuestAccessCardIssue").val(GuestAccessCardIssuevalue).trigger('change');
                    $("#ddlAccessCardCollected").val(AccessCardCollectionStatusValue).trigger('change');


                }, function () {
                    console.log("Error Occurs");
                });


            }
            else {
                // $scope.AppointmentModel.VisitorName = selected.originalObject;
            }
        }
        catch (err) {

        }


        $('#SecurityCheckModel').modal('show');
    }


    $scope.SaveSecurityCheck = function () {
        
        $scope.RemoteEmployeeSecurityCheck.Pkey = $('#HcodeSecurityCheckModel_Pkey').val();
        $scope.RemoteEmployeeSecurityCheck.GuestAccessCardIssue = $('#ddlGuestAccessCardIssue').val();
        $scope.RemoteEmployeeSecurityCheck.CheckOutDateTime = $("#CheckOutDateTimeSc").val();
        $scope.RemoteEmployeeSecurityCheck.CheckinDateTime = $("#CheckinDateTimeSc").val();
        

        if ($('#ddlGuestAccessCardIssue').val() == "Yes") {
            $scope.RemoteEmployeeSecurityCheck.AccessCardCollectionStatus = $("#ddlAccessCardCollected").val();
            $scope.RemoteEmployeeSecurityCheck.DeafultGuestCardNumber = $("#DeafultGuestCardNumber").val();
            $scope.RemoteEmployeeSecurityCheck.Status = "Check In";
        }
        else if ($('#ddlGuestAccessCardIssue').val() == "No") {
            $scope.RemoteEmployeeSecurityCheck.Status = "Checked Out";
        }
        if ($("#ddlAccessCardCollected").val() == "No") {
            $scope.RemoteEmployeeSecurityCheck.Escalation = $("#Escalation").val();
            $scope.RemoteEmployeeSecurityCheck.Status = "Checked Out";
        }
        if ($('#ddlAccessCardCollected').val() == "No" || $('#ddlAccessCardCollected').val() == "Yes") {
            $scope.RemoteEmployeeSecurityCheck.Status = "Checked Out";
        }
    

        var response = myService.SaveSecurityCheck($scope.RemoteEmployeeSecurityCheck); // get call from service.js
        response.then(function (d) {

            if (d.data.isSuccessful) {
                $scope.RemoteEmployeeSecurityCheck = [];
                $scope.showTable = true;
                $('#SecurityCheckModel').modal('hide');
                $scope.getAllRemoteEmployee();
                $scope.HcodeAutoCompleteDataCall("ForAutocomplete");
                toaster.success("Visitor's Management", "Security Check Request Added Successfully.");
            }
            else {
                toaster.error("Visitor's Management", d.data.message);
            }

        }).catch(function (error) {
            console.log(error);
            toaster.error("Visitor's Management", "Error Occured.");
        })

    }

    $scope.getAllRemoteEmployee();
    $scope.HcodeAutoCompleteDataCall("ForAutocomplete");
});