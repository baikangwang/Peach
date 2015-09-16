'use strict';
app.controller('ownerOrdersController', ['$scope', '$location','$modal', 'ownerService', function ($scope, $location, $modal, ownerService) {
        
        $scope.orders = [];
        $scope.orderCount = 0;

        init();

        function init() {
            listOrders();
        }

        function listOrders() {

            ownerService.list()
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
            }
            else
                return '待接单';
        };

        $scope.getStateStr = function (order) {
            if (order.State) {
                return YouHuHelper.orderStateHelper.toValue(order.State);
            } else
                return "ready";
        };

        $scope.showPublish = function() {
            var modalInstance = $modal.open({
                templateUrl: 'app/views/owner/publish.html',
                controller: 'publishOrderController'
            });

            modalInstance.result.then(function (success) {

                $scope.refresh();

                showFindFreight(success.orderId);

            }, function() {
                // nothing to do
            });
        };

        $scope.showFindFreight = function(order) {

            var modalInstance = $modal.open({
                templateUrl: 'app/views/owner/freights.html',
                controller: 'findFreightUnitController',
                size:'lg',
                resolve: {
                    order: function() {
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

        $scope.showPay = function(order) {
            var modalInstance = $modal.open({
                templateUrl: 'app/views/owner/pay.html',
                controller: 'payController',
                resolve: {
                    order: function() {
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

        $scope.showEvaluate = function (order) {
            var modalInstance = $modal.open({
                templateUrl: 'app/views/owner/evaluate.html',
                controller: 'evaluateController',
                resolve: {
                    order: function () {
                        return order;
                    }
                }
            });

            modalInstance.result.then(
                function () {
                    $scope.refresh();
                },
                function () {
                    // nothing to do
                }
            );
        };

        $scope.getCSS = function (order) {
            return YouHuHelper.orderStateHelper.toCSS(order.State);
        }
    }
]);

app.controller('publishOrderController', ['$scope', '$modalInstance', 'ownerService', function ($scope, $modalInstance, ownerService) {

    $scope.model = {
        Description: "",
        Destination: "",
        Source: "",
        Size: "",//0.0,
        Weight: ""//0.0
    };

    $scope.close = function() {
        $modalInstance.dismiss('cancel');
    };

    $scope.publish = function() {
        ownerService.publish($scope.model).then(function(response) {
            $modalInstance.close();

        }, function(error) {
            alert(YouHuHelper.errorHelper.getErrorMsg(error));
        });
    };

}]);

app.controller('findFreightUnitController', ['$scope', '$modal', '$modalInstance', 'order', 'ownerService', function ($scope, $modal, $modalInstance, order, ownerService) {

    $scope.freightUnits = [];
    $scope.freightUnitsCount = 0;
    $scope.Order = {};

    init();

    function init() {
        listFreightUnits();
        $scope.Order = order;
    }

    function listFreightUnits() {
        ownerService.findFrieghtUnit(order.Id)
            .success(function(success) {
                $scope.freightUnits = success;
                $scope.freightUnitsCount = success.length;
                $scope.load();
            })
            .error(function(error) {
                $scope.freightUnits = [];
                $scope.freightUnitsCount = 0;
                alert(YouHuHelper.errorHelper.getErrorMsg(error));
            });
    };

    $scope.refresh = function() {
        listFreightUnits();
    };

    $scope.close = function() {
        $modalInstance.dismiss('cancel');
    };

    $scope.showMakeDeal = function(freightUnit) {
        var modalInstance = $modal.open({
            templateUrl: 'app/views/owner/makeDeal.html',
            controller: 'makeDealController',
            resolve: {
                order: function () { return order; },
                freightUnit: function () { return freightUnit }
            }
        });

        modalInstance.result.then(function (success) {
            $modalInstance.close();
        }, function() {
            // nothing to do
        });
    };
}]);

app.controller('makeDealController', ['$scope', '$modalInstance', 'order', 'freightUnit', 'ownerService', function ($scope, $modalInstance, order, freightUnit, ownerService) {

    $scope.makeDealModel = {
        OrderId: 0,
        FreightUnitId: 0
    }

    $scope.freightCost = freightUnit.Cost;

    init();

    function init() {

        $scope.makeDealModel.OrderId = order.Id;
        $scope.makeDealModel.FreightUnitId = freightUnit.Id;
    }

    $scope.close = function() {
        $modalInstance.dismiss('cancel');
    };

    $scope.makeDeal = function() {
        ownerService.makeDeal($scope.makeDealModel).success(function (response) {
            alert("Make Deal Successfully");
            $modalInstance.close();
        })
            .error(function (error) {
                alert(YouHuHelper.errorHelper.getErrorMsg(error));
            });
    };
}]);

app.controller('payController', ['$scope', '$modalInstance', 'order', 'ownerService', function ($scope, $modalInstance, order, ownerService) {
    $scope.payModel = {
        OrderId: 0,
        PaymentCode: "",
        Paid: ""//0.0
    };

    $scope.freightCost = 0.0;

    init();

    function init() {
        $scope.payModel.OrderId = order.Id;
        $scope.freightCost = order.FreightCost;
    }

    $scope.close = function() {
        $modalInstance.dismiss('cancel');
    };

    $scope.pay = function() {
        ownerService.pay($scope.payModel)
            .success(function(success) {
                alert("Pay Successfully");
                $modalInstance.close();
            })
            .error(function(error) {
                alert(YouHuHelper.errorHelper.getErrorMsg(error));
            });
    };
}]);

app.controller('evaluateController', ['$scope', '$modalInstance', 'order', 'ownerService', function ($scope, $modalInstance, order, ownerService) {

    $scope.evaluateModel = {
        Comments: "",
        Rank: "",//0,
        OrderId:0
    }

    init();

    function init() {
        $scope.evaluateModel.OrderId = order.Id;
    }

    $scope.close= function () {
        $modalInstance.dismiss('cancel');
    }

    $scope.evaluate=function () {
        ownerService.evaluate($scope.evaluateModel)
        .success(function (success) {
            alert("Consign Successfully");
            $modalInstance.close();
        })
            .error(function (error) {
                alert(YouHuHelper.errorHelper.getErrorMsg(error));
            });
    }
}
]);
