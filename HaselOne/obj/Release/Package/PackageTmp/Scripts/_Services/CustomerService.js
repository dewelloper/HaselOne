HaselApp.service("CustomerService", ["$http", "$rootScope",
function ($http, $rootScope) {
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
}
]);