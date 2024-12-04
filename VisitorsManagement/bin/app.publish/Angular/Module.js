//var app = angular.module("visitorsManagement", ["angularUtils.directives.dirPagination", "ui.bootstrap", "ngSanitize", "ngFileUpload", "ngTable", "ngIdle"]);
var app = angular.module("visitorsManagement", ["angularUtils.directives.dirPagination", "ui.bootstrap", "ngSanitize", "ngFileUpload", "ngTable", "ngIdle", "toaster", "ngAnimate", "ngTableToCsv", "angucomplete-alt", "angular.filter"]);

//toaster.options = {
//    "debug": false,
//    "positionClass": "toast-top-right",
//    "onclick": null,
//    "fadeIn": 300,
//    "fadeOut": 1000,
//    "timeOut": 5000,
//    "extendedTimeOut": 1000
//}
var baseURL = "/";
app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('HttpInterceptor');
}])
.factory('HttpInterceptor', ['$rootScope', '$q', '$timeout', function ($rootScope, $q, $timeout) {

    return {
        'request': function (config) {

            if (config.url != "/SessionHeartbeat/ProcessRequest") {
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
            }, 200);

            return response || $q.when(response);
        },
        'responseError': function (rejection) {
            return $q.reject(rejection);
        }
    };
}]);
app.config(['KeepaliveProvider', 'IdleProvider', function (KeepaliveProvider, IdleProvider) {
    //IdleProvider.idle(5);
    //IdleProvider.timeout(20 * 60);
    KeepaliveProvider.interval(60);
    KeepaliveProvider.http('/SessionHeartbeat/ProcessRequest');
}]);
app.run(['Idle', function (Idle) {
    Idle.watch();
}]);

app.factory('Excel', function ($window) {
    var uri = 'data:application/vnd.ms-excel;base64,',
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
        base64 = function (s) { return $window.btoa(unescape(encodeURIComponent(s))); },
        format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };
    return {
        tableToExcel: function (tableId, worksheetName) {
            var table = $(tableId),
                ctx = { worksheet: worksheetName, table: table.html() },
                href = uri + base64(format(template, ctx));
            return href;
        }
    };
});
app.directive('excelExport', function () {
    return {
        restrict: 'A',
        scope: {
            fileName: "@",
            data: "&exportData"
        },
        replace: true,
        template: '<button class="btn btn-primary btn-ef btn-ef-3 btn-ef-3c mb-10" ng-click="download()">Export to Excel <i class="fa fa-download"></i></button>',
        link: function (scope, element) {

            scope.download = function () {

                function datenum(v, date1904) {
                    if (date1904) v += 1462;
                    var epoch = Date.parse(v);
                    return (epoch - new Date(Date.UTC(1899, 11, 30))) / (24 * 60 * 60 * 1000);
                };

                function getSheet(data, opts) {
                    var ws = {};
                    var range = { s: { c: 10000000, r: 10000000 }, e: { c: 0, r: 0 } };
                    for (var R = 0; R != data.length; ++R) {
                        for (var C = 0; C != data[R].length; ++C) {
                            if (range.s.r > R) range.s.r = R;
                            if (range.s.c > C) range.s.c = C;
                            if (range.e.r < R) range.e.r = R;
                            if (range.e.c < C) range.e.c = C;
                            var cell = { v: data[R][C] };
                            if (cell.v == null) continue;
                            var cell_ref = XLSX.utils.encode_cell({ c: C, r: R });

                            if (typeof cell.v === 'number') cell.t = 'n';
                            else if (typeof cell.v === 'boolean') cell.t = 'b';
                            else if (cell.v instanceof Date) {
                                cell.t = 'n'; cell.z = XLSX.SSF._table[14];
                                cell.v = datenum(cell.v);
                            }
                            else cell.t = 's';

                            ws[cell_ref] = cell;
                        }
                    }
                    if (range.s.c < 10000000) ws['!ref'] = XLSX.utils.encode_range(range);
                    return ws;
                };

                function Workbook() {
                    if (!(this instanceof Workbook)) return new Workbook();
                    this.SheetNames = [];
                    this.Sheets = {};
                }

                var wb = new Workbook(), ws = getSheet(scope.data());
                /* add worksheet to workbook */
                wb.SheetNames.push(scope.fileName);
                wb.Sheets[scope.fileName] = ws;
                var wbout = XLSX.write(wb, { bookType: 'xlsx', bookSST: true, type: 'binary' });

                function s2ab(s) {
                    var buf = new ArrayBuffer(s.length);
                    var view = new Uint8Array(buf);
                    for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
                    return buf;
                }

                saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), scope.fileName + '.xlsx');

            };

        }
    };
}
 );
