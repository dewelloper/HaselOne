if (!String.prototype.endsWith)
    String.prototype.endsWith = function (pattern) {
        var position = this.length - pattern.length;
        return position >= 0 && this.lastIndexOf(pattern) === position;
    };
if (!String.prototype.startsWith) {
    String.prototype.startsWith = function (searchString, position) {
        position = position || 0;
        return this.indexOf(searchString, position) === position;
    };
}
moment.locale('tr');

var activeLanguageAbbr = 'tr';
//angular
var HaselApp = angular.module("HaselApp", ['ui.bootstrap', 'ngSanitize', 'ngMessages', 'ui.mask', 'notifications', 'ngTable', 'ui.select', 'ngCookies', 'ngStorage', 'ui.tree', 'dx', 'ngJsTree', 'Memento']);
HaselApp.config(['MementoProvider', function (MementoProvider) {
    MementoProvider.storageMethod = 'window';
}]);
HaselApp.config(function($provide) {
    $provide.decorator("$exceptionHandler", ['$delegate', function($delegate) {
        return function(exception, cause) {
            $delegate(exception, cause);
            LogEA(exception.message, exception.stack);
           
        };
    }]);
});
HaselApp.filter('total',
    function () {
        return function (input, property) {
            var i = input instanceof Array ? input.length : 0;
            if (typeof property === 'undefined' || i === 0) {
                return i;
            } else if (isNaN(input[0][property])) {
                throw 'filter total can count only numeric values';
            } else {
                var total = 0;
                while (i--)
                    total += input[i][property];
                return total;
            }
        };
    });

HaselApp.filter('timeago', function () {
    return function (date) {
        if (date === null)
            return null;
        return moment(date).fromNow();
    };
});

HaselApp.filter('UTCtoLocalDate', function () {
    return function (date, specifiedFormat) {
        if (date === null)
            return null;
        return moment.utc(date).local().format(specifiedFormat);
    }
});

HaselApp.filter('tel', function () {
    return function (tel) {
        if (!tel) { return ''; }

        var value = tel.toString().trim().replace(/^\+/, '');

        if (value.match(/[^0-9]/)) {
            return tel;
        }

        var country, headCode, number;

        switch (value.length) {
            case 10: // +1PPP####### -> C (PPP) ###-####
                country = "";
                headCode = value.slice(0, 3);
                number = value.slice(3);
                break;

            case 11: // +CPPP####### -> CCC (PP) ###-####
                country = value[0] > 0 ? '+' + value[0] : value[0];
                headCode = value.slice(1, 4);
                number = value.slice(4);
                break;

            case 12: // +CCCPP####### -> CCC (PP) ###-####
                country = '+' + value.slice(0, 2);
                headCode = value.slice(2, 5);
                number = value.slice(5);
                break;

            default:
                return tel;
        }

        number = number.slice(0, 3) + '-' + number.slice(3);

        return (country + " (" + headCode + ") " + number).trim();
    };
});

HaselApp.filter('trimSpaces', [function() {
    return function(string) {
        if (!angular.isString(string)) {
            return string;
        }
        return string.replace(/[\s]/g, '');
    };
}])

