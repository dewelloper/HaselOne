HaselApp.controller('SelasmanStatsReportController', ['$scope', '$http', '$timeout', 'BaseService', function SelasmanStatsReportController($scope, $http, $timeout, BaseService) {
    $scope.SelectedRow = {};
    $scope.IsFilterExpanded = true;

    $scope.CategorySelectOptions = {
        DataSource: BaseService.GetCategories,
        placeholder: "Hepsi",
        textField: 'Title',
        AllowEmptyModel: false
    };
    $scope.AreaSelectOptions = {
        DataSource: BaseService.GetAreas,
        placeholder: "Hepsi",
        textField: 'AreaName',
    };
    $scope.SalesmanSelectOptions = {
        DataSource: BaseService.GetSalesmans,
        BindingParams: "",
        placeholder: "Hepsi",
        textField: 'Text'
    };

    $scope.MpCategorySelectOptions = {
        DataSource: BaseService.GetMachineparkCategories,
        BindingParams: "",
        placeholder: "Hepsi",
        textField: 'CategoryName',
    };
    $scope.MarkSelectOptions = {
        DataSource: BaseService.GetMarks,
        BindingParams: "",
        placeholder: "Hepsi",
        textField: 'Text'
    };

    $scope.SegmentSelectOptions = {
        DataSource: BaseService.GetSegments,
        BindingParams: "",
        placeholder: "Hepsi",
        textField: 'Title'
    };

    $scope.CategorySelectOptions.onChange = function () {
        if (typeof $scope.MpCategorySelectOptions.GetData === 'function')
            $scope.MpCategorySelectOptions.GetData();
    }
    $scope.BindDataGrid = function() {
        $scope.dataGridOptions = {
            dataSourceUrl: '/Report/GetSalesmanStats',
            dataSourceFilter: { filter: $scope.ReportFilter },
            ReportName: "Müşteri - Makine Parkı Sayıları(Baz: Satış Mühendisi) -" + moment().format('YYYYMMDDhhmmss'),
            columns: [{
                dataField: "SalesmanId",
                visible: false,
                caption: "Adı"
            }, {
                dataField: "SalesmanName",
                caption: "Satış Mühendisi",
                fixed: true
            }, {
                dataField: "CustomerCount",
                caption: "Müşteri",
                headerFilter: {
                    groupInterval: 100
                }
            }, {
                dataField: "TotalCustomerCount",
                caption: "Müşteri(Toplam)",
                headerFilter: {
                    groupInterval: 100
                }
            }, {
                dataField: "MachineParkCount",
                caption: "Makine Parkı",
                headerFilter: {
                    groupInterval: 100
                }
            }, {
                dataField: "TotalMachinePark",
                caption: "Makine Parkı(Toplam)",
                headerFilter: {
                    groupInterval: 100
                }
            }],
            summary: {
                totalItems: [
                    {
                        column: "SalesmanName",
                        summaryType: "count",
                        customizeText: function (data) {
                            return "Toplam: " + data.value;
                        }
                    }, {
                        column: "CustomerCount",
                        summaryType: "sum",
                        customizeText: function (data) {
                            return "Toplam: " + data.value;
                        }
                    }, {
                        column: "TotalCustomerCount",
                        summaryType: "sum",
                        customizeText: function (data) {
                            return "Toplam: " + data.value;
                        }
                    }, {
                        column: "MachineParkCount",
                        summaryType: "sum",
                        customizeText: function (data) {
                            return "Toplam: " + data.value;
                        }
                    }, {
                        column: "TotalMachinePark",
                        summaryType: "sum",
                        customizeText: function (data) {
                            return "Toplam: " + data.value;
                        }
                    }]
            }
        };
    }
    $scope.CategorySelectOptions.onDataBound = function () {
        $scope.ReportFilter.Category = $scope.CategorySelectOptions.data[1];
        $scope.BindDataGrid();
    }
    var refresTimeOut = null;
    $scope.$watchCollection('ReportFilter', function (filter) {
        if (refresTimeOut)
            $timeout.cancel(refresTimeOut);
        refresTimeOut = $timeout(function () {
            $scope.GridService.Refresh("grid");
        }, 800);
    });
}]);

