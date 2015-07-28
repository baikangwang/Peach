'use strict';
app.controller('driverOrdersController', ['$scope', '$location', '$modal', 'driverService', function ($scope, $location, $modal, driverService) {
        
        $scope.orders = [];
        $scope.orderCount = 0;
        //    function () {
        //    return $scope.orders.length;
        //};

        init();

        function init() {
            listOrders();
        }

        function listOrders() {

            driverService.list()
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

            //Ready = 0,
            //Dealing = 1,
            //Rejected = 2,
            //Dealt = 3,
            //Paying = 4,
            //Paid = 5,
            //InProgress = 6,
            //Arrived = 7,
            //Consigned = 8

            if (order.State) {
                switch (order.State) {
                case 0:
                    return "ready";
                case 1:
                    return "dealing";
                case 2:
                    return "rejected";
                case 3:
                    return "dealt";
                case 4:
                    return "paying";
                case 5:
                    return "paid";
                case 6:
                    return "inprogress";
                case 7:
                    return "arrived";
                case 8:
                    return "consigned";
                default:
                    return "ready";
                }

            } else
                return "ready";
        };

        $scope.showConfirmDeal = function(order) {
            var confirmDealModal = $modal.open({
                templateUrl: 'app/views/driver/confirmDeal.html',
                controller: 'confirmDealController',
                resolve: {
                    order: function () {
                        return order;
                    }
                }
            });

            confirmDealModal.result.then(function (success) {
                listOrders();
            }, function() {
                // nothing to do
            });
        };

        $scope.showUpdateState = function(order) {

            var updateStateModal = $modal.open({
                templateUrl: 'app/views/driver/updateState.html',
                controller: 'updateStateController',
                resolve: {
                    order: function () {
                        return order;
                    }
                }
            });

            updateStateModal.result.then(
                function() {
                    listOrders();
                },
                function() {
                    // nothing to do
                }
            );
        };

        $scope.showEvaluate = function(order) {
            var evaluateModal = $modal.open({
                templateUrl: 'app/views/driver/evaluate.html',
                controller: 'evaluateController',
                resolve: {
                    order: function () {
                        return order;
                    }
                }
            });

            evaluateModal.result.then(
                function() {
                    listOrders();
                },
                function() {
                    // nothing to do
                }
            );
        };
    }]);

app.controller('confirmDealController', ['$scope', '$confirmDealModal','order','driverService', function ($scope, $confirmDealModal,order, driverService) {

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
        $confirmDealModal.dismiss('cancel');
    };

    $scope.confirmDeal = function() {

        driverService.confirmDeal($scope.confirmDealModel).then(function (response) {
            $scope.close();

        }, function(error) {
            var msg;
            if (error.ExceptionMessage)
                msg = error.ExceptionMessage;
            else if (error.Message)
                msg = error.Message;
            else
                msg = 'Error occured';
            alert(msg);
        });
    };

}]);

app.controller('updateStateController', ['$scope', '$updateStateModal', 'order', 'driverService', function ($scope, $updateStateModal, order, driverService) {
    $scope.updateStateModel = {
        OrderId: 0,
        Location: ""
    }

    init();

    function init() {
        $scope.updateStateModel.OrderId = order.Id;
    }

    $scope.close = function() {
        $updateStateModal.dismiss('cancel');
    };

    $scope.updateState = function() {
        driverService.updateOrderState($scope.updateStateModel).success(function (response) {
                alert("Update order state Successfully");
                $scope.close();
            })
            .error(function(error) {
                var msg;
                if (error.ExceptionMessage)
                    msg = error.ExceptionMessage;
                else if (error.Message)
                    msg = error.Message;
                else
                    msg = 'Error occured';
                alert(msg);
            });
    };
}]);

app.controller('evaluateController', ['$scope', '$evaluateModal', 'order', 'driverService', function ($scope, $evaluateModal, order, driverService) {
    $scope.evaluateModel = {
        OrderId: 0,
        Rank: 0,
        Comments: ""
    };

    init();

    function init() {
        $scope.evaluateModel.OrderId = order.Id;
    }

    $scope.close = function() {
        $evaluateModal.dismiss('cancel');
    };

    $scope.evaluate = function() {
        driverService.evaluate($scope.evaluateModel)
            .success(function(success) {
                alert("Evaluate Successfully");
                $scope.close();
            })
            .error(function(error) {
                var msg;
                if (error.ExceptionMessage)
                    msg = error.ExceptionMessage;
                else if (error.Message)
                    msg = error.Message;
                else
                    msg = 'Error occured';
                alert(msg);
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
        driverService.listOrders()
            .success(function (success) {
                $scope.freightUnits = success;
                $scope.freightUnitsCount = success.length;

            })
            .error(function (error) {
                $scope.freightUnits = [];
                $scope.freightUnitsCount = 0;
                var msg;
                if (error.ExceptionMessage)
                    msg = error.ExceptionMessage;
                else if (error.Message)
                    msg = error.Message;
                else
                    msg = 'Error occured';
                alert(msg);
            });
    };

    $scope.refresh = function () {
        listFreightUnits();
    };

    $scope.showPublish = function (freightUnit) {
        var publishModal = $modal.open({
            templateUrl: 'app/views/driver/publish.html',
            contorller: 'publishFreightUnitController',
            resolve: {
                order: order,
                freightUnit: freightUnit
            }
        });

        publishModal.result.then(function (success) {
            $scope.close();
        }, function () {
            // nothing to do
            alert("Error Occured");
        });
    };
}]);

app.controller('publishFreightUnitController', ['$scope', '$publishModal', 'freightUnit','driverService',function ($scope, $publishModal, freightUnit,driverService)
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
        $publishModal.dismiss('cancel');
    };

    $scope.publish = function () {
        driverService.publish($scope.publishModel)
            .success(function (success) {
                alert("Publish Successfully");
                $scope.close();
            })
            .error(function (error) {
                var msg;
                if (error.ExceptionMessage)
                    msg = error.ExceptionMessage;
                else if (error.Message)
                    msg = error.Message;
                else
                    msg = 'Error occured';
                alert(msg);
            });
    };
}]);
