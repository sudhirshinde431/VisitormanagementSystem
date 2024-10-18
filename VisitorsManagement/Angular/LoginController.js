var loginApp = angular.module("myLoginApp", []);
loginApp.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('HttpInterceptor');
}])

.factory('HttpInterceptor', ['$rootScope', '$q', '$timeout', function ($rootScope, $q, $timeout) {

    return {
        'request': function (config) {

            if (config.url != "../SessionHeartbeat/ProcessRequest") {
                //$timeout(function() {
                $rootScope.isLoading = true;    // loading after 200ms
                $(".overlay").show();
                //}, 20000);
            }
            return config || $q.when(config);
        },
        'requestError': function (rejection) {
            return $q.reject(rejection);
        },
        'response': function (response) {

            $rootScope.isLoading = false;       // done loading
            $timeout(function () {
                $(".overlay").hide();
            }, 500);

            return response || $q.when(response);
        },
        'responseError': function (rejection) {
            return $q.reject(rejection);
        }
    };
}]);
var baseURL = "../";
loginApp.service("myService", function ($http) {

    this.UserLogin = function (User) {
        var response = $http({
            method: "post",
            url: baseURL + "Login/UserLogin",
            data: { user: User },
            dataType: "json"
        });
        return response;
    }

    this.GetUserAccess = function (UserId) {
        var response = $http({
            method: "get",
            url: baseURL + "Login/GetUserAccess",
            params: { UserId: UserId },
            dataType: "json"
        });
        return response;
    }

    this.sendPassword = function (EmailID) {
        var response = $http({
            method: "get",
            url: "../Login/sendPassword",
            params: { EmailID: EmailID },
            dataType: "json"
        });
        return response;
    }

});

loginApp.controller("sendPasswordCtrl", function ($scope, myService, $rootScope, $timeout) {

    $scope.sendPasswordMail = function () {

        if ($scope.resetPassEmail == '' || $scope.resetPassEmail == undefined)
            return;

        var sendPass = myService.sendPassword($scope.resetPassEmail);
        sendPass.then(function (res) {

            alert(res.data);
        }, function () {
            console.log('Error Occured');
        })

        $('#mdlforgotPassword').modal('hide');
    }
});


loginApp.controller("myLoginCtrl", function ($scope, myService, $rootScope, $timeout) {

    $scope.loadComplete = function () {
        $rootScope.isLoading = false;       // done loading
        $timeout(function () {
            $(".overlay").hide();
        }, 200);
    }
    $scope.loadComplete();

    $scope.forgotPassword = function () {
        $('#mdlforgotPassword').modal('show');
    }

    var appointmentId = $('#appointmentId').val();
    var previousPage = $('#previousPage').val();

    if (appointmentId && previousPage) {
        localStorage.setItem('previosPage', 'CheckIn');
        localStorage.setItem('appointmentId', appointmentId);
    }


    $scope.UserLogin = function () {
        $scope.msg = '';
        if ($scope.UserName == '' || $scope.UserName == undefined || $scope.password == '' || $scope.password == undefined) {
            return;
        }

        var User = {
            email: $scope.UserName,
            password: $scope.password
        };
        //localStorage.clear();
        if ($scope.UserName != '' && $scope.password != '') {
            var getData = myService.UserLogin(User);
            getData.then(function (msg) {                
                if (msg.data == 0) {
                    //$("#alertModal").modal('show');
                    $scope.msg = "Incorrect Login Id Or Password!";
                }
                else if (msg.data == 1) {                   
                    $scope.msg = "Account disabled, Contact System Administrator";
                }
                else if (msg.data == "Block") {
                    $scope.msg = "System expired, Contact System Administrator";
                }                
                else {

                    uID = msg.data;
                    localStorage.setItem("UserID", msg.data.UserId);
                    localStorage.setItem("UserName", msg.data.FirstName);
                    localStorage.setItem("UserFullName", msg.data.FullName);
                    localStorage.setItem("IsAdmin", msg.data.IsAdmin);
                    localStorage.setItem("Claims", msg.data.Claims);
                    localStorage.setItem("RoleName", msg.data.RoleName);

                    if (localStorage.getItem("previosPage") == 'CheckIn') {
                        var appointmentId = localStorage.getItem("appointmentId");
                        if (appointmentId) {
                            localStorage.removeItem("previosPage");
                            window.location.href = baseURL + "VM/CheckIn?appointmentId=" + appointmentId;
                        }
                        else {
                            window.location.href = baseURL + "Dashboard/Index";
                        }
                    }
                    else
                        window.location.href = baseURL + "Dashboard/Index";

                }
            });
        }
    }
});