HaselApp.controller('AreaStatsReportController', ['$scope', '$http', '$timeout', 'BaseService', function AreaStatsReportController($scope, $http, $timeout, BaseService) {
    $scope.SelectedRow = {};
    $scope.IsFilterExpanded = true;

    $scope.CategorySelectOptions = {
        DataSource: BaseService.GetCategories,
        placeholder: "Hepsi",
        textField: 'Title',
        AllowEmptyModel: false
    };
    $scope.AreaSelectOptions = {
        DataSource: BaseService.GetAreas,
        placeholder: "Hepsi",
        textField: 'AreaName',
    };
    $scope.SalesmanSelectOptions = {
        DataSource: BaseService.GetSalesmans,
        BindingParams: "",
        placeholder: "Hepsi",
        textField: 'Text'
    };
    $scope.MpCategorySelectOptions = {
        DataSource: BaseService.GetMachineparkCategories,
        BindingParams: "",
        placeholder: "Hepsi",
        textField: 'CategoryName',
    };
    $scope.MarkSelectOptions = {
        DataSource: BaseService.GetMarks,
        BindingParams: "",
        placeholder: "Hepsi",
        textField: 'Text'
    };
    $scope.SegmentSelectOptions = {
        DataSource: BaseService.GetSegments,
        BindingParams: "",
        placeholder: "Hepsi",
        textField: 'Title'
    };

    $scope.CategorySelectOptions.onChange = function () {
        if (typeof $scope.MpCategorySelectOptions.GetData === 'function')
            $scope.MpCategorySelectOptions.GetData();
    }
    $scope.BindDataGrid = function () {
        $scope.dataGridOptions = {
            dataSourceUrl: '/Report/GetAreaStats',
            dataSourceFilter: { filter: $scope.ReportFilter },
            ReportName: "Müşteri - Makine Parkı Sayıları(Baz: Bölge) -" + moment().format('YYYYMMDDhhmmss'),
            paging: {
                pageSize: 20
            },
            columns: [{
                dataField: "AreaId",
                visible: false,
                caption: "Id"
            }, {
                dataField: "AreaName",
                caption: "Bölge Adı",
                fixed: true
            }, {
                dataField: "CustomerCount",
                caption: "Müşteri",
                headerFilter: {
                    groupInterval: 100
                }
            }, {
                dataField: "TotalCustomerCount",
                caption: "Müşteri(Toplam)",
                headerFilter: {
                    groupInterval: 100
                }
            }, {
                dataField: "MachineParkCount",
                caption: "Makine Parkı",
                headerFilter: {
                    groupInterval: 100
                }
            }, {
                dataField: "TotalMachinePark",
                caption: "Makine Parkı(Toplam)",
                headerFilter: {
                    groupInterval: 100
                }
            }],
            summary: {
                totalItems: [
                    {
                        column: "AreaName",
                        summaryType: "count",
                        customizeText: function (data) {
                            return "Toplam: " + data.value;
                        }
                    }, {
                        column: "CustomerCount",
                        summaryType: "sum",
                        customizeText: function (data) {
                            return "Toplam: " + data.value;
                        }
                    }, {
                        column: "TotalCustomerCount",
                        summaryType: "sum",
                        customizeText: function (data) {
                            return "Toplam: " + data.value;
                        }
                    }, {
                        column: "MachineParkCount",
                        summaryType: "sum",
                        customizeText: function (data) {
                            return "Toplam: " + data.value;
                        }
                    }, {
                        column: "TotalMachinePark",
                        summaryType: "sum",
                        customizeText: function (data) {
                            return "Toplam: " + data.value;
                        }
                    }]
            }
        };
    }

    $scope.CategorySelectOptions.onDataBound = function () {
        $scope.ReportFilter.Category = $scope.CategorySelectOptions.data[1];
        $scope.BindDataGrid();
    }

    var refresTimeOut = null;
    $scope.$watchCollection('ReportFilter', function (filter) {
        if (refresTimeOut)
            $timeout.cancel(refresTimeOut);
        refresTimeOut = $timeout(function () {
            $scope.GridService.Refresh("grid");
        }, 800);
    });
}]);

