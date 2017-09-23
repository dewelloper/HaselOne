HaselApp.controller('CategoryManagementController', ['$scope', '$http', '$timeout', "$q", function ($scope, $http, $timeout, $q) {
    $scope.SourceDestChain = [];

    $scope.GetCategories = function (success, error) {
        $http.post('/CategoryManagement/GetCategoriesAll').then(
    function (response) {
        if (response.data.IsSuccess)
            $scope.list = response.data.Data;
    }
    , function error(e) { console.log("GetCategoriesAll error", e) });
    }
    $scope.GetCategories();
    $scope.toggle = function (scope) {
        scope.toggle();
    };
    $scope.collapseAll = function () {
        $scope.$broadcast('angular-ui-tree:collapse-all');
    };

    $scope.expandAll = function () {
        $scope.$broadcast('angular-ui-tree:expand-all');
    };

    $scope.UpdateCategory = function (sourceId, destId, sindex, dindex) {
        $http.post('/CategoryManagement/SetCategoryByNodeId', { source: sourceId, dest: destId, sourceIndex: sindex, destIndex: dindex }).then(
        function (response) {
            $timeout(function () {
                $scope.collapseAll();
            }, 200);
        }
        , function error(e) { console.log("SetCategoryByNodeId error", e) });
        console.log("source/dest", sourceId, destId);
    }

    $scope.Add = function () {
        $http.post('/CategoryManagement/AddNewCategory', { newCategoryName: $scope.NewCategoryName, parentId: $scope.selectedCategoryId }).then(
        function (response) {
            $scope.GetCategories();
            $timeout(function () {
                $scope.collapseAll();
            }, 200);
        }
        , function error(e) { console.log("AddNewCategory error", e) });
        console.log("catName/parentId", $scope.NewCategoryName, $scope.selectedCategoryId);
    }
    $scope.NewCategoryName = '';

    $scope.Delete = function () {
        var deferred = $q.defer();
        $scope.AlertService.Confirm("\"" + $scope.selectedCategoryName + "\" kategorisini silmek istediğinizden eminmisiniz (!Dikkat bu değişiklik geri alınamaz)?", "", function (result) {
            if (result) {

                $http.post('/CategoryManagement/DeleteCategory', { desCategoryId: $scope.selectedCategoryId }).then(
                function (response) {
                    $scope.GetCategories();
                    $timeout(function () {
                        $scope.collapseAll();
                    }, 200);
                }
                , function error(e) { console.log("DeleteCategory error", e) });
                console.log("desCategoryId", $scope.selectedCategoryId);

            }
            deferred.resolve(result);
        });
        return deferred.promise;
    }

    $scope.Reload = function ReloadCategories() {
        $scope.GetCategories();
        $timeout(function () {
            $scope.collapseAll();
        }, 200);
    }

    $scope.saveChanges = function () {
        if ($scope.SourceDestChain.length > 0) {
            var deferred = $q.defer();
            $scope.AlertService.Confirm("Kategori hiyerarşisi üzerinde yaptığınız değişiklikleri kaydetmek istediğinizden emin misiniz?", "", function (result) {
                if (result) {
                    for (var h = 0; h < $scope.SourceDestChain.length; h++) {
                        $scope.UpdateCategory($scope.SourceDestChain[h].SourceId, $scope.SourceDestChain[h].DestId, $scope.SourceDestChain[h].SourceIndex, $scope.SourceDestChain[h].DestIndex);
                    }
                    $scope.SourceDestChain = [];
                }
                deferred.resolve(result);
            });
            return deferred.promise;
        }
    }

    $scope.treeOptions = {
        beforeDrop: function (e) {
            $scope.selectedCategoryName = e.source.nodeScope.$modelValue.CategoryName;
            $scope.selectedCategoryId = e.source.nodeScope.$modelValue.Id;
            var sourceId = 0;
            var destId = 0;
            if (typeof e.source !== 'undefined'
                    && typeof e.dest !== 'undefined'
                    && typeof e.source.nodeScope !== 'undefined'
                    && typeof e.dest.nodesScope !== 'undefined'
                    && typeof e.source.nodeScope.$modelValue !== 'undefined'
                    && typeof e.dest.nodesScope.item !== 'undefined') {
                sourceId = e.source.nodeScope.$modelValue.Id;
                destId = e.dest.nodesScope.item.Id;
            }
            if (sourceId != destId) {
                var sdc = { SourceId: sourceId, DestId: destId, SourceIndex: e.source.index, DestIndex: e.dest.index };
                $scope.SourceDestChain.push(sdc);
            }
            else {
                return false;
            }
        }
    };
    $timeout(function () {
        $scope.collapseAll();
    }, 200);
}]);

