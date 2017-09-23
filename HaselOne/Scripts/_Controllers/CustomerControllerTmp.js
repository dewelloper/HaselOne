HaselApp.controller('CustomerMachineparkController', ['$scope', '$http', '$timeout', "$q", '$compile', function CustomerMachineparkController($scope, $http, $timeout, $q, $compile) {
    $scope.IsSaving = false;
    $scope.IsActiveTab = true;
    $scope.selectAllMode = "page";
    $scope.showCheckBoxesMode = "onClick";
    
    /*Filters*/
    $scope.MachineparkFilter = angular.copy(MachineparkFilter);
    $scope.MachineparkFilter.IsReleased = false;
    $scope.PassiveMachineparkFilter = angular.copy(MachineparkFilter);
    $scope.PassiveMachineparkFilter.IsReleased = true;
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
        $scope.CustomerMachinepark = angular.copy(CustomerMachineparkModel);
        $scope.CustomerMachinepark.CustomerId = $scope.GetCustomerId();
       
    }
    $scope.GetMachinepark = function (id) {
        var filter = angular.copy(MachineparkFilter);
        filter.Id = id;
        filter.CustomerId = $scope.GetCustomerId();
        filter.IsReleased = !$scope.IsActiveTab;
        $scope.MachineparkService.GetMachinepark(filter).then(function (res) {
            res.Data[0].SaleDate = res.Data[0].SaleDate == null ? null : new Date(res.Data[0].SaleDate);
            res.Data[0].ReleaseDate = res.Data[0].ReleaseDate == null ? null : new Date(res.Data[0].ReleaseDate);
            $scope.CustomerMachinepark = res.Data[0];

            if ($scope.CustomerMachinepark.HasRequest && $scope.GetCookie("IsCustomerRequestMessageRead") == null) {
                $scope.MachineparkpanelResult = {
                    ResultType: 'Info',
                    Message: 'Talepten gelen makine parkları yalnızca talep modülünden düzenlenebilir.'
                };
                $scope.SetCookie("IsCustomerRequestMessageRead", true) != null
            }
            $scope.SetEditPanel(true);
            $scope.ScrollTo("MachineParkPanel");
          
        });
    }

    $scope.SaveMachinepark = function () {
        $scope.IsSaving = true;
        $scope.MachineparkService.SaveMachinepark($scope.CustomerMachinepark).then(function (res) {
            $scope.MachineparkpanelResult = res;
            if (res.IsSuccess && res.IsValid) {
                //$scope.SetEditPanel(false);
                $timeout(function () {
                    $scope.ResetMachinepark();
                }, 200);

                if ($scope.IsActiveTab)
                    GetMachineParks();
                else
                    GetPassiveMachineparks();
            }
            $scope.IsSaving = false;
        });
    }
    $scope.DeleteMachinepark = function (id) {
        var ids = $scope.IsActiveTab ? $scope.dataGridOptions.SelectedRows.map(function (a) { return a.Id; })
                                     : $scope.dataGridOptionsForPassives.SelectedRows.map(function (a) { return a.Id; });;
        $scope.AlertService.Confirm("Seçilen makine parklarını silmek istediğinize emin misiniz?", null, function (res) {
            if (res) {
                $scope.IsSaving = true;
                var mpIds = [];
                if (typeof id !== 'undefined')
                    mpIds.push(id);
                else
                    mpIds = ids;

                $scope.MachineparkService.DeleteMachinepark(mpIds).then(function (res) {
                    $scope.IsSaving = false;
                    $scope.MachineparkpanelResult = res;
                    if (res.IsSuccess && res.IsValid) {
                        $scope.ResetMachinepark();
                        if ($scope.IsActiveTab)
                            GetMachineParks();
                        else
                            GetPassiveMachineparks();
                    }
                });
            }
        });
    }

    $scope.BoundFn = null;
    $scope.ReleaseMachinePark = function () {
        var mpIds = $scope.dataGridOptions.SelectedRows.map(function (a) { return a.Id; });
        var data = {
            Title: 'Elden Çıkarma Modülü',
            Ids: mpIds,
            ReleaseDate: new Date()
        };
        $scope.MachineparkService.OpenReleasePanel(data).then(function (res) {
            $scope.MachineparkpanelResult = res;
            GetMachineParks(function () {
                GetPassiveMachineparks();
                $scope.ShowTab("machineparkTab_passiveMp");
                $scope.IsActiveTab = false;
            });
        });
    }

    $scope.UnreleaseMachinePark = function () {
        var mpIds = $scope.dataGridOptionsForPassives.SelectedRows.map(function (a) { return a.Id; });
        var confirmText = "Seçili makine parklarının elden çıkarma işlemi geri alınacaktır. Bu işlemi yapmak istediğinize emin misiniz?";

        $scope.AlertService.Confirm(confirmText, null, function (res) {
            if (res) {
                $scope.MachineparkService.ReleaseMachinepark(mpIds, null)
                .then(function (res) {
                    $scope.MachineparkpanelResult = res;

                    GetPassiveMachineparks(function () {
                        GetMachineParks();
                        $scope.ShowTab("machineparkTab_activeMp");
                        $scope.IsActiveTab = true;
                    });
                });
            }
        });
    }

    $scope.CopyMachinepark = function (mid) {
        if ($scope.dataGridOptions.SelectedRow != null) {
            var id = typeof mid === 'undefined' ? $scope.dataGridOptions.SelectedRow.Id : mid;

            var customerId = $scope.GetCustomerId();
            var options = {
                input: 'number',
                confirmButtonText: 'Onay',
                showCancelButton: true,
                showLoaderOnConfirm: true,
                preConfirm: function (number) {
                    return new Promise(function (resolve, reject) {
                        if (number === 0)
                            reject("Girilen sayı 0'dan büyük olmak zorundadır.");
                        if (number > 50)
                            reject("Girilen sayı 50'den  büyük olamaz.");
                        else
                            resolve();
                    })
                }
            };
            $scope.AlertService.Show(
                'Kopyalamak istediğiniz makine miktarını giriniz.',
                'Kopyala',
                function (res) {
                    if (res) {
                        $scope.MachineparkService.CopyMachinepark(id, customerId, res).then(function (res) {
                            $scope.MachineparkpanelResult = res;
                            if (res.IsSuccess && res.IsValid) {
                                GetMachineParks();
                            }
                        });
                    }
                }, options);
        }
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
                    $scope.MachineparkpanelResult = res;
                    if (res.IsSuccess && res.IsValid) {
                        $scope.MpModel_SelectOptions.data.unshift(res.Data);
                        $scope.MpModelList.unshift(res.Data);
                        $scope.CustomerMachinepark.ModelId = res.Data.Id;
                        if ($scope.IsActiveTab)
                            GetMachineParks();
                        else
                            GetPassiveMachineparks();
                    }
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
                    $scope.MachineparkpanelResult = res;
                    if (res.IsSuccess && res.IsValid) {
                        $scope.MpMark_SelectOptions.data.unshift(res.Data);
                        $scope.CustomerMachinepark.MarkId = res.Data.Id;
                        if ($scope.IsActiveTab)
                            GetMachineParks();
                        else
                            GetPassiveMachineparks();
                    }
                });
            }
        });
    }
    $scope.GoToRequest = function (id) {
        $scope.ShowTab("tab_Request");
        $scope.initTab('initRequestTab')
    }

    $scope.CloseEditPanel = function () {
        //if ($scope.IsMachineparkModelChanged && !$scope.CustomerMachinepark.HasRequest)
        //    $scope.AlertService.Confirm("Düzenleme paneli kapatılacak ve yaptığınız değişiklikler varsa iptal edilecektir. Onaylıyor musunuz?", null, function (res) {
        //        if (res) {
        //            $scope.SetEditPanel(!$scope.IsEditPanelActive);
        //            $scope.ResetMachinepark();
                  
        //        }
        //    });
        //else {
            $scope.SetEditPanel(!$scope.IsEditPanelActive);
            $scope.ResetMachinepark();
       // }
        
    }
    /*Methods*/

    /*Events*/
    $scope.btnMachineparkNew_Click = function () {
        $scope.ResetMachinepark();
    }

    $scope.$on('initMachineparkTab', function (e) {
        $scope.MachineparkCategoryFilter.CustomerId = $scope.GetCustomerId();
        $scope.PassiveMachineparkFilter.CustomerId = $scope.GetCustomerId();
        $scope.MachineparkFilter.CustomerId = $scope.GetCustomerId();
        $scope.ResetMachinepark();
        GetMachineParks(function () {
            GetPassiveMachineparks();
        });

        initSelectOptions();
    });

    $scope.$watchCollection("dataGridOptions.SelectedRows", function () {
        $scope.ResetMachinepark();
        $scope.SetEditPanel(false);
    });
    $scope.$watchCollection("dataGridOptionsForPassives.SelectedRows", function () {
        $scope.ResetMachinepark();
        $scope.SetEditPanel(false);
    });
    $scope.$watch('MpModel_SelectOptions.SearchText', function (res) {
        $scope.MpModel_SelectOptions.DisableAdd = res == "";
    });
    $scope.$watch('MpMark_SelectOptions.SearchText', function (res) {
        $scope.MpMark_SelectOptions.DisableAdd = res == "";
    });

    var refreshTimer = true;
    $scope.$watch('IsActiveTab', function (res) {
        // $scope.MachineparkFilter.IsReleased = !res;
        $scope.SetEditPanel(false);
        if (refreshTimer)
            $timeout.cancel(refreshTimer);
        refreshTimer = $timeout(function () {
            var id = res ? "machineparkGrid" : "machineparkGridPassive";
            $scope.GridService.Refresh(id);
        }, 800);
    });
   
    /*Events*/

    /*Independed Funcs*/
    var defaultMpColumns = [
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
                    dataField: "MarkName"
                },
                {
                    dataField: "ModelName",
                    caption: "Model",
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
                        caption: "Adet",
                        width: 50
                    },
                {
                    dataField: "SaleDate",
                    caption: "Alış Tar.",
                    dataType: "date",
                    format: "dd.MM.yyyy"
                }

    ];

    function GetMachineParks(boundFn) {
        $scope.BoundFn = boundFn;
        // $scope.MachineparkFilter.IsReleased = false;
        if (typeof $scope.dataGridOptions !== 'undefined')
            $scope.GridService.Refresh('machineparkGrid');
        else {
            var mpColums = angular.copy(defaultMpColumns);
            mpColums.push({
                dataField: "HasRequest",
                caption: "Talep",
                dataType: 'boolean',
                cellTemplate: "cellTemplateHasRequest",
                width: 70
            });
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
                    $scope.$parent.CustomerStats.ActiveMachinepark = pack.Data.length;
                    if (typeof $scope.BoundFn === 'function')
                        $scope.BoundFn();
                },
                onContextMenuPreparing: function (e) {
                    if (e.target === 'header') {
                        e.items.unshift({
                            beginGroup: true,
                            icon: 'fa fa-refresh',
                            text: "Yenile",
                            onItemClick: function (s) {
                                $scope.GridService.Refresh('machineparkGrid')
                            }
                        });
                        e.items.unshift({
                            icon: 'fa fa-plus',
                            text: "Yeni Makine Ekle",
                            onItemClick: function (s) {
                                $scope.SetEditPanel(!$scope.IsEditPanelActive);
                                $scope.btnMachineparkNew_Click();
                            }
                        });
                    }
                    if (e.row.rowType === "data") {
                        $scope.GridService.SelectRow('machineparkGrid', $scope.dataGridOptions.SelectedRows, e.row.data, 'Id');
                        e.items = [
                            {
                                icon: 'fa fa-pencil',
                                text: "Düzenle",
                                onItemClick: function (s) {
                                    $scope.GridService.SelectRow('machineparkGrid', [], e.row.data, 'Id');
                                    $scope.GetMachinepark(e.row.data.Id);
                                }
                            }

                        ];
                        if (!e.row.data.HasRequest) {
                            e.items.push({
                                icon: 'fa fa-copy',
                                text: "Kopyala",
                                onItemClick: function (s) {
                                    $scope.GridService.SelectRow('machineparkGrid', [], e.row.data, 'Id');
                                    $scope.CopyMachinepark(e.row.data.Id);
                                }
                            });
                            e.items.push({
                                icon: 'fa fa-trash-o',
                                text: "Sil",
                                onItemClick: function (s) {
                                    $scope.DeleteMachinepark();
                                }
                            });
                        }
                        e.items.push({
                            beginGroup: true,
                            icon: 'fa fa-share-square-o',
                            text: "Elden Çıkar",
                            onItemClick: function (s) {
                                $scope.ReleaseMachinePark();
                            }
                        });
                        if (e.row.data.HasRequest) {
                            {
                                e.items.push({
                                    icon: 'fa fa-tumblr',
                                    text: "Talebe Git",
                                    onItemClick: function (s) {
                                        $scope.GoToRequest(e.row.data.Id);
                                    }
                                });
                            }
                        }
                    }
                },
                onEditorPrepared: function (info) {
                    if (info.parentType == 'filterRow' && info.dataField == "HasRequest" && info.editorName == "dxSelectBox") {
                        info.trueText = "Var";
                        info.falseText = "Yok";
                    }
                },
                columns: mpColums
            };
        }
    }
    function GetPassiveMachineparks(boundFn) {
        $scope.BoundFn = boundFn;
        // $scope.MachineparkFilter.IsReleased = true;
        //Önemli: Önce normal kayıtları kur. optionları oradan kopyala.
        if (typeof $scope.dataGridOptionsForPassives !== 'undefined')
            $scope.GridService.Refresh('machineparkGridPassive');
        else {
            var mpColums = angular.copy(defaultMpColumns);
            mpColums.push({
                dataField: "ReleaseDate",
                caption: "Satış Tar.",
                dataType: "date",
                format: "dd.MM.yyyy"
            })
            mpColums.push(
                        {
                            dataField: "HasRequest",
                            caption: "Talep",
                            dataType: 'boolean',
                            cellTemplate: "cellTemplateHasRequest",
                            width: 70
                        });

            $scope.dataGridOptionsForPassives = {
                dataSourceUrl: '/Machinepark/Get',
                bindingOptions: {
                    "selection.selectAllMode": "selectAllMode",
                    "selection.showCheckBoxesMode": "showCheckBoxesMode",
                },
                selection: {
                    mode: "multiple"
                },
                dataSourceFilter: {
                    filter: $scope.PassiveMachineparkFilter
                },
                onDataSourceBound: function (pack) {
                    $scope.$parent.CustomerStats.PassiveMachinepark = pack.Data.length;
                    if (typeof $scope.BoundFn === 'function')
                        $scope.BoundFn();
                },
                onContextMenuPreparing: function (e) {
                    if (e.target === 'header') {
                        e.items.unshift({
                            beginGroup: true,
                            icon: 'fa fa-refresh',
                            text: "Yenile",
                            onItemClick: function (s) {
                                $scope.GridService.Refresh('machineparkGridPassive')
                            }
                        });
                    }
                    if (e.row.rowType === "data") {
                        $scope.GridService.SelectRow('machineparkGridPassive', $scope.dataGridOptionsForPassives.SelectedRows, e.row.data, 'Id');
                        e.items = [
                            {
                                icon: 'fa fa-pencil',
                                text: "Düzenle",
                                onItemClick: function (s) {
                                    $scope.GridService.SelectRow('machineparkGridPassive', [], e.row.data, 'Id');
                                    $scope.GetMachinepark(e.row.data.Id);
                                }
                            }
                        ];
                        if (!e.row.data.HasRequest) {
                            e.items.push({
                                icon: 'fa fa-trash-o',
                                text: "Sil",
                                onItemClick: function (s) {
                                    $scope.DeleteMachinepark();
                                }
                            });
                        }
                        e.items.push({
                            beginGroup: true,
                            icon: 'fa fa-reply',
                            text: "Geri Al",
                            onItemClick: function (s) {
                                $scope.UnreleaseMachinePark();
                            }
                        });

                        if (e.row.data.HasRequest) {
                            {
                                e.items.push({
                                    icon: 'fa fa-tumblr',
                                    text: "Talebe Git",
                                    onItemClick: function (s) {
                                        $scope.GoToRequest(e.row.data.Id);
                                    }
                                });
                            }
                        }
                    }
                },
                onEditorPrepared: function (info) {
                    if (info.parentType == 'filterRow' && info.dataField == "HasRequest" && info.editorName == "dxSelectBox") {
                        info.trueText = "Var";
                        info.falseText = "Yok";
                    }
                },
                columns: mpColums
            };
        }
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