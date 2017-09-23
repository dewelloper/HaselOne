HaselApp.service("CustomerService", ["$http", "$rootScope", "$q",
function ($http, $rootScope, $q) {
    this.GetCustomer = function (filter) {
        return $rootScope.AjaxPost('/Customer/Get', { filter: angular.copy(filter) });
    }

    this.GetCustomerOptions = function () {
        return $rootScope.AjaxPost('/Customer/GetCustomerOptions', { comboId: 1 });
    }  

    this.GetSalesmanList = function (params) {
        return $rootScope.AjaxPost('/Salesman/GetList', { filter: params });
    }  
     
    this.GetRequest = function (params) {
        return $rootScope.AjaxPost('/CustomerRequest/Get', { customerRequestFilter: params });
    }
  
    this.CustomerRequest_CopyMp = function (MpId, Count) {
         return $rootScope.AjaxPost('/CustomerRequest/CopyMp', { MachineParkId: MpId, Count:Count });
    }
  

    this.GetLocationLookup = function (filter, success, error) {
        $http.post('/Location/Lookup', { filter: filter }).then(
                function (response) {
                    if (success)
                        success(response.data);
                }, error);
    }

    
    this.GetLocations = function (filter, success, error) {
        
        $http.post('/Location/Get', { filter: filter }).then(
                function (response) {
                    if (success)
                        success(response.data);
                }, error);
    }

    this.GetSectors = function (success, error) {
        $http.post('/Sector/Get').then(
                function (response) {
                    if (success)
                        success(response.data);
                }, error);
    }

    this.MpFullUpdatePanelModal = function (machineParkId) {
        //debugger;
        var deferred = $q.defer();
        var modalInstance = $rootScope.OpenModal({
            templateUrl: '/Static/Request/MpFullUpdatePanel.html?v='+ $rootScope.makeId(),
            resolve: {
                Data: function () {
                    return machineParkId;
                }
            },
            controller: ["$scope", "$timeout", "$http", "Data", "$uibModalInstance", "CustomerService","MachineparkService",
                function ($scope, $timeout, $http, Data, $uibModalInstance, CustomerService, MachineparkService) {
                    
                    var filter = angular.copy(MachineparkFilter);
                    filter.Id = Data;
                    filter.CustomerId = $rootScope.getQueryStringByName("cid");
                    
                    $scope.Data = {};
                    $scope.Data.MpId = machineParkId;
                    $scope.Data.Message = "Tüm makinelerin Marka ve Modelini değiştir";

                    if (typeof $scope.Data.Title === 'undefined')
                        $scope.Data.Title = 'Toplu Güncelleme';
                     
                    $scope.Close = function () {
                        $uibModalInstance.dismiss();
                    }

                    
                    MachineparkService.GetMachinepark(filter).then(function (res) {
                       
                        var mp = res.Data[0];

                        //get marka
                        var Markfilter = angular.copy(MachineparkMarkFilter);
                        Markfilter.CategoryId = mp.CategoryId;
                        MachineparkService.GetMachineparkMark(Markfilter).then(function (res) {
                            debugger;
                            $scope.MpMark_SelectOptions.DataSource = res.Data;
                            $scope.MpMark_SelectOptions.GetData();
                        });
                    });
                    
                    var confirmText = "Tüm makine parklarının marka ve modelinin değiştirmek emin misiniz?";

                    $scope.Save = function () {
                        $scope.AlertService.Confirm(confirmText, null, function (res) {
                            if (res) {
                                debugger;
                            }
                        });
                    }
                }]
        });
        return deferred.promise;
    }
}
]);