﻿'use strict';
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
                return 'ready';
        };

        $scope.showPublish = function() {
            var modalInstance = $modal.open({
                templateUrl: 'app/views/owner/publish.html',
                controller: 'publishOrderController'
            });

            modalInstance.result.then(function (success) {

                listOrders();

                showFindFreight(success.orderId);

            }, function() {
                // nothing to do
            });
        };

        $scope.showFindFreight = function(order) {

            var findModal = $modal.open({
                templateUrl: 'app/views/owner/freights.html',
                controller: 'findFreightUnitController',
                resolve: {
                    orderId: function() {
                        return order.Id;
                    }
                }
            });

            findModal.result.then(
                function() {
                    listOrders();
                },
                function() {
                    // nothing to do
                }
            );
        };

        $scope.showPay = function(order) {
            var payModal = $modal.open({
                templateUrl: 'app/views/owner/pay.html',
                controller: 'payController',
                resolve: {
                    order: function() {
                        return order;
                    }
                }
            });

            payModal.result.then(
                function() {
                    listOrders();
                },
                function() {
                    // nothing to do
                }
            );
        };

        $scope.showConsign = function(order) {
            var consignModal = $modal.open({
                templateUrl: 'app/views/owner/consign.html',
                controller: 'consignController',
                resolve: {
                    order: function() {
                        return order;
                    }
                }
            });

            consignModal.result.then(
                function() {
                    listOrders();
                },
                function() {
                    // nothing to do
                }
            );
        };

        $scope.showEvaluate = function (order) {
            var evaluateModal = $modal.open({
                templateUrl: 'app/views/owner/evaluate.html',
                controller: 'evaluateController',
                resolve: {
                    order: function () {
                        return order;
                    }
                }
            });

            evaluateModal.result.then(
                function () {
                    listOrders();
                },
                function () {
                    // nothing to do
                }
            );
        };
    }
]);

app.controller('publishOrderController', ['$scope', '$modalInstance', 'ownerService', function ($scope, $modalInstance, ownerService) {

    $scope.publishModel = {
        Description: "",
        Destination: "",
        Source: "",
        Size: 0.0,
        Weight: 0.0
    };

    $scope.close = function() {
        $modalInstance.dismiss('cancel');
    };

    $scope.publish = function() {

        ownerService.publish($scope.publishModel).then(function(response) {
            $scope.close();

        }, function(error) {
            alert(YouHuHelper.errorHelper.getErrorMsg(error));
        });
    };

}]);

app.controller('findFreightUnitController', ['$scope','$modal','$findModal','order','ownerService', function($scope,$modal, $findModal,order, ownerService) {

    $scope.freightUnits = [];
    $scope.freightUnitsCount = 0;

    init();

    function init() {
        listFreightUnits();
    }

    function listFreightUnits() {
        ownerService.find(order.Id)
            .success(function(success) {
                $scope.freightUnits = success;
                $scope.freightUnitsCount = success.length;

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
        $findModal.dismiss('cancel');
    };

    $scope.showMakeDeal = function(freightUnit) {
        var makeDealModal = $modal.open({
            templateUrl: 'app/views/owner/makeDeal.html',
            contorller: 'makeDealController',
            resolve: {
                order: order,
                freightUnit: freightUnit
            }
        });

        makeDealModal.result.then(function(success) {
            $scope.close();
        }, function() {
            // nothing to do
            alert("Error Occured");
        });
    };
}]);

app.controller('makeDealController', ['$scope', '$makeDealModal', 'order','freightUnit', 'ownerService', function($scope, $makeDealModal, order,freightUnit, ownerService) {
    $scope.makeDealModel = {
        OrderId: 0,
        FreightUnitId: 0
    }

    init();

    function init() {

        $scope.makeDealModel.OrderId = order.Id;
        $scope.makeDealModel.FreightUnitId = freightUnit.Id;
    }

    $scope.close = function() {
        $makeDealModal.dismiss('cancel');
    };

    $scope.makeDeal = function() {
        ownerService.makeDeal($scope.makeDealModel).success(function(response) {
                alert("Make Deal Successfully");
                $scope.close();
            })
            .error(function(error) {
                alert(YouHuHelper.errorHelper.getErrorMsg(error));
            });
    };
}]);

app.controller('payController', ['$scope','$payModal','order', 'ownerService', function ($scope,$payModal,order, ownerService) {
    $scope.payModel = {
        OrderId: 0,
        PaymentCode: "",
        Paid: 0.0
    };

    $scope.freightCost = 0.0;

    init();

    function init() {
        $scope.payModel.OrderId = order.Id;
        $scope.freightCost = order.FreightCost;
    }

    $scope.close = function() {
        $payModal.dismiss('cancel');
    };

    $scope.pay = function() {
        ownerService.pay($scope.payModel)
            .success(function(success) {
                alert("Pay Successfully");
                $scope.close();
            })
            .error(function(error) {
                alert(YouHuHelper.errorHelper.getErrorMsg(error));
            });
    };
}]);

app.controller('cosignController', ['$scope','$consignModal','order','ownerService', function ($scope,$consignModal,order, ownerService) {
    $scope.consignModel = {
        OrderId: 0,
        PaymentCode: ""
    };

    init();

    function init() {
        $scope.consignModel.OrderId = order.Id;
    }

    $scope.close= function() {
        $consignModal.dismiss('cancel');
    }

    $scope.consign= function() {
        ownerService.consign($scope.consignModel)
        .success(function(success) {
            alert("Consign Successfully");
                $scope.close();
            })
        .error(function(error) {
            alert(YouHuHelper.errorHelper.getErrorMsg(error));
        });
    }
}]);

app.controller('evaluateController', ['$scope', '$evaluateModal', 'order', 'ownerService', function ($scope, $evaluateModal, order, ownerService) {

    $scope.evaluateModel = {
        Comments: "",
        Rank: 0,
        OrderId:0
    }

    init();

    function init() {
        $scope.evaluateModel.OrderId = order.Id;
    }

    $scope.close= function () {
        $evaluateModal.dismiss('cancel');
    }

    $scope.evaluate=function () {
        ownerService.evaluate($scope.evaluateModel)
        .success(function (success) {
            alert("Consign Successfully");
            $scope.close();
        })
            .error(function (error) {
                alert(YouHuHelper.errorHelper.getErrorMsg(error));
            });
    }
}
]);
