HaselApp.controller('CustomerRequestController', ['$scope', '$http', '$timeout', "CustomerService", '$q',
    function CustomerRequestController($scope, $http, $timeout, CustomerService, $q) {
        $scope.IsEditPanelActive = true;
        $scope.MessagePanelTimeout = 5000;
        $scope.GetCustomerId = function () {
            return $scope.getQueryStringByName("cid") == "" ? 0 : $scope.getQueryStringByName("cid");
        }

        $scope.SetEditPanel = function (expandStatus) {
            $scope.ToggleCollapsePanel("RequestPanel", expandStatus);
            $scope.IsEditPanelActive = expandStatus;
        }
        CustomerRequestModel.RequestDate = new Date();
        $scope.CustomerRequest = angular.copy(CustomerRequestModel);
        $scope.RequestMpWrapper = {};
        $scope.RequestMpWrapperList = [];

        //#Region Watch
        $scope.$watch('RequestResult',
            function (param) {
                if (typeof param != "undefined" && typeof param.ResultType != "undefined") {
                    if (param.ResultType != "Hide") {
                        $scope.ScrollTo("MessgePanelRequest");
                        if (param.ResultType == "Info") {
                            $timeout(function () {
                                $scope.RequestResult.ResultType = "Hide";
                            },
                            $scope.MessagePanelTimeout);
                        }
                    }
                }
            });

        $scope.$watch('RequestMpWrapper',
            function (param) {
                if (typeof param != "undefined" && typeof param.ResultType != "undefined" /*&& typeof param == "object"*/) {
                    if (param.ResultType != "Hide") {
                        $scope.ScrollTo("MesPanReqMacPark");
                        if (param.ResultType == "Info") {
                            $timeout(function () {
                                $scope.RequestMpWrapper.ResultType = "Hide";
                            },
                            $scope.MessagePanelTimeout);
                        }
                    }
                }
            });

        $scope.RentModelDisableReadonly = false;
        $scope.$watch('CustomerRequest.SalesType',
            function (p) {
                if (typeof p !== "undefined") {
                    if (p == 2) {
                        //alert("Kiralik modulu aktif degildir"); -- kiralik girilsin fakat sonlanmasin istenildi.
                        //$scope.CustomerRequest.SalesType = 1;
                        $scope.RentModelDisableReadonly = true;
                    } else {
                        $scope.RentModelDisableReadonly = false;
                    }


                }
            });

        //#EndRegion

        $scope.MachineModelFilter = angular.copy(MachineModelFilter);
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
        $scope.SetCustomerId = function () {
            $scope.CustomerRequest.CustomerId = $scope.GetCustomerId();
        }

        $scope.ResetCustomer = function () {
            $scope.CustomerRequest = angular.copy(CustomerRequestModel);
        }

        $scope.SetCustomerId();
        $scope.CustomerRequestFilter = angular.copy(CustomerRequestFilter);
        $scope.CustomerRequestFilter.CustomerId = $scope.CustomerRequest.CustomerId;

        //#region Events
        $scope.RequestSave = function () {
            $scope.AjaxPost('/CustomerRequest/Save', { vm: $scope.CustomerRequest }).then(function (a) {
                $scope.RequestResult = a;
                if (a.IsSuccess && a.IsValid) {
                    $scope.ResetCustomer();
                    $scope.SetCustomerId();
                    $scope.CustomerRequestFilter.Id = 0;
                    $scope.RequestGridRefresh();
                    ToggRequesters();
                }
            });
        };
        $scope.RequestGridRefresh = function () {
            $scope.CustomerRequestFilter.Id = 0;
            $scope.GridService.Refresh("customerRequestGrid");
        }
        $scope.MachineParkGridRefresh = function () {
            $scope.MachineParkGridPanelShow = true;
            $scope.GridService.Refresh("customerRequestMachineParkGrid");
            //$scope.RequestGridRefresh();
        }
        $scope.RequestGridsResfresh = function () {
            $scope.RequestGridRefresh();
            $scope.MachineParkGridRefresh();
        }

        $scope.FormModeEngine = function (tabId, formMode) {
            var disable;
            if (FormMode == "readonly") {
                disable = true;
            } else {
                disable = false;
            }

            $("#" + tabId).find("input").each(function () {
                //select-search de calismiyor.
                $(this).prop('disabled', disable);
            });

        }

        $scope.QuantityIsReadonlyControl = function (customerRequest) {
             
            $scope.QuantityIsReadonly = false;
            if (typeof customerRequest != "undefined" && customerRequest != null) {
                if (customerRequest.ResultType == 2 || customerRequest.ResultType == 1) {
                    $scope.QuantityIsReadonly = true;
                }
            }

        }

        $scope.getCustomerRequest = function (rowId) {
            $scope.CustomerRequestFilter.Id = rowId;
            CustomerService.GetRequest($scope.CustomerRequestFilter).then(function (pack) {
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

        $scope.changeResultType = function (rowId) {
            var resultyType = 0;
            $scope.AlertService.Confirm("", "Kaydı sonlardır",
                function (res) {//{1: "Satış", 2: "Kayıp Satış", 3: "Bekliyor"} 
                    if (res == true) {
                        resultyType = 1;
                    } else {
                        resultyType = 2;
                    }
                     
                    $scope.AjaxPost('/CustomerRequest/ChangeResultType', { requestId: rowId, resultType: resultyType }).then(function (a) {
                        $scope.RequestResult = a;

                        if (a.IsSuccess && a.IsValid) {
                            $scope.SetCustomerId();
                            $scope.RequestGridsResfresh();
                            dataGridOptionMachinePark($scope.RightClickSelectId);
                        }

                    });
                },
        {
            confirmButtonText: "Satış",
            cancelButtonText: "Kayıp Satış",
            showCancelButton: true,
        });

        }

        $scope.SelectRequestId = 0;
        $scope.showRequestResult = function (rowId) {
            $scope.SelectRequestId = rowId;
            dataGridOptionMachinePark(rowId);

            $scope.ScrollTo("RequestForm");
            //  $scope.SetEditPanel(true);
        }

        $scope.QuantityIsReadonly = "";
        $scope.btnRequestNew_Click = function () {
            $scope.QuantityIsReadonly = "";
            $scope.ResetCustomer();
            $scope.SetCustomerId();
            ToggRequesters_Everyopen();
        };

        //#endregion

        //#region Factory
        $scope.GridGen = function () {
            dataGridOption();
        }

        $scope.$on('initRequestTab', function (e) {
            $scope.MachineparkCategoryFilter.CustomerId = $scope.GetCustomerId();
            $scope.GridGen();
            initSelectOptions();
        });
        $scope.$watchCollection("dataGridOptions.SelectedRows", function (e) {
            if (typeof e != "undefined") {
                $scope.dontExitsCreate = false;
                if (e[0].ResultType != 3) {
                   
                    $scope.SelectRequestId = e[0].Id;
                    dataGridOptionMachinePark(e[0].Id);
                    $scope.ScrollTo("RequestMachineParkPanel");
                } else {
                    dataGridOptionMachinePark(e[0].Id);
                }

            }

        });
        $scope.btnRefresh_Click = function () {
            $scope.getCustomerRequest($scope.customerRequestFilter.Id);
        }
        $scope.RightClickSelectId = 0;
        function dataGridOption() {
            $scope.dataGridOptions = {
                dataSourceUrl: '/CustomerRequest/GridList',
                dataSourceFilter: {
                    customerRequestFilter: $scope.CustomerRequestFilter
                },
                onDataSourceBound: function (pack) {
                    $scope.$parent.CustomerStats.Request = pack.Data.length;
                },
                bindingOptions: {
                    "selection.selectAllMode": "selectAllMode",
                    "selection.showCheckBoxesMode": "showCheckBoxesMode"
                },
                selection: {
                    mode: "multiple"
                },
                onContextMenuPreparing: function (e) {
                     
                    console.log(e);
                    if (e.row.rowType === "data") {
                        var selectedId = e.row.data.Id;
                        $scope.RightClickSelectId = selectedId;
                        e.items = [
                                       {
                                           text: "Düzenle",
                                           onItemClick: function (s) {
                                               $scope.getCustomerRequest(selectedId);
                                           }
                                       },
                                       {
                                           text: "Sonuçlandır",
                                           onItemClick: function (s) {
                                               $scope.changeResultType(selectedId);

                                           }
                                       }
                        ];
                    }
                },
                columns: [
                    {
                        dataField: "Id",
                        visible: false,
                        caption: "Id"
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
                        caption: "Sahip"
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
                        caption: "Talep Türü",
                        width: 80
                    },
                    {
                        dataField: "MonthlyWorkingHours",
                        caption: "Aylık Çalışma Saati",
                    },
                    {
                        dataField: "Salesman",
                        caption: "Satis Temsilcisi",
                    },
                    {
                        dataField: "UseDurationFull",
                        caption: "Kira Süresi",
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

                ]
            };
        }

        //#endregion Factory
        $scope.MachineParkGridPanelShow = false;


        function saveMachinePark(filterObj) {
            var url = "/CustomerRequest/SaveMachinePark";
            $scope.AjaxPost(url, { parameter: filterObj, mode: 2 }).then(function (result) {
              
                $scope.RequestMpWrapper = result;
                $scope.RequestGridRefresh();

            });
        }
        function deleteMachinePark(filterObj) {
            var url = "/CustomerRequest/SaveMachinePark";
            $scope.AjaxPost(url, { parameter: filterObj, mode: 3}).then(function (result) {

                $scope.RequestMpWrapper = result;
                $scope.RequestGridRefresh();

            });
        }
        function dataGridOptionMachinePark(rowId) {


            if (typeof $scope.RequestMpWrapper == "object") {
                $scope.RequestMpWrapper.ResultType = "Hide";
            }

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
                        mode: "row",
                        allowUpdating: true,
                        allowDeleting: true,
                        allowAdding: false,
                        texts: $scope.GeneralGridEditingTexts
                    },
                    onRowInserting: function (e) {
                        var insertData = $scope.GridService.InsertNewValues(e);

                        insertData.RequestId = $scope.SelectRequestId;
                        insertData.CustomerId = $scope.GetCustomerId();
                        insertData.Quantity = 1;
                        saveMachinePark(insertData);
                        ToggRequesters_Everyclose();
                    },
                    onContextMenuPreparing: function (e) {
                        console.log(e);
                        if (e.target === 'header') {
                            e.items.push({
                                beginGroup: true,
                                text: "Test1",
                                onItemClick: function (e) {
                                    console.log(e);
                                }
                            },
                                {
                                    beginGroup: true,
                                    text: "Test2",
                                    onItemClick: function () {
                                        console.log("test2");
                                    }
                                });
                        }

                        if (e.row.rowType === "data") {
                            e.items = [
                                {
                                    text: "Kaydı kopyala",
                                    onItemClick: function (s) {

                                        var count = prompt("Kac Adet", "");
                                        if (count > 0 && count < 55) {
                                            CustomerService.CustomerRequest_CopyMp(e.row.data.Id, count)
                                                .then(function (rest) {
                                                    $scope.RequestResult = rest;
                                                    $scope.RequestMpWrapper = rest.Data;
                                                    $scope.RequestGridsResfresh();
                                                });

                                        } else {
                                            alert("0 50 arasi olmali");
                                        }

                                    }
                                }
                            ];
                        }
                    },

                    onRowInserted: function (e) {
                    },
                    onRowUpdating: function (e) {
                        saveMachinePark($scope.GridService.UpdateNewValues(e).key);
                    },

                    onRowUpdated: function (e) {
                    },
                    onRowRemoving: function (e) {
                        deleteMachinePark($scope.GridService.UpdateNewValues(e).key);
                    },

                    onRowRemoved: function (e) {
                    },
                    onRowSelecting: function (e) {

                    },
                    onRowSelected: function (e) {

                    },
                    columns: [
                        {
                            visible: false,
                            dataField: "Id"
                        },

                        {
                            dataField: "CategoryId",
                            caption: "Kategori",
                            disable: false,
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
                            dataSource: $scope.MachineparkMarkList,
                            displayExpr: "MarkName",
                            valueExpr: "Id"
                        }
                    },
                    {
                        dataField: "ModelId",
                        caption: "Model",
                        lookup: {
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
                          format: "dd.MM.yyyy"
                      },
                        //{
                        //    dataField: "PlanedReleaseDate",
                        //    caption: "Elden Çıkarma(Planlanan)",
                        //    dataType: "date",
                        //    format: "dd.MM.yyyy"
                        //},
                          {
                              dataField: "ReleaseDate",
                              caption: "Elden Çıkarma",
                              dataType: "date",
                              format: "dd.MM.yyyy"
                          },
                    ]
                };
        }
        function initSelectOptions() {
            $scope.MpModel_SelectOptions.BindingParams = $scope.MachineModelFilter;
            $scope.MpMark_SelectOptions.BindingParams = $scope.MachineparkMarkFilter;
            $scope.MPCategory_SelectOptions.BindingParams = $scope.MachineparkCategoryFilter;
            $scope.MpMark_SelectOptions.DeferDataSource = $scope.MachineparkService.GetMachineparkMark;
            $scope.SalesmanList_SelectOptions.GetData();

            $scope.SalesmanList_SelectOptions.onChange = function () {
                $scope.MachineparkCategoryFilter.SalesmanId = ($scope.SalesmanModel != null) ? $scope.SalesmanModel.Value : 0;
                $scope.MpCategory = null;
                $scope.MPCategory_SelectOptions.GetData();
            }

            $scope.MPCategory_SelectOptions.onChange = function () {
                $scope.MachineModelFilter.CategoryId =
                    $scope.MachineparkMarkFilter.CategoryId = ($scope.MpCategory != null) ? $scope.MpCategory.Id : null;
                $scope.MachineModelFilter.RequestVisible = true;
                $scope.MpMark = null;
                $timeout(function () {
                    $scope.MpMark_SelectOptions.GetData();
                }, 300);
            }

            $scope.MpMark_SelectOptions.onChange = function () {
                $scope.MachineModelFilter.MarkId = $scope.MpMark != null ? $scope.MpMark.Id : null;
                $scope.MpModel = null;
                $timeout(function () {
                    $scope.MpModel_SelectOptions.GetData();
                }, 600);
            };

            $scope.MPCategory_SelectOptions.onDataBound = function () {

                $scope.MpCategory = $scope.MPCategory_SelectOptions.GetModel($scope.CustomerRequest.CategoryId);
                if ($scope.MpCategory == null)
                    $scope.CustomerRequest.CategoryId = null;
            }

            $scope.MpMark_SelectOptions.onDataBound = function () {

                $scope.MpMark = $scope.MpMark_SelectOptions.GetModel($scope.CustomerRequest.MarkId);
                if ($scope.MpMark == null) {
                    $scope.CustomerRequest.MarkId = null;
                }
            }
            $scope.MpModel_SelectOptions.onDataBound = function () {

                $scope.MpModel = $scope.MpModel_SelectOptions.GetModel($scope.CustomerRequest.ModelId);
                if ($scope.MpModel == null) {
                    $scope.CustomerRequest.ModelId = null;
                }
            }
        }

        //garbage
        $scope.conditionStatus2xx = function (rowId) {
            //{1: "Satış", 2: "Kayıp Satış", 3: "Bekliyor"} 

            $scope.AlertService.Confirm("", "Kaydı sonlardır",
            {
                html: $scope.generatorSwartButton(rowId)

            });

        }
        $scope.generatorSwartButtonxx = function (rowId) {

            var resultyType = -1;
            var buttons = $('<div>').append(createButton('Satış', function () {
                resultyType = 1;
                ResultTypeService(rowId, resultyType);
                swal.close();
            }))
                .append(createButton('Kayıp Satış', function () {
                    resultyType = 2;
                    ResultTypeService(rowId, resultyType);
                    swal.close();
                }))
                .append(createButton('Iptal', function () {
                    swal.close();
                }));
            return buttons;
        }
        function ResultTypeServicexx(rowId, resultType) {
            $scope.AjaxPost('/CustomerRequest/ChangeResultType', { requestId: rowId, resultType: resultType }).then(function (a) {
                $scope.RequestResult = a;
                if (a.IsSuccess && a.IsValid) {
                    $scope.ResetCustomer();
                    $scope.SetCustomerId();
                    $scope.CustomerRequestFilter.Id = 0;
                    // $scope.GridService.Refresh("customerRequestGrid");
                    ToggRequesters_Everyclose();

                }
            });
        }
        //todo array tipde roota tasinmali
        function createButtoxxxn(text, cb) {
            return $('<button>' + text + '</button>').on('click', cb);
        }

    }]);

HaselApp.controller('CustomerController', ['$scope', '$http', '$timeout', "CustomerService", "MachineparkService", '$q', function CustomerController($scope, $http, $timeout, CustomerService, MachineparkService, $q) {

    $scope.CustomerService = CustomerService;
    $scope.MachineparkService = MachineparkService;

    $scope.LocationFilter = angular.copy(LocationFilter);

    $scope.MachineParkCategoryList = MachineParkCategoryList;
    $scope.MachineparkMarkList = MachineparkMarkList;
    $scope.MpModelList = MpModelList;

    $scope.GetCustomerId = function () {
        return $scope.getQueryStringByName("cid") == "" ? 0 : $scope.getQueryStringByName("cid");
    }

    $scope.LocationFilter.CustomerId = $scope.GetCustomerId();

    $scope.CustomerLocations = new DevExpress.data.CustomStore({
        load: function (loadopt) {
            var defer = $q.defer();
            $scope.CustomerService.GetLocationLookup($scope.LocationFilter, function (r) {
                defer.resolve(r.Data);
            });
            return defer.promise;
        },
        byKey: function (key, extra) {

        }
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
        BindOnLoad: false
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
        Machinepark: 0
    };

    $scope.initTab = function (tabeventName) {
        $scope.$broadcast(tabeventName);
    }

    $scope.CollapseButtonPanel = function (rowId) {
        $("#CollapseButtonPanel" + rowId).animate({
            width: "toggle",
            opacity: "toggle"
        }, "slow");
    };

    $scope.RequiredMessage = "Zorunludur";
}]);