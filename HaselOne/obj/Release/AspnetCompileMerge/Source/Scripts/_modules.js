HaselApp.service('selectHelper', ["$timeout", function ($timeout) {
    this.InitDirectiveScope = function (scope, attrs, element) {
        scope.params = {};

        scope.multipleX = false;
        scope.enabled = true;

        $timeout(function () {
            element.find("input").attr("name", attrs.name);
            element.find("input").attr("id", attrs.name);
        });

        if (angular.isDefined(attrs["multiple"]) && scope.multiple != false) {
            scope.multipleX = true;
        }

        scope.enabled = typeof scope.ngEnabled == 'undefined' ? true : scope.ngEnabled;
        scope.$watch("ngEnabled", function (v) {
            scope.enabled = typeof scope.ngEnabled == 'undefined' ? true : scope.ngEnabled;
        });
        if (typeof scope.ngModel == 'undefined')
            scope.ngModel = null;

        if (typeof scope.options != 'undefined') {
            if (typeof scope.options.textField == 'undefined')
                scope.options.textField = "Text";
            if (typeof scope.options.placeholder == 'undefined')
                scope.options.placeholder = "Hepsi";
            if (typeof scope.options.label == 'undefined')
                scope.options.label = "Tanımsız";
            if (typeof scope.options.customizeText != 'function')
                scope.options.customizeText = function (c) {
                    return c[scope.options.textField];
                };
            if(typeof scope.options.DisableAdd == 'undefined')
            scope.options.DisableAdd = false;

        }

        scope.params.Model = scope.multipleX ? [] : {};

        scope.GetDisplayText = function (p) {
            if (angular.isArray(p)) {
                return "Desteklenmeyen nesne türü";
            }
            if (angular.isNumber(p))
                return p;

            if (angular.isString(p))
                return p;

            if (angular.isDate(p))
                return moment(p).format("DD.MM.YYYY HH:mm");

            if (angular.isFunction(scope.options.customizeDisplayText) && p != null)
                return scope.options.customizeDisplayText(p);

            if (angular.isObject(p)) {
                if (angular.isDefined(scope.options.textField)) {
                    var witchProperty = scope.options.textField;
                    return p[witchProperty];
                }
                else {
                    return "Metin alanı gereklidi.";
                }
            }
            else
                return "Desteklenmeyen nesne";
        }

        scope.GetText = function (p) {
            if (angular.isFunction(scope.options.customizeText) && p != null)
                return scope.options.customizeText(p);
            return scope.GetDisplayText(p);
        }

        scope.$watch("ngModel", function (v) {
            if (scope.ngModel != scope.params.Model) {
                scope.params.Model = scope.ngModel;
                //console.log("$watch.ngModel", scope.params.Model);
            }
        });

        scope.$watch("params.Model", function (v) {
           
            if (scope.ngModel != scope.params.Model) {
                scope.ngModel = scope.params.Model;
                $timeout(Changed);
                if (typeof scope.options.valueField !== "undefined" && typeof scope.ngSyncValue !== "undefined") {
                    scope.ngSyncValue = scope.ngModel == null ? null : scope.ngModel[scope.options.valueField];
                }
                //console.log("Selection changed: ", scope.params.Model);
            }
        });

        scope.$watch('ngSyncValue', function () {
            if (typeof scope.options.valueField !== "undefined" && typeof scope.ngSyncValue !== "undefined")
                var modelValue = scope.ngModel == null ? null : scope.ngModel[scope.options.valueField];
            if (scope.ngSyncValue !== modelValue) {
                scope.isValueNotFound = true;
                for (var i = 0; i < scope.options.data.length; i++) {
                    var value = scope.options.data[i];
                    if (value[scope.options.valueField] === scope.ngSyncValue) {
                        scope.ngModel = value;
                        scope.isValueNotFound = false;
                        break;
                    }
                }

                if (scope.isValueNotFound) {
                    scope.ngModel = null;
                }
            }
        });
        function Changed() {
            if (typeof scope.ngChange == 'function')
                scope.ngChange(scope.ngModel);
        };
    }
}]);

HaselApp.service('refreshHelper', ["$rootScope", function ($rootScope) {
    $rootScope.TableTmp = null;
    $rootScope.SelectedRow = null;
    $rootScope.RowIdenity = null;

    this.SaveTableParams = function (params, selectedRow, rowIdentity) {
        $rootScope.TableTmp = angular.copy(params);
        $rootScope.SelectedRow = selectedRow;
        $rootScope.RowIdenity = rowIdentity;
        delete $rootScope.TableTmp.data;
    }

    this.LoadTableParams = function (params) {
        if ($rootScope.TableTmp != null) {
            params.filter($rootScope.TableTmp.filter());
            params.group($rootScope.TableTmp.group());
            params.sorting($rootScope.TableTmp.sorting());
            params.page($rootScope.TableTmp.page());
        }
        if ($rootScope.SelectedRow != null) {
            var data = null;
            if (params.settings().dataset != null)
                data = params.settings().dataset;
            if (params.data != null)
                data = params.data;
            if ($rootScope.SelectedRow[$rootScope.RowIdenity] == undefined)
                $rootScope.SelectedRow = data[0];

            for (var i = 0; i < data.length; i++) {
                if ($rootScope.SelectedRow[$rootScope.RowIdenity] == data[i][$rootScope.RowIdenity]) {
                    angular.extend($rootScope.SelectedRow, data[i]);
                    break;
                }
            }
        }
    }
}]);

HaselApp.service('fileHelper', function () {
    this.CreateDownloadLink = function (fileData) {
        var blob = new Blob([fileData.data], { type: fileData.headers("content-type") })
        var fileName = 'file_' + moment().format('YYYYMMDDhhmmsszz')

        var matches = fileData.headers("content-disposition").match(/filename=(.+)/);
        if (matches === null)
            matches = fileData.headers("content-disposition").match(/filename\*=.+\'\'(.+)/);
        if (matches === null)
            matches = fileData.headers("content-disposition").match(/filename\*=(.+)/);
        if (matches !== null)
            fileName = decodeURIComponent(matches[1]);

        //FileSaver.js
        saveAs(blob, fileName);
    }
});