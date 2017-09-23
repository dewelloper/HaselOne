HaselApp.service("MachineparkService", ["$http", "$rootScope", "$q",
function ($http, $rootScope, $q) {
    this.GetMachineparkYears = function (filter, success, error) {
        return $rootScope.AjaxPost('/MachineparkYear/MachineparkYear', { filter: angular.copy(filter) });
    }

    this.SaveMachinepark = function (entity) {
        return $rootScope.AjaxPost('/Machinepark/Save', { entity: angular.copy(entity) });
    }

    this.CopyMachinepark = function (id,customerId, count) {
        return $rootScope.AjaxPost('/Machinepark/Copy', { id: id, customerId: customerId, count: count });
    }

    this.GetMachinepark = function (filter) {
        return $rootScope.AjaxPost('/Machinepark/Get', { filter: angular.copy(filter) });
    }

    this.DeleteMachinepark = function (ids) {
        return $rootScope.AjaxPost('/Machinepark/Delete', { ids: angular.copy(ids) });
    }
    this.ReleaseMachinepark = function (ids, date) {
        return $rootScope.AjaxPost('/Machinepark/Release', { ids: ids, date: date });
    }
    this.GetMachineparkCategory = function (filter) {
        return $rootScope.AjaxPost('/MachineparkCategory/Get', { filter: angular.copy(filter) });
    }
    this.SaveMachineparkCategory = function (entity) {
        return $rootScope.AjaxPost('/MachineparkCategory/Save', { entity: angular.copy(entity) });
    }
    this.GetMachineModel = function (filter) {
        return $rootScope.AjaxPost('/MachineModel/Get', { filter: angular.copy(filter) });
    }
    this.SaveMachineModel = function (entity) {
        return $rootScope.AjaxPost('/MachineModel/Save', { entity: angular.copy(entity) });
    }
    this.GetMachineparkMark = function (filter) {
        return $rootScope.AjaxPost('/MachineparkMark/Get', { filter: angular.copy(filter) });
    }
    this.SaveMachineparkMark = function (entity) {
        return $rootScope.AjaxPost('/MachineparkMark/Save', { entity: angular.copy(entity) });
    }
    this.OpenReleasePanel = function (data) {
        var deferred = $q.defer();
        var modalInstance = $rootScope.OpenModal({
            templateUrl: '/Static/Machinepark/ReleaseMachineparkPanel.html?v=2',
            resolve: {
                Data: function () {
                    return data;
                }
            },
            controller: ["$scope", "$timeout", "$http", "Data", "$uibModalInstance", "MachineparkService",
             function ($scope, $timeout, $http, Data, $uibModalInstance, MachineparkService) {
                 $scope.Data = Data;
                 if (typeof $scope.Data.Title === 'undefined')
                     $scope.Data.Title = 'Elden Çıkarma Modülü';
                 $scope.Close = function () {
                     $uibModalInstance.dismiss();
                 }
                 var confirmText = "Seçili makine parklarını, " + moment($scope.Data.ReleaseDate).format("DD.MM.YYYY") + " tarihi için elden çıkarmak istediğinize emin misiniz?";
                 $scope.Save = function () {
                     $scope.AlertService.Confirm(confirmText, null, function (res) {
                         if (res) {
                             MachineparkService.ReleaseMachinepark(Data.Ids, Data.ReleaseDate)
                             .then(function (res) {
                                 deferred.resolve(res);
                                 $scope.Close();
                             });
                         }
                     });
                 }
             }]
        });
        return deferred.promise;
    }
}
]);