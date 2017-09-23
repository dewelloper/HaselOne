HaselApp.controller('CustomerRequestController', ['$scope', '$http', '$timeout', "CustomerService", '$q',
    function CustomerRequestController($scope, $http, $timeout, CustomerService, $q) {
        $scope.IsEditPanelActive = true;
        ///deneme
        $scope.GetCustomerId = function() {
            return $scope.getQueryStringByName("cid") == "" ? 0 : $scope.getQueryStringByName("cid");
        }

        $scope.IsSaveBtnActive = false;

        $scope.SetEditPanel = function(expandStatus) {
            $scope.ToggleCollapsePanel("RequestPanel", expandStatus);
            $scope.IsEditPanelActive = expandStatus;
        }
        CustomerRequestModel.RequestDate = new Date();
        $scope.CustomerRequest = angular.copy(CustomerRequestModel);
        $scope.RequestMpWrapper = {};
        $scope.RequestMpWrapperList = []; 

        $scope.RentModelDisableReadonly = false;
        $scope.$watch('CustomerRequest.SalesType',
            function(p) {
                if (typeof p !== "undefined") {
                    if (p == 2) {
                        //alert("Kiralik modulu aktif degildir"); -- kiralik girilsin fakat sonlanmasin istenildi.
                        //$scope.CustomerRequest.SalesType = 1;
                        $scope.RentModelDisableReadonly = true;

                    } else {
                        $scope.RentModelDisableReadonly = false;
                        $scope.CustomerRequest.UseDurationUnit = 0;
                        $scope.CustomerRequest.UseDuration = null;
                    }
                }
            });

        //#EndRegion

        $scope.MachineModelFilter = angular.copy(MachineModelFilter);
        $scope.MachineModelFilterForMpList = angular.copy(MachineModelFilter);

        $scope.MachineModelForMpList = {};

        $scope.MachineparkMarkFilter = angular.copy(MachineparkMarkFilter);
        $scope.MachineparkCategoryFilter = angular.copy(MachineparkCategoryFilter);

        $scope.MpCategory = {};
        $scope.MpMark = {};
        $scope.MpLocation = {};
        $scope.MpModel = {};
        $scope.MpYear = {};
        $scope.SalesType = {};

        $scope.SalesTypeList = SalesTypeList;
        $scope.SalesType_SelectOptions = {
            DataSource: $scope.SalesTypeList,
            placeholder: "Seçiniz",
            textField: 'Text',
            valueField: "Value"
        };

        $scope.UseDurationUnit = {};
        $scope.UseDurationUnitList = UseDurationUnitList;
        $scope.UseDurationUnit_SelectOptions = {
            DataSource: $scope.UseDurationUnitList,
            placeholder: "Seçiniz",
            textField: 'Text',
            valueField: "Value"
        };

        $scope.ChannelModel = {};
        $scope.ChannelList = ChannelList;
        $scope.Channel_SelectOptions = {
            DataSource: $scope.ChannelList,
            placeholder: "Seçiniz",
            textField: 'Text',
            valueField: "Value"
        };

        $scope.OwnerModel = {};
        $scope.OwnerList = OwnerList;
        $scope.Owner_SelectOptions = {
            DataSource: $scope.OwnerList,
            placeholder: "Seçiniz",
            textField: 'Text',
            valueField: "Value"
        };

        $scope.SalesmanModel = {};
        $scope.SalesmanFilter = angular.copy(SalesmanFilter);
        $scope.SalesmanFilter.CustomerId = $scope.GetCustomerId();
        $scope.SalesmanList_SelectOptions = {
            DeferDataSource: $scope.CustomerService.GetSalesmanList,
            BindingParams: $scope.SalesmanFilter,
            placeholder: "Seçiniz",
            textField: 'Text',
            valueField: "Value",
            BindOnLoad: false
        };

        $scope.ResultType = {};
        $scope.ResultTypeList = ResultTypeList;
        $scope.ResultTypeList_SelectOptions = {
            DataSource: $scope.ResultTypeList,
            placeholder: "Seçiniz",
            textField: 'Text',
            valueField: "Value"
        };

        $scope.ConditionType = {};
        $scope.ConditionTypeList = ConditionTypeList;
        $scope.ConditionTypeList_SelectOptions = {
            DataSource: $scope.ConditionTypeList,
            placeholder: "Seçiniz",
            textField: 'Text',
            valueField: "Value"
        };

        //$("[id$='spRequestCount']")[0].innerHTML = result.Data.Length;
        $scope.SetCustomerId = function() {
            $scope.CustomerRequest.CustomerId = $scope.GetCustomerId();
        }

        $scope.ResetCustomer = function() {
            $scope.CustomerRequest = angular.copy(CustomerRequestModel);
        }

        $scope.SetCustomerId();
        $scope.CustomerRequestFilter = angular.copy(CustomerRequestFilter);
        $scope.CustomerRequestFilter.CustomerId = $scope.CustomerRequest.CustomerId;

        //#region Events
        $scope.RequestSave = function() {
            $scope.AjaxPost('/CustomerRequest/Save', { vm: $scope.CustomerRequest }).then(function(a) {

                $scope.RequestResult = a;
                if (a.IsSuccess && a.IsValid) {
                    $scope.ResetCustomer();
                    $scope.SetCustomerId();
                    $scope.CustomerRequestFilter.Id = 0;
                    $scope.RequestGridRefresh();
                    ToggRequesters();
                    $scope.MachineParkGridPanelShow = false;
                }
            });
        };
        $scope.RequestGridRefresh = function() {
            $scope.CustomerRequestFilter.Id = 0;
            $scope.GridService.Refresh("customerRequestGrid_Open");
            $scope.GridService.Refresh("customerRequestGrid_Close");
        }
        $scope.RequestGridRefresh_Close = function() {
            $scope.GridService.Refresh("customerRequestGrid_Close");
        }
        $scope.RequestGridRefresh_Open = function() {
            $scope.GridService.Refresh("customerRequestGrid_Open");
        }
        $scope.MachineParkGridRefresh = function() {
            $scope.MachineParkGridPanelShow = true;
            $scope.GridService.Refresh("customerRequestMachineParkGrid");
        }
        $scope.RequestGridsResfresh = function() {
            $scope.RequestGridRefresh();
            $scope.MachineParkGridRefresh();
        }

        $scope.FormModeEngine = function(tabId, formMode) {
            var disable;
            if (FormMode == "readonly") {
                disable = true;
            } else {
                disable = false;
            }

            $("#" + tabId).find("input").each(function() {
                //select-search de calismiyor.
                $(this).prop('disabled', disable);
            });
        }

        $scope.QuantityIsReadonlyControl = function(customerRequest) {

            $scope.QuantityIsReadonly = false;
            if (typeof customerRequest != "undefined" && customerRequest != null) {
                if (customerRequest.ResultType == 2 || customerRequest.ResultType == 1) {
                    $scope.QuantityIsReadonly = true;
                }
            }
        }

        $scope.getCustomerRequest = function(rowId) {

            $scope.CustomerRequestFilter.Id = rowId;
            CustomerService.GetRequest($scope.CustomerRequestFilter).then(function(pack) {
                if (pack.IsSuccess) {
                    ToggRequesters_Everyopen();

                    pack.Data.EstimatedBuyDate = new Date(pack.Data.EstimatedBuyDate);
                    pack.Data.RequestDate = new Date(pack.Data.RequestDate);
                    $scope.CustomerRequest = pack.Data;

                    $scope.QuantityIsReadonlyControl($scope.CustomerRequest);

                    // if ($scope.CustomerRequest.FormMode == "readonly") {
                    //    // $scope.FormModeEngine("RequestForm", "readonly");
                    // }

                    $scope.ScrollTo("RequestPanel");
                }
            });
        }

        $scope.changeResultType = function(rowId, e) {

            $scope.ConfirmModal({
                Title: "Onay",
                Message: "Kaydı sonlandır",
                Button1Text: "Satış",
                Button2Text: "Kayıp Satış"
            }).then(function(res) {
                //{1: "Satış", 2: "Kayıp Satış", 3: "Bekliyor"}
                var resultyType = 0;
                if (res == true) {
                    resultyType = 1;
                } else {
                    resultyType = 2;
                }
                $scope.AjaxPost('/CustomerRequest/ChangeResultType', { requestId: rowId, resultType: resultyType }).then(function(a) {
                    $scope.RequestResult = a;

                    if (a.IsSuccess && a.IsValid) {
                        $scope.SetCustomerId();
                        $scope.RequestGridsResfresh();
                        dataGridOptionMachinePark($scope.RightClickSelectId);
                        $scope.ShowTab("tabCloseRequest");
                        debugger;
                        //GridSelectRow("customerRequestGrid_Close", e);
                        //$scope.GridService.SelectRow('customerRequestGrid_Close', [], e.row.data, 'Id');

                    }
                });
            });


        }

        $scope.SelectRequestId = 0;
        $scope.showRequestResult = function(rowId) {
            $scope.SelectRequestId = rowId;
            dataGridOptionMachinePark(rowId);

            $scope.ScrollTo("RequestForm");
            //  $scope.SetEditPanel(true);
        }

        $scope.QuantityIsReadonly = "";
        $scope.btnRequestNew_Click = function() {
            $scope.QuantityIsReadonly = "";
            $scope.ResetCustomer();
            $scope.SetCustomerId();
            ToggRequesters_Everyopen();
        };

        //#endregion

        //#region Factory
        $scope.GridGen = function() {
            dataGridOption();
        }

        $scope.$on('initRequestTab', function(e) {
            $scope.MachineparkCategoryFilter.CustomerId = $scope.GetCustomerId();
            $scope.GridGen();
            initSelectOptions();
        });

        // function dataGridOptionsSelectRowWatch(model) {
        //     $scope.$watchCollection(model, function(e) {
        //         if (typeof e != "undefined") {
        //             $scope.dontExitsCreate = false;
        //             if (e.length > 0)
        //                 if (e[0].ResultType != "undefined") {
        //                     if (e[0].ResultType != 3) {
        //                         $scope.SelectRequestId = e[0].Id;
        //                         dataGridOptionMachinePark(e[0].Id);
        //                         $scope.ScrollTo("RequestMachineParkPanel");
        //                     } else {//acik taleplerin makineleri yoktur.
        //                         dataGridOptionMachinePark(e[0].Id);
        //                     }
        //                 }

        //         }
        //     });
        // }

        //dataGridOptionsSelectRowWatch("dataGridOptions_Open.SelectedRows");
        //dataGridOptionsSelectRowWatch("dataGridOptions_Close.SelectedRows");

        $scope.getPreMachineParkGrid = function(e) {
            $scope.SelectRequestId = $scope.RightClickSelectId;
            $scope.dontExitsCreate = false;
            dataGridOptionMachinePark($scope.RightClickSelectId);
            $scope.ScrollTo("PreRequestMachineParkPanel");
        }
        $scope.btnRefresh_Click = function() {
            $scope.getCustomerRequest($scope.customerRequestFilter.Id);
        }
        $scope.RightClickSelectId = 0;

        var g_bindingOptions = {
            "selection.selectAllMode": "selectAllMode",
            "selection.showCheckBoxesMode": "showCheckBoxesMode"
        };
        var g_selection = {
            mode: "multiple"
        };
        
        var g_columns = [
            //{
            //    dataField: "UpdateDate",
            //    caption: "Guncelleme",
            //    dataType: "date",
            //    format: "dd.MM.yyyy hh:mm"
            //},
            //{
            //    dataField: "CreateDate",
            //    caption: "Yeni",
            //    dataType: "date",
            //    format: "dd.MM.yyyy hh:mm"
            //  //  format: "dd.MM.yyyy"
            //},
            {
                visible: false,
                dataField: "Id"
            },
            {
                dataField: "SerialNoHasntMacCount",
                caption: "Eksik SeriNo",
            },
            {
                dataField: "Id",
                caption: "No"
            },
            {
                dataField: "ResultText",
                caption: "Durum"
            },
            {
                dataField: "CategoryName",
                caption: "Kategori"
            },
            {
                dataField: "MarkName",
                caption: "Marka"
            },
            {
                dataField: "Owner",
                caption: "Talebi Bulan"
            },
            {
                dataField: "RequestDate",
                caption: "Talep Tarihi",
                dataType: "date",
                format: "dd.MM.yyyy",
                width: 80
            },
            {
                dataField: "EstimatedBuyDate",
                caption: "Satın alma Tarihi",
                dataType: "date",
                format: "dd.MM.yyyy"
            },
            {
                dataField: "ModelName",
                caption: "Model"
            },
            {
                dataField: "Quantity",
                caption: "Adet"
            },
            {
                dataField: "SalesType",
                caption: "Satış Tipi",
                cellTemplate: "cellTemplateSalesType",
                width: 80
            },
            {
                dataField: "ConditionType",
                caption: "Kondisyon",
                cellTemplate: "cellTemplateConditionType",
                width: 80
            },
            {
                dataField: "Channel",
                caption: "Talep Kaynağı",
                width: 80
            },
            {
                dataField: "MonthlyWorkingHours",
                caption: "Aylık Çalışma Saati",
                width: 50
            },
            {
                dataField: "Salesman",
                caption: "Satış Temsilcisi",
            },
            {
                dataField: "UseDurationFull",
                caption: "Kira Süresi",
            },
            {
                dataField: "CategoryId",//machine grid "category>model" fill
                visible: false
            },
            {
                dataField: "MarkId",//machine grid "category>model" fill
                visible: false
            }


            //{
            //    dataField: "IdForCommand",
            //    caption: "İşlem",
            //    cellTemplate: "cellTemplateCommand",
            //    // columnFixing: "leftPosition",
            //    alignment: 'right',
            //    width: 155
            //    //width: 30
            //},
        ];
        
        function dataGridOption() {
            $scope.open_columns = $scope.removeKeyFromList(g_columns, ["SerialNoHasntMacCount", "ResultText"], "dataField");
            $scope.dataGridOptions_Open = {
                dataSourceUrl: '/CustomerRequest/GridList',
                dataSourceFilter: {
                    customerRequestFilter: $scope.CustomerRequestFilter,
                    openCloseMode: 1
                },
                onDataSourceBound: function(pack) {
                    $scope.$parent.CustomerStats.Request = pack.Data.length;
                },
                bindingOptions: g_bindingOptions,
                selection: g_selection,
                onContextMenuPreparing: function(e) {

                    if (e.target === 'header') {
                        if (typeof e.items != "undefined")
                            e.items.push({
                                beginGroup: true,
                                text: "Yeni",
                                onItemClick: function(e) {
                                    $scope.btnRequestNew_Click();
                                }
                            });
                    }
                    if (e.row.rowType === "data") {
                        GridSelectRow("customerRequestGrid_Open", e);
                        var selectedId = e.row.data.Id;
                        $scope.RightClickSelectId = selectedId;
                        e.items = [{
                            text: "Yeni",
                            onItemClick: function(s) {
                                $scope.btnRequestNew_Click();
                            }
                        },
                        {
                            text: "Düzenle",
                            onItemClick: function(s) {
                                $scope.getCustomerRequest(selectedId);
                            }
                        },
                        {
                            text: "Sonuçlandır",
                            onItemClick: function(s) {
                                debugger;
                                MachineModelForMpListFill(e.row.data.CategoryId, e.row.data.MarkId);
                                $scope.changeResultType(selectedId, e);

                               


                            }
                        },
                        {
                            text: "Yenile",
                            onItemClick: function(s) {
                                $scope.RequestGridRefresh_Open();

                            }
                        },


                        ];
                    }
                },
                columns: $scope.open_columns
            };

            $scope.dataGridOptions_Close = {
                dataSourceUrl: '/CustomerRequest/GridList',
                dataSourceFilter: {
                    customerRequestFilter: $scope.CustomerRequestFilter,
                    openCloseMode: 2
                },
                onDataSourceBound: function(pack) {
                    $scope.$parent.CustomerStats.Request = pack.Data.length;
                },
                bindingOptions: g_bindingOptions,
                selection: g_selection,
                onContextMenuPreparing: function(e) {
                    if (e.target === 'header') {
                        if (typeof e.items != "undefined")
                            e.items.push({
                                beginGroup: true,
                                text: "Yeni",
                                onItemClick: function(e) {
                                    $scope.btnRequestNew_Click();
                                }
                            });
                    }
                    if (e.row.rowType === "data") {
                        GridSelectRow("customerRequestGrid_Close", e);
                        var selectedId = e.row.data.Id;
                        $scope.RightClickSelectId = selectedId;
                        e.items = [{
                            text: "Yeni",
                            onItemClick: function(s) {
                                $scope.btnRequestNew_Click();
                            }
                        },
                        {
                            text: "Düzenle",
                            onItemClick: function(s) {
                                $scope.getCustomerRequest(selectedId);
                            }
                        },
                        {
                            text: "Makine Listesi",
                            onItemClick: function(s) {
                                $scope.getPreMachineParkGrid(s);
                                MachineModelForMpListFill(e.row.data.CategoryId, e.row.data.MarkId);
                            }
                        },
                        {
                            text: "Yenile",
                            onItemClick: function(s) {
                                $scope.RequestGridRefresh_Close();

                            }
                        },
                        ];
                    }
                },
                columns: g_columns
            };


        }

        //#endregion Factory
        $scope.MachineParkGridPanelShow = false;

        function saveMachinePark(filterObj) {
            var url = "/CustomerRequest/SaveMachinePark";
            $scope.AjaxPost(url, { parameter: filterObj, mode: 2 }).then(function(result) {

                $scope.RequestMpWrapper = result;
                $scope.RequestGridRefresh();

            });
        }

        function deleteMachinePark(filterObj) {
            var url = "/CustomerRequest/SaveMachinePark";
            $scope.AjaxPost(url, { parameter: filterObj, mode: 3 }).then(function(result) {
                $scope.RequestMpWrapper = result;
                $scope.RequestGridRefresh();
            });
        }

        function GridSelectRow(gridId, e) {
            $scope.GridService.Grid(gridId).selectRows([e.row.data]);
        }

        function MachineModelForMpListFill(categoryId, markId)
        {
            $scope.MachineModelFilterForMpList.CategoryId = categoryId;
            $scope.MachineModelFilterForMpList.MarkId = markId;
            $scope.MachineparkService.GetMachineModel($scope.MachineModelFilterForMpList).then(function(res) {
              
                $scope.MachineModelForMpList = res.data;
            });
        }
      
        function dataGridOptionMachinePark(rowId) {
            $scope.ModelHidePanel($scope.RequestMpWrapper)
            //if (typeof $scope.RequestMpWrapper == "object") {
            //    if ($scope.RequestMpWrapper != null) {
            //        $scope.RequestMpWrapper.ResultType = "Hide";
            //    }
            //}

            $scope.MachineParkGridPanelShow = true;
            if ($scope.dataGridOptionMachinePark != null) {
                $scope.dataGridOptionMachinePark.dataSourceFilter.requestId = rowId;
                $scope.MachineParkGridRefresh();
            } else //grid generator
                $scope.dataGridOptionMachinePark = {
                    dataSourceUrl: '/CustomerRequest/GenerateViewMachinePark',
                    dataSourceFilter: {
                        requestId: rowId,
                        dontExitsCreate: $scope.dontExitsCreate
                    },
                    editing: {
                        mode: "row", // 'batch', 'cell', 'form'
                        allowUpdating: true,
                        allowDeleting: true,
                        allowAdding: false,
                        texts: $scope.GeneralGridEditingTexts
                    },
                    onRowInserting: function(e) {
                        var insertData = $scope.GridService.InsertNewValues(e);
                        insertData.RequestId = $scope.SelectRequestId;
                        insertData.CustomerId = $scope.GetCustomerId();
                        insertData.Quantity = 1;
                        saveMachinePark(insertData);
                        ToggRequesters_Everyclose();
                    },
                    onEditingStart: function(e) {
                    },
                    onContextMenuPreparing: function(e) {
                        if (e.row.rowType === "data") {
                            GridSelectRow("customerRequestMachineParkGrid", e);
                            e.items = [
                                {
                                text: "Kaydı kopyala",
                                onItemClick: function(s) {
                                    $scope.Mp_Copy(e);
                                }
                            },
                               // {
                               //     text: "Toplu Güncelleme",
                               //     onItemClick: function (s) {
                               //        
                               //             $scope.MpFullUpdatePanelOpen(e.row.data.Id);
                               //     }
                               // }
                            ];
                        }

                     
                    },
                    onEditing: function(e) {
                    },
                    onRowUpdating: function(e) {
                        saveMachinePark($scope.GridService.UpdateNewValues(e).key);
                    },
                    onRowRemoving: function(e) {
                        if (e.cancel != true) {
                            deleteMachinePark($scope.GridService.UpdateNewValues(e).key);
                        }

                    },
                    columns: [
                        {
                            dataField: "CategoryId",
                            caption: "Kategori",
                            visible:false,
                            //visible: false,
                            //lookup: {
                            //    dataSource: $scope.MachineParkCategoryList,
                            //    displayExpr: "CategoryName",
                            //    valueExpr: "Id"
                            //}
                        },
                        {
                            dataField: "CategoryName",
                            caption: "Kategori",
                            allowEditing: false,
                            //editCellTemplate: function(cellElement, cellInfo) {
                            //    debugger;
                            //    //cellInfo.column.disable = false
                            //    //cellElement.dxDateBox({
                            //    //    format: 'datetime',
                            //    //    onValueChanged: function(e) {
                            //    //        cellInfo.setValue(e.value);
                            //    //    }
                            //    //});
                            //}
                            //lookup: {
                            //    dataSource: $scope.MachineParkCategoryList,
                            //    displayExpr: "CategoryName",
                            //    valueExpr: "Id"
                            //}
                        },
                        {
                            caption: "Marka",
                            dataField: "MarkId",
                            lookup: {
                                dataSource: $scope.MachineparkMarkList,
                                displayExpr: "MarkName",
                                valueExpr: "Id"
                            }
                        },
                        {
                            dataField: "ModelId",
                            caption: "Model",
                            lookup: {
                               // dataSource: $scope.MachineModelForMpList,
                                dataSource: $scope.MpModelList,
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
                            caption: "Seri No"
                        },
                        {
                            dataField: "ManufactureYear",
                            caption: "Yıl",
                            lookup: {
                                dataSource: $scope.MpYearList,
                                displayExpr: "Year",
                                valueExpr: "Year"
                            }
                        },
                        {
                            dataField: "SaleDate",
                            caption: "Satın Alma",
                            dataType: "date",
                            format: "dd.MM.yyyy",
                            allowEditing: false,
                        },
                        {
                            dataField: "ReleaseDate",
                            caption: "Elden Çıkarma",
                            dataType: "date",
                            format: "dd.MM.yyyy"
                        },
                    ]
                };
        }
        $scope.Test = ""
        $scope.RequestOpencloseTab_Click = function(tab) {
           
            var gridId = ""
            if (tab == "close") {
                gridId = "customerRequestGrid_Close";

            } else {
                gridId = "customerRequestGrid_Open";
            }
           
            $scope.GridService.Repaint(gridId);
            $scope.MachineParkGridPanelShow = false;

        }

        $scope.ToggRequesters_Everyclose = function() {
            ToggRequesters_Everyclose();
        }

        $scope.Mp_Copy = function(e) {

            var id = e.row.data.Id;
            var customerId = $scope.GetCustomerId();
            var options = {
                input: 'number',
                confirmButtonText: 'Onay',
                showCancelButton: true,
                showLoaderOnConfirm: true,
                preConfirm: function(number) {
                    return new Promise(function(resolve, reject) {
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
                function(res) {
                    if (res) {
                        CustomerService.CustomerRequest_CopyMp(e.row.data.Id, res)
                            .then(function (rest) {

                                $scope.RequestMpWrapper = rest;
                                $scope.RequestGridsResfresh();
                               // $scope.RequestResult = rest;
                            });
                    }
                }, options);
        }

        $scope.MpFullUpdatePanelOpen = function (machineParkId) {
            CustomerService.MpFullUpdatePanelModal(machineParkId);
        }

        function initSelectOptions() {

            $scope.MpModel_SelectOptions.BindingParams = $scope.MachineModelFilter;
            $scope.MpMark_SelectOptions.BindingParams = $scope.MachineparkMarkFilter;

            $scope.MPCategory_SelectOptions.BindingParams = $scope.MachineparkCategoryFilter;
            $scope.MpMark_SelectOptions.DeferDataSource = $scope.MachineparkService.GetMachineparkMark;
            $scope.SalesmanList_SelectOptions.GetData();

            $scope.SalesmanList_SelectOptions.onChange = function() {
                $scope.MachineparkCategoryFilter.OnlyRequestVisibleTrue = true;
                $scope.MachineparkCategoryFilter.SalesmanId = ($scope.SalesmanModel != null) ? $scope.SalesmanModel.Value : 0;
                $scope.MpCategory = null;
                $scope.MPCategory_SelectOptions.GetData();
            }

            $scope.MPCategory_SelectOptions.onChange = function() {
                $scope.MachineModelFilter.CategoryId =
                    $scope.MachineparkMarkFilter.CategoryId = ($scope.MpCategory != null) ? $scope.MpCategory.Id : null;
                $scope.MachineModelFilter.RequestVisible = true;
                $scope.MpMark = null;
                $timeout(function() {
                    $scope.MpMark_SelectOptions.GetData();
                }, 300);
            }

            $scope.MpMark_SelectOptions.onChange = function() {
                $scope.MachineModelFilter.MarkId = $scope.MpMark != null ? $scope.MpMark.Id : null;
                $scope.MpModel = null;
                $timeout(function() {
                    $scope.MpModel_SelectOptions.GetData();
                }, 600);
            };

            $scope.MPCategory_SelectOptions.onDataBound = function() {
                $scope.MpCategory = $scope.MPCategory_SelectOptions.GetModel($scope.CustomerRequest.CategoryId);
                if ($scope.MpCategory == null)
                    $scope.CustomerRequest.CategoryId = null;
            }

            $scope.MpMark_SelectOptions.onDataBound = function() {
                $scope.MpMark = $scope.MpMark_SelectOptions.GetModel($scope.CustomerRequest.MarkId);
                if ($scope.MpMark == null) {
                    $scope.CustomerRequest.MarkId = null;
                }
            }
            $scope.MpModel_SelectOptions.onDataBound = function() {
                $scope.MpModel = $scope.MpModel_SelectOptions.GetModel($scope.CustomerRequest.ModelId);
                if ($scope.MpModel == null) {
                    $scope.CustomerRequest.ModelId = null;
                }
            }
        }

    }
]);

HaselApp.controller('CustomerController', ['$scope', '$http', '$timeout', "CustomerService", "MachineparkService", '$q', function CustomerController($scope, $http, $timeout, CustomerService, MachineparkService, $q) {
    $scope.CustomerService = CustomerService;
    $scope.MachineparkService = MachineparkService;

    $scope.LocationFilter = angular.copy(LocationFilter);

    $scope.MachineParkCategoryList = MachineParkCategoryList;
    $scope.MachineparkMarkList = MachineparkMarkList;
    $scope.MpModelList = MpModelList;



    $scope.GetCustomerId = function() {
        return $scope.getQueryStringByName("cid") == "" ? 0 : $scope.getQueryStringByName("cid");
    }

    $scope.LocationFilter.CustomerId = $scope.GetCustomerId();

    $scope.CustomerLocations = new DevExpress.data.CustomStore({
        load: function(loadopt) {
            var defer = $q.defer();
            $scope.CustomerService.GetLocationLookup($scope.LocationFilter, function(r) {
                defer.resolve(r.Data);
            });
            return defer.promise;
        },
        byKey: function(key, extra) { }
    });

    $scope.MpModel_SelectOptions = {
        DeferDataSource: $scope.MachineparkService.GetMachineModel,
        placeholder: "Seçiniz",
        textField: 'Name',
        valueField: "Id",
        BindOnLoad: false,
    };

    $scope.MpMark_SelectOptions = {
        placeholder: "Seçiniz",
        textField: 'MarkName',
        valueField: "Id",
        BindOnLoad: false,

        /* customizeText:function(text) {
            return text.IsOwnerMachine;
        }
       [Id],[MarkName]      ,[IsOwnerMachine]      ,[IsActive]      ,[IsDeleted]
       */
    };

    $scope.MPCategory_SelectOptions = {
        DeferDataSource: $scope.MachineparkService.GetMachineparkCategory,
        placeholder: "Seçiniz",
        textField: 'CategoryName',
        valueField: "Id",
        childField:'Categories',
        levelField: 'TreeLevel',
        BindOnLoad: false,
    };

    $scope.MpYearList = MpYearList;
    $scope.MpYear_SelectOptions = {
        DataSource: $scope.MpYearList,
        placeholder: "Seçiniz",
        textField: 'Year',
        valueField: 'Year'
    };

    $scope.LocationFilter.CustomerId = $scope.GetCustomerId();
    $scope.MpLocation_SelectOptions = {
        DataSource: $scope.CustomerService.GetLocations,
        BindingParams: $scope.LocationFilter,
        placeholder: "Seçiniz",
        textField: 'Name',
        valueField: 'Id',
    };
    $scope.CustomerStats = {
        Request: 0,
        ActiveMachinepark: 0,
        PassiveMachinepark: 0
    };

    $scope.initTab = function(tabeventName) {
        $scope.$broadcast(tabeventName);
    }

    $scope.CollapseButtonPanel = function(rowId) {
        $("#CollapseButtonPanel" + rowId).animate({
            width: "toggle",
            opacity: "toggle"
        }, "slow");
    };

    $scope.RequiredMessage = "Zorunludur";
}]);