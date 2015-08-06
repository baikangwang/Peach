'use strict';
app.controller('driverOrdersController', ['$scope', '$location', '$modal', 'driverService', function ($scope, $location, $modal, driverService) {
        
        $scope.orders = [];
        $scope.orderCount = 0;

        init();

        function init() {
            listOrders();
        }

        function listOrders() {

            driverService.listOrders()
                .success(function(response) {
                    $scope.orders = response;
                    $scope.orderCount = $scope.orders.length;
                })
                .error(function(response) {
                    $scope.orders = [];
                    $scope.orderCount = 0;
                });
        }

        $scope.refresh = function() {
            listOrders();
        };

        $scope.getState = function(order) {

            if (order.State) {
                return YouHuHelper.orderStateHelper.toLabel(order.State);
            } else
                return "未接单";
        };

        $scope.getStateStr = function (order) {
            if (order.State) {
                return YouHuHelper.orderStateHelper.toValue(order.State);
            } else
                return "ready";
        };

        $scope.showConfirmDeal = function(order) {
            var modalInstance = $modal.open({
                templateUrl: 'app/views/driver/confirmDeal.html',
                controller: 'confirmDealController',
                resolve: {
                    order: function () {
                        return order;
                    }
                }
            });

            modalInstance.result.then(function (success) {
                $scope.refresh();
            }, function() {
                // nothing to do
            });
        };

        $scope.showUpdateState = function(order) {

            var modalInstance = $modal.open({
                templateUrl: 'app/views/driver/updateState.html',
                controller: 'updateStateController',
                resolve: {
                    order: function () {
                        return order;
                    }
                }
            });

            modalInstance.result.then(
                function() {
                    $scope.refresh();
                },
                function() {
                    // nothing to do
                }
            );
        };

        $scope.showEvaluate = function(order) {
            var modalInstance = $modal.open({
                templateUrl: 'app/views/driver/evaluate.html',
                controller: 'evaluateController',
                resolve: {
                    order: function () {
                        return order;
                    }
                }
            });

            modalInstance.result.then(
                function() {
                    $scope.refresh();
                },
                function() {
                    // nothing to do
                }
            );
        };

        $scope.getCSS = function (order) {
            return YouHuHelper.orderStateHelper.toCSS(order.State);
        }

    }]);

app.controller('confirmDealController', ['$scope', '$modalInstance', 'order', 'driverService', function ($scope, $modalInstance, order, driverService) {

    $scope.confirmDealModel = {
        AcceptedId: 0,
        FreightCost: 0.0
    };

    init();

    function init() {
        $scope.confirmDealModel.AcceptedId = order.Id;
        $scope.confirmDealModel.FreightCost = order.FreightCost;
    }

    $scope.close = function() {
        $modalInstance.dismiss('cancel');
    };
    
    $scope.confirmDeal = function() {

        driverService.confirmDeal($scope.confirmDealModel).then(function (response) {
            $modalInstance.close();

        }, function(error) {
            alert(YouHuHelper.errorHelper.getErrorMsg(error));
        });
    };

}]);

app.controller('updateStateController', ['$scope', '$modalInstance', 'order', 'driverService', function ($scope, $modalInstance, order, driverService) {
    $scope.updateStateModel = {
        OrderId: 0,
        Location: ""
    }

    init();

    function init() {
        $scope.updateStateModel.OrderId = order.Id;
    }

    $scope.close = function() {
        $modalInstance.dismiss('cancel');
    };

    $scope.updateState = function() {
        driverService.updateOrderState($scope.updateStateModel).success(function (response) {
                alert("Update order state Successfully");
                $modalInstance.close();
            })
            .error(function(error) {
                alert(YouHuHelper.errorHelper.getErrorMsg(error));
            });
    };
}]);

app.controller('evaluateController', ['$scope', '$modalInstance', 'order', 'driverService', function ($scope, $modalInstance, order, driverService) {
    $scope.evaluateModel = {
        OrderId: 0,
        Rank: "",//0,
        Comments: ""
    };

    init();

    function init() {
        $scope.evaluateModel.OrderId = order.Id;
    }

    $scope.close = function() {
        $modalInstance.dismiss('cancel');
    };

    $scope.evaluate = function() {
        driverService.evaluate($scope.evaluateModel)
            .success(function(success) {
                alert("Evaluate Successfully");
                $modalInstance.close();
            })
            .error(function(error) {
                alert(YouHuHelper.errorHelper.getErrorMsg(error));
            });
    };
}]);

app.controller('freightUnitsController', ['$scope','$modal','driverService', function ($scope,$modal, driverService) {

    $scope.freightUnits = [];
    $scope.freightUnitsCount = 0;

    init();

    function init() {
        listFreightUnits();
    }

    function listFreightUnits() {
        driverService.listFreights()
            .success(function (success) {
                $scope.freightUnits = success;
                $scope.freightUnitsCount = success.length;

            })
            .error(function (error) {
                $scope.freightUnits = [];
                $scope.freightUnitsCount = 0;
                alert(YouHuHelper.errorHelper.getErrorMsg(error));
            });
    };

    $scope.refresh = function () {
        listFreightUnits();
    };

    $scope.showPublish = function (freightUnit) {
        var modalInstance = $modal.open({
            templateUrl: 'app/views/driver/publish.html',
            controller: 'publishFreightUnitController',
            resolve: {
                freightUnit: function () { return freightUnit }
            }
        });

        modalInstance.result.then(function (success) {
            $scope.refresh();
        }, function () {
            // nothing to do
        });
    };

    $scope.showRegister=function () {
        var modalInstance = $modal.open({
            templateUrl: 'app/views/driver/register.html',
            controller: 'registerFreightUnitController'
        });

        modalInstance.result.then(function (success) {
            $scope.refresh();
        }, function () {
            // nothing to do
        });
    }

    $scope.getState=function (freightUnit) {
        return YouHuHelper.freightUnitStateHelper.toLabel(freightUnit.State);
    }

    $scope.getCSS=function (freightUnit) {
        var state = $scope.getState(freightUnit);
        return YouHuHelper.freightUnitStateHelper.toCSS(state);
    }
}]);

app.controller('publishFreightUnitController', ['$scope', '$modalInstance', 'freightUnit', 'driverService', function ($scope, $modalInstance, freightUnit, driverService)
{
    $scope.publishModel = {
        Id: 0,
        Location: ""
    };

    init();

    function init() {
        $scope.publishModel.Id = freightUnit.Id;
    }

    $scope.close = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.publish = function () {
        driverService.publish($scope.publishModel)
            .success(function (success) {
                alert("Publish Successfully");
                $modalInstance.close();
            })
            .error(function (error) {
                alert(YouHuHelper.errorHelper.getErrorMsg(error));
            });
    };
}]);

app.controller('registerFreightUnitController', ['$scope', '$modalInstance', 'driverService', function ($scope, $modalInstance, driverService) {

    $scope.registerModel = {
        Location: "",
        Height: "",//0.0,
        Length: "",//0.0,
        Licence: "",
        Type: 0,
        Weight: ""//0.0
    };

    $scope.close = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.register = function () {
        driverService.register($scope.registerModel)
            .success(function (success) {
                alert("Register Successfully");
                $modalInstance.close();
            })
            .error(function (error) {
                alert(YouHuHelper.errorHelper.getErrorMsg(error));
            });
    }
}]);