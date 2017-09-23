HaselApp.controller('CustomerMachineparkController', ['$scope', '$http', '$timeout', "$q", function CustomerMachineparkController($scope, $http, $timeout, $q) {
    $scope.IsSaving = false;
    $scope.IsEditPanelActive = false;
    $scope.selectAllMode = "allPages";
    $scope.showCheckBoxesMode = "onClick";
    /*Filters*/
    $scope.MachineparkFilter = angular.copy(MachineparkFilter);
    $scope.MachineModelFilter = angular.copy(MachineModelFilter);
    $scope.MachineparkMarkFilter = angular.copy(MachineparkMarkFilter);
    $scope.MachineparkCategoryFilter = angular.copy(MachineparkCategoryFilter);
    /*Filters*/
    /* Lookups */
    $scope.MpCategory = {};
    $scope.MpMark = {};
    $scope.MpModel = {};
    $scope.MpYear = {};
    $scope.MpLocation = {};
    $scope.MarkListLookupStore = new DevExpress.data.CustomStore({
        load: function (loadopt) {
            var defer = $q.defer();

            defer.resolve($scope.MachineparkMarkList);

            return defer.promise;
        },
        byKey: function (key, extra) {
        }
    });
    $scope.ModelListLookupStore = new DevExpress.data.CustomStore({
        load: function (loadopt) {
            var defer = $q.defer();

            defer.resolve($scope.MpModelList);

            return defer.promise;
        },
        byKey: function (key, extra) {
        }
    });
    /* Lookups */
    /*Methods*/
    $scope.SetEditPanel = function (expandStatus) {
        $scope.ToggleCollapsePanel("MachineParkPanel", expandStatus);
        $scope.IsEditPanelActive = expandStatus;
    }
    $scope.ResetMachinepark = function () {
        $scope.CheckAll = true;
        $scope.CustomerMachinepark = angular.copy(CustomerMachineparkModel);
        $scope.CustomerMachinepark.CustomerId = $scope.GetCustomerId();
    }
    $scope.GetMachinepark = function (id) {
        var filter = angular.copy(MachineparkFilter);
        filter.Id = id;
        filter.CustomerId = $scope.GetCustomerId();
        $scope.MachineparkService.GetMachinepark(filter).then(function (res) {
            res.Data[0].SaleDate = res.Data[0].SaleDate == null ? null : new Date(res.Data[0].SaleDate);
            res.Data[0].ReleaseDate = res.Data[0].ReleaseDate == null ? null : new Date(res.Data[0].ReleaseDate);
            $scope.CustomerMachinepark = res.Data[0];

            $scope.SetEditPanel(true);
            $scope.ScrollTo("MachineParkPanel");
        });
    }
    $scope.SaveMachinepark = function () {
        $scope.IsSaving = true;
        $scope.MachineparkService.SaveMachinepark($scope.CustomerMachinepark).then(function (res) {
            if (res.IsSuccess) {
                $scope.AlertService.Success();
                $scope.GridService.Refresh("machineparkGrid");
            } else
                $scope.AlertService.Error(res.Message);
            $scope.IsSaving = false;
        });
    }
    $scope.DeleteMachinepark = function (id) {
        $scope.AlertService.Confirm("Seçilen makine parklarını silmek istediğinize emin misiniz?", null, function (res) {
            if (res) {
                $scope.IsSaving = true;
                var mpIds = [];
                if (typeof id !== 'undefined')
                    mpIds.push(id);
                else
                    mpIds = $scope.dataGridOptions.SelectedRows.map(function (a) { return a.Id; });

                $scope.MachineparkService.DeleteMachinepark(mpIds).then(function (res) {
                    $scope.IsSaving = false;
                    if (res.IsSuccess) {
                        $scope.AlertService.Success();
                        $scope.ResetMachinepark();
                        $scope.GridService.Refresh("machineparkGrid");
                    } else
                        $scope.AlertService.Error(res.Message);
                });
            }
        });
    }
    $scope.ReleaseMachinePark = function () {
        var mpIds = $scope.dataGridOptions.SelectedRows.map(function (a) { return a.Id; });
        var data = {
            Title : 'Elden Çıkarma Modülü',
            Ids : mpIds,
            ReleaseDate : new Date()
        };
        $scope.MachineparkService.OpenReleasePanel(data).then(function(res){
            if(res.IsSuccess) $scope.AlertService.Success();
            else $scope.AlertService.Error("res.Message");
            GetMachineParks();
        });
    }
    $scope.RentMachineParks = function () {
        var mpIds = $scope.dataGridOptions.SelectedRows.map(function (a) { return a.Id; });
    }
    $scope.SaveMpModel = function () {
        if ($scope.MpCategory == null || $scope.MpMark == null) {
            $scope.AlertService.Error("Yeni model kaydı için kategori ve marka alanı gereklidir.");
            return false;
        }
        var name = $scope.MpModel_SelectOptions.SearchText;

        var catName = $scope.MpCategory.CategoryName;
        var markName = $scope.MpMark.MarkName;
        $scope.AlertService.Confirm("Kategori: " + catName + ", Marka: " + markName + " için; " + name + " isimli model kaydedilecek. Onaylıyor musunuz?", null, function (res) {
            if (res) {
                $scope.NewMpModel = angular.copy(MachineModelModel);
                $scope.NewMpModel.Name = name;
                $scope.NewMpModel.CategoryId = $scope.MpCategory.Id;
                $scope.NewMpModel.MarkId = $scope.MpMark.Id;
                $scope.MachineparkService.SaveMachineModel($scope.NewMpModel).then(function (res) {
                    if (res.IsSuccess) {
                        $scope.MpModel_SelectOptions.data.unshift(res.Data);
                        $scope.MpModelList.unshift(res.Data);
                        $scope.CustomerMachinepark.ModelId = res.Data.Id;
                        $scope.GridService.Refresh("machineparkGrid");
                    }
                    else
                        $scope.AlertService.Error(res.Message);
                });
            }
        });
    }
    $scope.SaveMpMark = function () {
        var markName = $scope.MpMark_SelectOptions.SearchText;
        $scope.AlertService.Confirm(markName + " isimli marka kaydedilecek. Onaylıyor musunuz?", null, function (res) {
            if (res) {
                $scope.NewMpMark = angular.copy(MachineparkMarkModel);
                $scope.NewMpMark.MarkName = markName;
                $scope.MachineparkService.SaveMachineparkMark($scope.NewMpMark).then(function (res) {
                    if (res.IsSuccess) {
                        $scope.MpMark_SelectOptions.data.unshift(res.Data);
                        $scope.CustomerMachinepark.MarkId = res.Data.Id;
                        $scope.GridService.Refresh("machineparkGrid");
                    }
                    else
                        $scope.AlertService.Error(res.Message);
                });
            }
        });
    }
    /*Methods*/

    /*Events*/
    $scope.btnMachineparkNew_Click = function () {
        $scope.ResetMachinepark();
    }
    $scope.$on('initMachineparkTab', function (e) {
        $scope.MachineparkCategoryFilter.CustomerId = $scope.GetCustomerId();
        $scope.MachineparkFilter.CustomerId = $scope.GetCustomerId();
        $scope.ResetMachinepark();
        GetMachineParks();
        initSelectOptions();
        $("input").rules("remove"); //Angular validasyonla aynı anda çalıştığı için kuralları silindi.
    });
    $scope.$watchCollection("dataGridOptions.SelectedRows", function () {
        $scope.ResetMachinepark();
        $scope.SetEditPanel(false);
    });
    $scope.$watch('MpModel_SelectOptions.SearchText', function (res) {
        $scope.MpModel_SelectOptions.DisableAdd = res == "";
    });
    $scope.$watch('MpMark_SelectOptions.SearchText', function (res) {
        $scope.MpMark_SelectOptions.DisableAdd = res == "";
    });
    /*Events*/

    /*Independed Funcs*/
    function GetMachineParks() {
        if (typeof $scope.dataGridOptions !== 'undefined')
            $scope.GridService.Refresh('machineparkGrid');
        else
            $scope.dataGridOptions = {
                dataSourceUrl: '/Machinepark/Get',
                bindingOptions: {
                    "selection.selectAllMode": "selectAllMode",
                    "selection.showCheckBoxesMode": "showCheckBoxesMode",
                },
                selection: {
                    mode: "multiple"
                },
                dataSourceFilter: {
                    filter: $scope.MachineparkFilter
                },
                onDataSourceBound: function (pack) {
                    $scope.$parent.CustomerStats.Machinepark = pack.Data.length;
                },
                onContextMenuPreparing: function (e) {
                    if (e.row.rowType === "data") {
                        e.items = [
                            {
                                icon: 'fa fa-edit',
                                text: "Düzenle",
                                onItemClick: function (s) {
                                    $scope.GetMachinepark(e.row.data.Id);
                                }
                            },
                            {
                                icon: 'fa fa-trash-o',
                                text: "Sil",
                                onItemClick: function (s) {
                                    $scope.DeleteMachinepark(e.row.data.Id);
                                }
                            }

                        ];
                    }
                },
                columns: [
                   {
                       dataField: "CategoryId",
                       caption: "Tür",
                       lookup: {
                           dataSource: $scope.MachineParkCategoryList,
                           displayExpr: "CategoryName",
                           valueExpr: "Id"
                       }
                   },
                {
                    caption: "Marka",
                    dataField: "MarkId",
                    lookup: {
                        dataSource: $scope.MarkListLookupStore,
                        displayExpr: "MarkName",
                        valueExpr: "Id"
                    }
                },
                {
                    dataField: "ModelId",
                    caption: "Model",
                    lookup: {
                        dataSource: $scope.ModelListLookupStore,
                        displayExpr: "Name",
                        valueExpr: "Id"
                    }
                },
                {
                    dataField: "LocationId",
                    lookup: {
                        dataSource: $scope.CustomerLocations,
                        displayExpr: "Name",
                        valueExpr: "Id"
                    },
                    caption: "Konum"
                },
                  {
                      dataField: "SerialNo",
                      caption: "Seri Numarası"
                  },
                    {
                        dataField: "Quantity",
                        caption: "Adet"
                    },
                {
                    dataField: "SaleDate",
                    caption: "Baş.",
                    dataType: "date",
                    format: "dd.MM.yyyy"
                }]
            };
    }
    function initSelectOptions() {
        $scope.MpModel_SelectOptions.AllowAdding = true;
        $scope.MpMark_SelectOptions.AllowAdding = true;

        $scope.MpModel_SelectOptions.BindingParams = $scope.MachineModelFilter;
        $scope.MPCategory_SelectOptions.BindingParams = $scope.MachineparkCategoryFilter;
        $scope.MpMark_SelectOptions.data = $scope.MachineparkMarkList;
        $scope.MPCategory_SelectOptions.GetData();
        $scope.MpMark_SelectOptions.onChange = function () {
            $scope.MachineModelFilter.MarkId = $scope.MpMark != null ? $scope.MpMark.Id : null;
            $scope.MpModel = null;

            $timeout(function () {
                $scope.MpModel_SelectOptions.GetData();
            });
        };

        $scope.MPCategory_SelectOptions.onChange = function () {
            $scope.MachineModelFilter.CategoryId = ($scope.MpCategory != null) ? $scope.MpCategory.Id : null;
            $scope.MpModel = null;
            $timeout(function () {
                $scope.MpModel_SelectOptions.GetData();
            });
        }

        $scope.MpModel_SelectOptions.onDataBound = function () {
            $scope.MpModel = $scope.MpModel_SelectOptions.GetModel($scope.CustomerMachinepark.ModelId);
            if ($scope.MpModel == null)
                $scope.CustomerMachinepark.ModelId = null;
        }
    }
    /*Independed Funcs*/
}]);