HaselApp.controller('SegmentStatsReportController', ['$scope', '$http', '$timeout', 'BaseService', function SegmentStatsReportController($scope, $http, $timeout, BaseService) {
    $scope.SelectedRow = {};
    $scope.IsFilterExpanded = true;

    $scope.CategorySelectOptions = {
        DataSource: BaseService.GetCategories,
        placeholder: "Hepsi",
        textField: 'Title',
        AllowEmptyModel: false
    };
    $scope.AreaSelectOptions = {
        DataSource: BaseService.GetAreas,
        placeholder: "Hepsi",
        textField: 'AreaName',
    };
    $scope.SalesmanSelectOptions = {
        DataSource: BaseService.GetSalesmans,
        BindingParams: "",
        placeholder: "Hepsi",
        textField: 'Text'
    };
    $scope.MpCategorySelectOptions = {
        DataSource: BaseService.GetMachineparkCategories,
        BindingParams: "",
        placeholder: "Hepsi",
        textField: 'CategoryName',
    };
    $scope.MarkSelectOptions = {
        DataSource: BaseService.GetMarks,
        BindingParams: "",
        placeholder: "Hepsi",
        textField: 'Text'
    };
    $scope.SegmentSelectOptions = {
        DataSource: BaseService.GetSegments,
        BindingParams: "",
        placeholder: "Hepsi",
        textField: 'Title'
    };
    $scope.CategorySelectOptions.onChange = function () {
        if (typeof $scope.MpCategorySelectOptions.GetData === 'function')
            $scope.MpCategorySelectOptions.GetData();
    }
    $scope.BindDataGrid = function () {
        $scope.dataGridOptions = {
            dataSourceUrl: '/Report/GetSegmentStats',
            dataSourceFilter: { filter: $scope.ReportFilter },
            ReportName: "Müşteri - Makine Parkı Sayıları(Baz: Segment) -" + moment().format('YYYYMMDDhhmmss'),
            paging: {
                pageSize: 20
            },
            columns: [{
                dataField: "SegmentId",
                visible: false,
                caption: "Id"
            }, {
                dataField: "SegmentName",
                caption: "Segment",
                fixed: true
            }, {
                dataField: "CustomerCount",
                caption: "Müşteri",
                headerFilter: {
                    groupInterval: 100
                }
            }, {
                dataField: "TotalCustomerCount",
                caption: "Müşteri(Toplam)",
                headerFilter: {
                    groupInterval: 100
                }
            }, {
                dataField: "MachineParkCount",
                caption: "Makine Parkı",
                headerFilter: {
                    groupInterval: 100
                }
            }, {
                dataField: "TotalMachinePark",
                caption: "Makine Parkı(Toplam)",
                headerFilter: {
                    groupInterval: 100
                }
            }],
            summary: {
                totalItems: [
                    {
                        column: "CustomerCount",
                        summaryType: "sum",
                        customizeText: function (data) {
                            return "Toplam: " + data.value;
                        }
                    }, {
                        column: "TotalCustomerCount",
                        summaryType: "sum",
                        customizeText: function (data) {
                            return "Toplam: " + data.value;
                        }
                    }, {
                        column: "MachineParkCount",
                        summaryType: "sum",
                        customizeText: function (data) {
                            return "Toplam: " + data.value;
                        }
                    }, {
                        column: "TotalMachinePark",
                        summaryType: "sum",
                        customizeText: function (data) {
                            return "Toplam: " + data.value;
                        }
                    }]
            }
        };
    }

    $scope.CategorySelectOptions.onDataBound = function () {
        $scope.ReportFilter.Category = $scope.CategorySelectOptions.data[1];
        $scope.BindDataGrid();
    }

    var refresTimeOut = null;
    $scope.$watchCollection('ReportFilter', function (filter) {
        if (refresTimeOut)
            $timeout.cancel(refresTimeOut);
        refresTimeOut = $timeout(function () {
            $scope.GridService.Refresh("grid");
        }, 800);
    });
}]);