app.directive('fixedTableHeaders', ['$timeout', function ($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            $timeout(function () {
                var container = element.parentsUntil(attrs.fixedTableHeaders);
                element.stickyTableHeaders({ scrollableArea: container, "fixedOffset": 2 });
            }, 0);
        }
    }
}]);
app.constant('appConfig', {
    MaxDays: 5,
    RowsToDisplay: 500
});

app.constant('Months', {
    1: 'Jan',
    2: 'Feb',
    3: 'Mar',
    4: 'Apr',
    5: 'May',
    6: 'Jun',
    7: 'Jul',
    8: 'Aug',
    9: 'Sep',
    10: 'Oct',
    11: 'Nov',
    12: 'Dec'
});
var UsersSelectedGroups;
function removeAllSessionStorage() {
    Object.keys(localStorage)
        .forEach(function (key) {
            localStorage.removeItem(key);
        });
};
function pad(d) {
    return (d < 10) ? '0' + d.toString() : d.toString();
}

Date.prototype.toShortFormat = function () {

    var month_names = ["Jan", "Feb", "Mar",
                      "Apr", "May", "Jun",
                      "Jul", "Aug", "Sep",
                      "Oct", "Nov", "Dec"];

    var day = this.getDate();
    var month_index = this.getMonth();
    var year = this.getFullYear();

    return "" + pad(day) + "-" + month_names[month_index] + "-" + year;
}
Date.prototype.toCurrentTime = function formatAMPM() {
    var hours = this.getHours();
    var minutes = this.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}

$(window).scroll(function () {
    if ($(window).scrollTop() >= 50) {
        $('.innermenus').addClass('sticky');
    } else {
        $('.innermenus').removeClass('sticky');
    }
});



app.constant('constants', {

    VMStatus: {
        'Waitingforapproval': 'Waiting for approval',
        'Open': 'Open',
        'CheckIn': 'Checked In',
        'CheckOut': 'Checked Out',
        'Cancel': 'Cancelled',
        'Reject': 'Rejected',
        'DirectApproval': 'Direct approval'
    },

    //WPStatus: {
    //    'New': 'New',
    //    'WaitingHRApproval': 'Waiting HR Approval',
    //    'RejectedByHR':'Rejected By HR',
    //    'WaitingIMSApproval': 'Waiting IMS Approval',
    //    'RejectedByIMS': 'Rejected By IMS',
    //    'WaitingFinalApproval': 'Waiting Approval',
    //    'RejectedByApprover': 'Rejected By Approver',
    //    'Approved' : 'Approved',
    //    'Reject': 'Rejected'
    //}

    WPStatus: {
        'Open': 'Open',
        'WaitingforManagerApproval': 'Waiting for Manager Approval',
        'RejectedByManager': 'Rejected By Manager',
        'WaitingforHRAction': 'Waiting for HR Action',
        'RejectedByHR': 'Rejected By HR',
        'WaitingforSafetyTrainingEHSApproval': 'Waiting for Safety Training & EHS Approval',
        'WaitingforSafetyTraining': 'Waiting for Safety Training',
        'WaitingforIMSApproval': 'Waiting for IMS Approval',
        'Approved': 'Approved',
        'Reject': 'Rejected',
        'Closed': 'Closed'
    }

});

app.constant('FileGroupType', {
    FileGroupType: {
        'LegalDocument': 11,
        'LegalReport': 12,
        'LegalRequirement': 13
    }
});

app.constant('Levels', {
    'Purchase': 1,
    'Quality1': 2,
    'Stores1': 3,
    'Quality2': 4,
    'Stores2': 5,
    'Quality3': 6,
    'Production': 7
});

var MonthNames = {
    1: 'Jan',
    2: 'Feb',
    3: 'Mar',
    4: 'Apr',
    5: 'May',
    6: 'Jun',
    7: 'Jul',
    8: 'Aug',
    9: 'Sep',
    10: 'Oct',
    11: 'Nov',
    12: 'Dec'
};
