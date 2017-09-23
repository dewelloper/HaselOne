HaselApp.service("BaseService", ["$http","$rootScope",
function ($http, $rootScope) {
    this.GetCategories = function (params, success, error) {
        $http.post('/Report/GetCategories').then(
    function (response) {
        if (success)
            success(response.data);
    }, error);
    }

    this.GetAreas = function (params, success, error) {
        $http.post('/Report/GetAreas').then(
    function (response) {
        if (success)
            success(response.data);
    }, error);
    }

    this.GetSalesmans = function (params, success, error) {
        $http.post('/Report/GetSalesmans', { keyword: params }).then(
    function (response) {
        if (success)
            success(response.data);
    }, error);
    }

    this.GetMachineparkCategories = function (params, success, error) {
        if ($rootScope.ReportFilter.Category !== null)
            params = $rootScope.ReportFilter.Category.Id;
        $http.post('/Report/GetMachineparkCategories', { categoryId: params }).then(
          function (response) {
              if (success)
                  success(response.data);
          }, error);
    }

    this.GetMarks = function (params, success, error) {
        $http.post('/Report/GetMarks').then(
    function (response) {
        if (success)
            success(response.data);
    }, error);
    }

    this.GetSegments = function (params, success, error) {
        $http.post('/Report/GetSegments').then(
    function (response) {
        if (success)
            success(response.data);
    }, error);
    }
}
]);