HaselApp.controller('CustomerListController', ['$scope', '$http', '$timeout', "CustomerService", '$q', function CustomerController($scope, $http, $timeout, CustomerService, $q) {
    $scope.CustomerService = CustomerService;

    $scope.IsSaving = false;
    $scope.IsEditPanelActive = false;
    $scope.CustomerFilter = angular.copy(CustomerFilter);
    $scope.SetEditPanel = function (expandStatus) {
        $scope.ToggleCollapsePanel("CustomerPanel", expandStatus);
        $scope.IsEditPanelActive = expandStatus;
    }
    $scope.DraftCustomers = null;
    $scope.DraftCustomerOptions = {
        DeferDataSource: $scope.CustomerService.GetCustomerOptions,
        onChange: function () {
            GetCustomers();
        },
        placeholder: "Seçiniz...",
        textField: 'Text',
        valueField: 'Value'
    };

    $scope.SectorList = new DevExpress.data.CustomStore({
        load: function (loadopt) {
            var defer = $q.defer();
            $scope.CustomerService.GetSectors(function (r) {
                defer.resolve(r.Data);
            });
            return defer.promise;
        },
        byKey: function (key, extra) {
            // . . .
        },
    });

    //GetCustomers();

    $scope.selectAllMode = "allPages";
    $scope.showCheckBoxesMode = "onClick";

    $scope.GetCustomer = function (id) {
        var filter = angular.copy(CustomerFilter);
        filter.Id = id;
        $scope.CustomerService.GetCustomer($scope.CustomerFilter).then(function (res) {
            $scope.CustomerList = res.Data[0];
            $scope.SetEditPanel(true);
            $scope.ScrollTo("CustomerPanel");
        });
    }

    function GetCustomers() {
        debugger;
        if (typeof $scope.dataGridOptions != 'undefined')
            $scope.GridService.Refresh('customerGrid');
        else
            $scope.dataGridOptions = {
                dataSourceUrl: '/Customer/Get',
                bindingOptions: {
                    "selection.selectAllMode": "selectAllMode",
                    "selection.showCheckBoxesMode": "showCheckBoxesMode",
                },
                selection: {
                    mode: "multiple"
                },
                dataSourceFilter: {
                    filter: $scope.CustomerFilter
                },
                columns: [
                 {
                    displayName: 'Link',
                    cellTemplate: 'cellTemplateCommand'
                },
                {
                      dataField: "Id",
                      caption: "No"
                  },
                {
                      dataField: "Name",
                      caption: "Ad"
                  },
                {
                      dataField: "ShortName",
                      caption: "Kısa Ad"
                  },
                {
                        dataField: "IsHasel",
                        caption: "Hasel/Rentlift"
                    },
                {
                        dataField: "TaxOffice",
                        caption: "Vergi Dairesi"
                    },
                {
                    dataField: "TaxNumber",
                    caption: "Vergi No"
                },
                {
                    dataField: "NetsisRentliftCode",
                    caption: "Rentlift Kodu"
                },
                {
                    dataField: "NetsisHaselCode",
                    caption: "Hasel Kodu"
                },
                {
                    dataField: "SectorId",
                    lookup: {
                        dataSource: $scope.SectorList,
                        displayExpr: "SectorName",
                        valueExpr: "Id"
                    },
                    caption: "Sektör"
                },
                {
                    dataField: "CreateDate",
                    caption: "Oluşturma Tarihi",
                    dataType: "date",
                    format: "dd.MM.yyyy"
                },
                {
                    dataField: "LocationName",
                    caption: "Lokasyon"
                }
                //,
                //{
                //    dataField: "AuthenticatorName",
                //    caption: "Authenticator"
                //}
                //,
                //{
                //    dataField: "SaleEngineeer",
                //    caption: "SaleEngineeer"
                //}
                
                ]
            };
    }



}]);