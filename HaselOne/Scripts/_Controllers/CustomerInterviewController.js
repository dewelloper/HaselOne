HaselApp.controller('CustomerInterviewController', ['$scope', '$http', '$timeout', "CustomerService", '$q', 'InterviewService',
    function CustomerInterviewController($scope, $http, $timeout, CustomerService, $q, InterviewService) {
        var ControlConfig = {
            ModelId: "CustomerInterview",
            initTab: "initInterviewTab",
            FormId: "InterviewForm",
            PlaceholderText: "Seçiniz",
            GridId: "customerInterviewGrid",
            SaveUrl:"/Interview/Save",
            GetUrl: "/Interview/Get",
            GetListUrl:"/Interview/GetList",
            GridColumn: [
                {
                    dataField: "Id",
                    visible: false,
                    caption: "",
                },
                {
                    dataField: "InterviewDate",
                    caption: "Tarih",
                    dataType: "date",
                    format: "dd.MM.yyyy",
                    width: 100
                },
                {
                    dataField: "User",
                    caption: "Görüşen",
                },
                {
                    dataField: "AuthenticatorsName",
                    caption: "Görüşülen",
                },
                {
                    dataField: "Interview",
                    caption: "Görüşme Tipi",
                },
                {
                    dataField: "InterviewImportant",
                    caption: "Önem Durumu",
                }, {
                    dataField: "Interviewed",
                    caption: "Görüşüldü",
                },

            ],
            GridContextMenu: function (e) {
                if (e.target === 'header') {
                    if (typeof e.items != "undefined")
                        e.items.push({
                            beginGroup: true,
                            text: "Yeni",
                            onItemClick: function (e) {
                                $scope.newForm();
                            }
                        });
                }

                if (e.row != "undefined")
                    if (e.row.rowType === "data") {
                        //GridSelectRow(ControlConfig.GridId, e);
                        e.items = [
                            {
                                text: "Yeni",
                                onItemClick: function (s) {
                                    $scope.newForm();
                                }
                            },
                            {
                                text: "Düzenle",
                                onItemClick: function (s) {
                                    debugger;
                                    $scope.getItem(e.row.data.Id);
                                }
                            }
                        ];
                    }
            }
        };
        $scope.currentCustomerId = $scope.getQueryStringByName("cid");
        $scope.CustomerInterview = angular.copy(CustomerInterviews);
        /*****************************************************************************
        *
        * Event listeners for UI elements
        *
        ****************************************************************************/
        $scope.$on(ControlConfig.initTab, function (e) {
            dataGridGenerator();
            $scope.Interview_SelectOptions.GetData();
            $scope.InterviewImportant_SelectOptions.GetData();
            $scope.InterviewUser_SelectOptions.GetData();

            $scope.InterviewAuthenticator_SelectOptions.BindingParams = { filter: $scope.currentCustomerId };
            $scope.InterviewAuthenticator_SelectOptions.GetData();
        });
        $scope.$watch(ControlConfig.ModelId, function (p) {
            $scope.MessagePanelFromModelTracker(p);
        });
        $scope.resetForm = function () {
            $scope.CustomerInterview = angular.copy(CustomerInterviews);
            $scope.CustomerInterview.CustomerId = $scope.currentCustomerId;
            $scope.CustomerInterview.UserId =null;
            $scope.MessagePanelFromModelTracker($scope.CustomerInterview);
        }
        $scope.gridRefresh = function () {
            $scope.GridService.Refresh(ControlConfig.GridId);
        }
        $scope.newForm = function () {
            $scope.resetForm();
            $scope.ToggleCollapsePanel(ControlConfig.FormId, true);
            $scope.ScrollTo(ControlConfig.FormId);
        }
        $scope.getItem = function (rowId) {
            $scope.AjaxPost(ControlConfig.GetUrl, { filter: parseInt(rowId) }).then(function (pack) {
                if (pack.IsSuccess)
                {
                    pack.Data.InterviewDate = new Date(pack.Data.InterviewDate);
                    $scope.CustomerInterview = pack.Data;

                    $scope.ToggleCollapsePanel(ControlConfig.FormId, true);
                    $scope.ScrollTo(ControlConfig.FormId)
                }
                else {
                    $scope.CustomerInterview = pack;
                }
               
            })
        }
        $scope.saveForm = function () {
            $scope.AjaxPost(ControlConfig.SaveUrl, { vm: $scope.CustomerInterview }).then(function (a) {
                 
                $scope.CustomerInterview = a;
                if (a.IsSuccess && a.IsValid) {
                    $scope.resetForm();
                    $scope.gridRefresh();
                    $scope.ToggleCollapsePanel(ControlConfig.FormId, false);
                }
            });
        };

        /*****************************************************************************
        *
        * Lookup config
        *
        ****************************************************************************/
        $scope.InterviewModel = {};
        $scope.Interview_SelectOptions = {
            DataSource: $scope.InterviewModelList,
            placeholder: ControlConfig.PlaceholderText,
            textField: 'Text',
            valueField: "Value",
            BindOnLoad: false,
            DeferDataSource: InterviewService.GetInterview,

        };
        $scope.InterviewImportantModel = {};
        $scope.InterviewImportant_SelectOptions = {

            placeholder: ControlConfig.PlaceholderText,
            textField: 'Text',
            valueField: "Value",
            BindOnLoad: false,
            DeferDataSource: InterviewService.GetInterviewImportant,

        };
        $scope.InterviewAuthenticatorModel = {};
        $scope.InterviewAuthenticator_SelectOptions = {

            placeholder: ControlConfig.PlaceholderText,
            textField: 'Text',
            valueField: "Value",
            BindOnLoad: false,
            DeferDataSource: InterviewService.GetAuthenticator,

        };
        $scope.InterviewUserModel = {};
        $scope.InterviewUser_SelectOptions = {

            placeholder: ControlConfig.PlaceholderText,
            textField: 'Text',
            valueField: "Value",
            BindOnLoad: false,
            DeferDataSource: InterviewService.GetInterviewUser,

        };
        
        function dataGridGenerator() {

            $scope.MessagePanelFromModelTracker($scope.CustomerInterview);
            $scope.Interviewfilter = angular.copy(CustomerInterviewsFilter);
            $scope.Interviewfilter.CustomerId = $scope.currentCustomerId;


            $scope.customerInterviewGrid = {
                dataSourceUrl: ControlConfig.GetListUrl,
                dataSourceFilter: {
                    filter: $scope.Interviewfilter
                },

                onContextMenuPreparing: ControlConfig.GridContextMenu,
                columns:ControlConfig.GridColumn,
            };
        }
        
    }
]);