HaselApp.factory('alertFactory', function () {
    function GetAlert(options, resultFunc) {
        if (typeof options.customClass == 'undefined')
            options.customClass = "sweetalert-custom"

        if (typeof options.confirmButtonText == 'undefined')
            options.confirmButtonText = "Evet"

        if (typeof options.cancelButtonText == 'undefined')
            options.cancelButtonText = "Hayır"

        if (typeof options.confirmButtonClass == 'undefined')
            options.confirmButtonClass = 'btn btn-primary push-15-r';

        if (typeof options.cancelButtonClass == 'undefined')
            options.cancelButtonClass = 'btn btn-default push-15-r';

        if (typeof options.html == 'undefined')
            options.html = false;

        if (typeof options.buttonsStyling == 'undefined')
            options.buttonsStyling = false;

        if (typeof options.input !== 'undefined')
            swal(options).then(
               function (obj) {
                   if (typeof resultFunc === 'function')
                       resultFunc(obj);
               });
        else
            swal(options).then(
                function (success) {
                    //console.log("GetAlert.then 1", success);
                    if (typeof resultFunc === 'function')
                        resultFunc(true);
                },
                function (dismiss) {
                    //console.log("GetAlert.then 2", dismiss);
                    // dismiss can be 'cancel', 'overlay', 'close', and 'timer'
                    if (typeof resultFunc === 'function')
                        resultFunc(false, dismiss);
                });
    }
    return {
        Show: function (message, title, resultFunc, options) {
            if (typeof options == 'undefined')
                options = {};
            options.text = message;
            options.title = title;
            GetAlert(options, resultFunc)
        },
        Success: function (message, title, options) {
            if (typeof options == 'undefined')
                options = {};

            options.type = 'success';
            options.text = message;
            options.title = 'Başarılı';
            if (title)
                options.title = title;

            if (typeof options.confirmButtonText == 'undefined')
                options.confirmButtonText = 'Tamam'

            GetAlert(options)
        },
        Error: function (message, title, options) {
            if (typeof options == 'undefined')
                options = {};

            options.type = 'error';
            options.text = message;
            options.title = 'Hata!';
            if (title)
                options.title = title;

            if (typeof options.confirmButtonText == 'undefined')
                options.confirmButtonText = 'Tamam'

            GetAlert(options)
        },
        Info: function (message, title, options) {
            if (typeof options == 'undefined')
                options = {};

            options.type = 'info';
            options.text = message;
            options.title = 'Bilgi';
            if (title)
                options.title = title;

            if (typeof options.confirmButtonText == 'undefined')
                options.confirmButtonText = 'Tamam'

            GetAlert(options)
        },
        Confirm: function (message, title, resultFunc, options) {
            if (typeof options == 'undefined')
                options = {};

            options.type = 'question';
            options.text = message;
            options.showCancelButton = true;
            options.title = 'Onay';
            if (title)
                options.title = title;

            GetAlert(options, resultFunc);
        }
    };
});
HaselApp.service("GridService", ["$http", "$rootScope", "$notification",
    function ($http, $rootScope, $notification) {
        this.Grid = function (id) {
            return $('#' + id).data("dxDataGrid");
        };
        this.SetOptions = function (options) {
            if (typeof options === 'undefined')
                options = {};

            $rootScope.filterRow = {
                visible: true,
                applyFilter: "auto"
            };
            $rootScope.headerFilter = {
                visible: true,
                texts: {
                    "cancel": "İptal",
                    "emptyValue": "(Boş)",
                    "ok": "Tamam"
                }
            };

            var store = null;
            var triggerDataSourceBound = function (obj) {
                if (typeof options.onDataSourceBound === 'function')
                    options.onDataSourceBound(obj);
            }
            if (typeof options.itemAlias === 'undefined')
                options.itemAlias = "entity";
            if (typeof options.dataSourceUrl !== 'undefined') {
                store = new DevExpress.data.CustomStore({
                    load: function (loadOptions) {
                        var deferred = $.Deferred();
                        if (typeof options.dataSourceFilter !== 'undefined')
                            $http.post(options.dataSourceUrl, options.dataSourceFilter).then(
                                function (response) {
                                    var result = response.data;
                                    triggerDataSourceBound(result);
                                    if (result.IsSuccess)
                                        deferred.resolve(result.Data);
                                    else
                                        $notification.Notify("Hata", result.Message, 'd');
                                },
                                function (e) {
                                    triggerDataSourceBound(null);
                                    if (e.status == 503)
                                        $notification.Notify("Hata", "Sunucu yanıt vermiyor.", 'd');
                                    if (e.status == 500)
                                        $notification.Notify("Hata", "Sunucuda bir hata oluştu.", 'd');
                                    if (e.status == 401)
                                        $notification.Notify("Hata", "Oturum açınız.", 'd');
                                    if (e.status == 408)
                                        $notification.Notify("Hata", "Bağlantı zaman aşımına uğradı.", 'd');
                                    if (e.status == 415)
                                        $notification.Notify("Hata", "Desteklenmeyen format.", 'd');

                                    return e;
                                });
                        else
                            $http.post(options.dataSourceUrl).then(
                                function (response) {
                                    var result = response.data;
                                    triggerDataSourceBound(result);
                                    if (result.IsSuccess)
                                        deferred.resolve(result.Data);
                                },
                                function (e) {
                                    triggerDataSourceBound(null);
                                    return e;
                                });
                        return deferred.promise();
                    },
                    byKey: function(key, extra) {},
                    update: function(key, values) {},
                    insert: function(key, value) { },
                      remove: function(key, value) { }
                });

                options.dataSource = store;
            }

            if (typeof options.onContextMenuPreparing !== "undefined") {
                options.onContextMenuPreparing = options.onContextMenuPreparing;
            }

            //{
            //    text: "insert",
            //    onItemClick: function () {
            //        $("#gridContainer").dxDataGrid("instance").insertRow();
            //    }
            //},
            //{
            //    text: "delete",
            //    onItemClick: function () {
            //        $("#gridContainer").dxDataGrid("instance").removeRow(e.row.rowIndex);
            //    }
            //}
            //];
            if (typeof options.noDataText === 'undefined')
                options.noDataText = "Veri yok";

            if (typeof options.hoverStateEnabled === 'undefined')
                options.hoverStateEnabled = true;
            if (typeof options.selection === 'undefined')
                options.selection = {
                    mode: "single"
                };
            if (typeof options.onSelectionChanged === 'undefined')
                options.onSelectionChanged = function (selectedItems) {
                    var data = selectedItems.selectedRowsData;
                    if (data) {
                        options.SelectedRow = data[0];
                        options.SelectedRows = data;
                    }
                }

            if (typeof options.bindingOptions === 'undefined')
                options.bindingOptions = {
                    filterRow: "filterRow",
                    headerFilter: "headerFilter"
                }
            if (typeof options.ReportName === 'undefined')
                options.ReportName = "GridReport" + " -" + moment().format('YYYYMMDDhhmmss');
            if (typeof options.export === 'undefined')
                options.export = {
                    enabled: false,
                    fileName: options.ReportName
                }

            if (typeof options.paging === 'undefined')
                options.paging = {
                    pageSize: 10
                }

            if (typeof options.sorting === 'undefined')
                options.sorting = {
                    ascendingText: "Sırala A-Z",
                    clearText: "Sıralama Kaldır",
                    descendingText: "Sırala Z-A",
                    mode: "multiple"
                }
            if (typeof options.pager === 'undefined')
                options.pager = {
                    infoText: "Sayfa {0}/{1}",
                    showPageSizeSelector: true,
                    allowedPageSizes: [5, 10, 20],
                    showInfo: true
                }

            if (typeof options.filterRow === 'undefined')
                options.filterRow = {
                    visible: true,
                    applyFilterText: "Filtre uygula",
                    betweenEndText: "Bitiş",
                    betweenStartText: "Başlangıç",
                    showAllText: "(Hepsi)",
                    operationDescriptions: {
                        between: "Arası",
                        contains: "İçer",
                        endsWith: "ile Biten",
                        equal: "Eşit",
                        greaterThan: "Büyüktür",
                        greaterThanOrEqual: "Büyük eşit",
                        lessThan: "Küçüktür",
                        lessThanOrEqual: "Küçük eşit",
                        notContains: "İçermiyor",
                        notEqual: "Eşit değil",
                        startsWith: "İle başlayan",
                        reset: "Sıfırla"
                    }
                }

            if (typeof options.allowColumnReordering === 'undefined')
                options.allowColumnReordering = true;

            if (typeof options.allowColumnResizing === 'undefined')
                options.allowColumnResizing = true;

            if (typeof options.columnAutoWidth === 'undefined')
                options.columnAutoWidth = true;

            if (typeof options.columnChooser === 'undefined')
                options.columnChooser = {
                    enabled: false
                };

            if (typeof options.groupPanel === 'undefined')
                options.groupPanel = {
                    visible: false
                };
            if (typeof options.columnFixing === 'undefined')
                options.columnFixing = {
                    enabled: true
                };

            if (typeof options.columns === 'undefined') {
                //  console.log("grid columns not found");
                return null;
            } else
                return options;
        }

        this.Refresh = function (id) {
            var grid = this.Grid(id);
            if (typeof grid != 'undefined')
                grid.refresh();
        }

        this.Repaint = function(id)
        {
            var g = this.Grid(id);
            //debugger;
           // g.repaintRows();
           // g.resize();
        }

        this.ExportToExcel = function(id) {
            var grid = this.Grid(id);
            if (typeof grid != 'undefined')
                grid.exportToExcel();
        }

        this.UpdateNewValues = function (e) {
            angular.forEach(e.newData, function (value, keyx) {
                e.key[keyx] = value;
            });

            return e;
        }

        this.InsertNewValues = function (e) {
            return e.data;
        }
        this.SelectRow = function (gridId, selectedRows, rowData, idColumn) {
            var isExist = false;
            if (typeof selectedRows === 'undefined' || selectedRows === null)
                selectedRows = [];
            for (var i = 0; i < selectedRows.length; i++) {
                if (rowData[idColumn] == selectedRows[i][idColumn]) {
                    isExist = true;
                    break;
                }
            }
            if (!isExist)
                selectedRows.push(rowData);
            this.Grid(gridId).selectRows(selectedRows);
        }
    }

]);
HaselApp.run(['$anchorScroll', '$rootScope', 'alertFactory', '$interval', '$cookies', '$timeout', '$localStorage', 'GridService', '$q', "$notification", "$http", "$uibModal", 'Memento',
    function ($anchorScroll, $rootScope, alertFactory, $interval, $cookies, $timeout, $localStorage, GridService, $q, $notification, $http, $uibModal, Memento) {
        $rootScope.MainTemplate = App;
        $rootScope.AlertService = alertFactory;
        $rootScope.GridService = GridService;
        $rootScope.Viewport = BootstrapToolkit;
        if (typeof ReportFilter !== 'undefined')
            $rootScope.ReportFilter = ReportFilter;

        $rootScope.Site = {
            Name: 'Hasel CRM',
            localStorage: true,
            Settings: {
                IsRightSidebarOpen: false
            }
        }

        $rootScope.getQueryStringByName = function (name, url) {
            if (!url) {
                url = window.location.href.toLowerCase();
            }
            name = name.toLowerCase();
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }

        $rootScope.makeId =function m()
        {
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            for (var i = 0; i < 5; i++)
                text += possible.charAt(Math.floor(Math.random() * possible.length));

            return text;
        }

        $rootScope.randomMake = function ()
        {
          return  Math.random().toString(36).substring(7);
        }

        //*** LOCAL STORAGE ***//
        if ($rootScope.Site.localStorage) {
            if ($rootScope.Site.localStorage) {
                if (angular.isDefined($localStorage.HaselPanelSettings)) {
                    $rootScope.Site.Settings = $localStorage.HaselPanelSettings;
                } else {
                    $localStorage.HaselPanelSettings = $rootScope.Site.Settings;
                }
            }

            $rootScope.$watch('Site.Settings', function () {
                $localStorage.HaselPanelSettings = $rootScope.Site.Settings;
            }, true);
        }

        //*** END LOCAL STORAGE ***//

        //*** COOKIEs ***//
        $rootScope.GetCookie = function (cookieName) {
            try {
                return $cookies.getObject(cookieName);
            } catch (e1) {
                try {
                    return $cookies.get(cookieName);
                } catch (e2) {
                    console.log(e2);
                }
            }
        }

        $rootScope.SetCookie = function (cookieName, value, isObject, options) {
            /* OPTIONS *************************************************
            path - {string} - The cookie will be available only for this path and its sub-paths. By default, this is the URL that appears in your <base> tag.
            domain - {string} - The cookie will be available only for this domain and its sub-domains.
                                For security reasons the user agent will not accept the cookie if the current domain is not a sub-domain of this domain or equal to it.
            expires - {string|Date} - String of the form "Wdy, DD Mon YYYY HH:MM:SS GMT" or a Date object indicating the exact date/time this cookie will expire.
            secure - {boolean} - If true, then the cookie will only be available through a secured connection.
            */
            if (typeof options == 'object')
                if (isObject)
                    return $cookies.putObject(cookieName, value, options);
                else
                    return $cookies.put(cookieName, value, options);
            else
                if (isObject)
                    return $cookies.putObject(cookieName, value);
                else
                    return $cookies.put(cookieName, value);
        }

        $rootScope.RemoveCookie = function (cookieName) {
            if (typeof options == 'object')
                return $cookies.remove(cookieName, options);
            else
                return $cookies.remove(cookieName);
        }

        //*** END COOKIEs ***//

        //*** POPUP ***//
        $rootScope.OpenPopup = function (page, qs, w, h, wName, resizable) {
            var width = w || 1010;
            if (typeof resizable === 'undefined')
                resizable = true;
            var height = h || 748;
            var specs = "width=" + width + ",height=" + height;

            specs = specs + ",scrollbars=yes,resizable=" + (resizable ? "yes" : "no") + ",left=0,top=0,menubar=no,toolbar=no,status=yes";
            page = page + ((qs != null && qs != "") ? "?" + (qs.toString().substr(0, 1) == "&" ? "" : "&") + qs : "");
            var wnd = window.open(page, wName, specs);

            wnd.focus();
            return wnd;
        }

        $rootScope.ShowPopup = function (page, qs, w, h, windowName, closeFn, closeFnParam, resizable) {
            if (typeof windowName === 'undefined')
                windowName = "";

            var queryStringUri = null;
            if (qs !== null)
                queryStringUri = decodeURI(jQuery.param(qs))
            var t = $rootScope.OpenPopup(page, queryStringUri, w, h, windowName, resizable);
            if (typeof closeFn !== 'undefined' && typeof closeFn === 'function')
                t.onbeforeunload = function () {
                    closeFn(closeFnParam);
                }
        }

        //*** END POPUP ***//
        $rootScope.ErrorHandler = function (e) {
            if (e.status == 503) {
                $notification.Notify("Hata", "Sunucu yanıt vermiyor.", 'd');
            }
            if (e.status == 500)
                $notification.Notify("Hata", "Sunucuda bir hata oluştu.", 'd');
            if (e.status == 401)
                $notification.Notify("Hata", "Oturum açınız.", 'd');
            if (e.status == 408)
                $notification.Notify("Hata", "Bağlantı zaman aşımına uğradı.", 'd');
            if (e.status == 415)
                $notification.Notify("Hata", "Desteklenmeyen format.", 'd');
            if (e.status == -1) {
                $notification.Notify("Hata", "Bağlantı bulunamadı.", 'd');
                //  $notification.Notify("Hata", "Bağlantı bulunamadı. <a href='#' onclick='location.reload();'>Sayfa yenile</a>", 'd');
            }
        }
        $rootScope.IsLoading = false;
        $rootScope.AjaxPost = function (uri, filterObj) {
            $rootScope.IsLoading = true;
            var deferred = $q.defer();
            $http.post(uri, filterObj).then(
                function (response) {
                    $rootScope.IsLoading = false;
                    deferred.resolve(response.data);
                },
                function (e) {
                    $rootScope.IsLoading = false;
                    deferred.reject(e);
                    $rootScope.ErrorHandler(e);
                });

            return deferred.promise;
        }

        $rootScope.GeneralGridEditingTexts = {
            addRow: "Ekle",
            cancelAllChanges: "Değişiklik İptal",
            cancelRowChanges: "Iptal",
            confirmDeleteMessage: "Silme işlemini onaylıyormusunuz?",
            confirmDeleteTitle: "Silme",
            deleteRow: "Sil",
            editRow: "Düzenle",
            saveAllChanges: "Değişikliği Kaydet",
            saveRowChanges: "Kaydet",
            undeleteRow: "Silme Geri Al",
            validationCancelChanges: "Değişiklik İptal"
        };

        $rootScope.removeKeyFromList = function (columnsList, removeKey, keyName) {
            var newList = [];
            if (columnsList.length > 0 && removeKey.length > 0) {
                for (var i = 0; i < columnsList.length; i++) {
                    var isAdd = true;

                    for (var j = 0; j < removeKey.length > 0; j++) {
                        if (columnsList[i][keyName] == removeKey[j]) {
                            isAdd = false;
                        }
                    }
                    if (isAdd) {
                        newList.push(columnsList[i]);
                    }
                }
            }
            return newList;
        }

        function handleException(error) {
            vm.uiState.isMessageAreaHidden = false;
            vm.uiState.isLoading = false;
            vm.uiState.messages = [];

            switch (error.status) {
                case 400: // 'Bad Request'
                    // Model state errors
                    var errors = error.data.modelState;

                    // Loop through and get all
                    // validation errors
                    for (var key in errors) {
                        for (var i = 0; i < errors[key].length; i++) {
                            vm.uiState.messages.push({
                                message: errors[key][i]
                            });
                        }
                    }

                    break;
                case 404: // 'Not Found'
                    vm.uiState.messages.push({
                        message: "The data you were " +
                            "requesting could not be found"
                    });
                    break;

                case 500: // 'Internal Error'
                    vm.uiState.messages.push({
                        message: error.data.exceptionMessage
                    });
                    break;

                default:
                    vm.uiState.messages.push({
                        message: "Status: " +
                            error.status +
                            " - Error Message: " +
                            error.statusText
                    });
                    break;
            }
        }

        $rootScope.ToggleCollapsePanel = function (panelId, expandStatus) {
            var panel = $("#" + panelId);
            if (typeof expandStatus !== 'undefined') {
                if (expandStatus && !panel.is(":visible")) panel.toggle("in");
                if (!expandStatus && panel.is(":visible")) panel.toggle("in");
            } else
                panel.toggle("in");
        }
        $rootScope.ScrollTo = function (id) {
            $("body, html").animate({ scrollTop: $('#' + id).offset().top - 200 }, 600);
        }

        $rootScope.OpenModal = function (options) {
            if (typeof options === 'undefined')
                options = {};
            if (typeof options.size === 'undefined')
                options.size = 'md';
            if (typeof options.templateUrl === 'undefined' && typeof options.template === 'undefined')
                console.log("Modal options: template veya templateUrl belirleyiniz.");
            if (typeof options.backdrop === 'undefined')
                options.backdrop = 'static';
            if (typeof options.controller === 'undefined')
                options.controller = ["$scope", "$uibModalInstance", function ($scope, $uibModalInstance) {
                    $scope.Close = function () {
                        $uibModalInstance.dismiss();
                    }
                }];
            return $uibModal.open(options);
        }

        $rootScope.ConfirmModal = function (data) {
            var deferred = $q.defer();
            var modalInstance = $rootScope.OpenModal({
                templateUrl: '/Content/HtmlTemplates/ConfirmModalTemplate.html?v=1',
                resolve: {
                    Data: function () {
                        return data;
                    }
                },
                controller: ["$scope", "$timeout", "Data", "$uibModalInstance",
                    function ($scope, $timeout, Data, $uibModalInstance) {
                        $scope.Data = Data;
                        if (typeof $scope.Data.Title === 'undefined')
                            $scope.Data.Title = 'Onay';

                        if (typeof $scope.Data.Button1Class === 'undefined')
                            $scope.Data.Button1Class = 'btn-success';

                        if (typeof $scope.Data.Button2Class === 'undefined')
                            $scope.Data.Button2Class = 'btn-danger';

                        if (typeof $scope.Data.Button1Text === 'undefined')
                            $scope.Data.Button1Text = 'Kaydet';

                        if (typeof $scope.Data.Button2Text === 'undefined')
                            $scope.Data.Button2Text = 'Kapat';
                        if (typeof $scope.Data.Message === 'undefined')
                            $scope.Data.Message = "Bu işlemi yapmak istediğinize eminmisiniz?";
                        $scope.Close = function () {
                            $uibModalInstance.dismiss();
                        }

                        $scope.Confirm = function (res) {
                            deferred.resolve(res);
                            $scope.Close();
                        }
                    }]
            });
            return deferred.promise;
        }

        $rootScope.ShowTab = function (id) {
            if (typeof id !== 'undefined')
                $('[href=#' + id + ']').tab('show');
        } 

        $rootScope.GetMemento= function(obj)
        {
            var memObj = new Memento(obj);
            return memObj;
        }

        $rootScope.MessagePanelFromModelTracker = function(item)
        {
            if (typeof item == "object") {
                if (item != null) {
                    if (item.ResultType == undefined)
                    {
                        item.ResultType = "Hide";
                    }
                   
                }
            }
           
        }

    }
]);