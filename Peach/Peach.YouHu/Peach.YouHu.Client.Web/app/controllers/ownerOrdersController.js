'use strict';
app.controller('ownerOrdersController', ['$scope', '$location','$modal', 'ownerService', function ($scope, $location, $modal, ownerService) {
        
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

app.controller('findFreightUnitController', ['$scope','$modal','$findModal','order','ownerService', function($scope,$modal, $findModal,order, ownerService) {

    $scope.freightUnits = [];

    init();

    function init() {
        listFreightUnits();
    }

    function listFreightUnits() {
        ownerService.find(order.Id)
            .success(function(success) {
                $scope.freightUnits = success;
            })
            .error(function(error) {
                $scope.freightUnits = [];
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
        orderId: 0,
        freightUnitId: 0
    }

    init();

    function init() {

        $scope.makeDealModel.orderId = order.Id;
        $scope.makeDealModel.freightUnitId = freightUnit.Id;
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

app.controller('payController', ['$scope','$payModal','order', 'ownerService', function ($scope,$payModal,order, ownerService) {
    $scope.payModel = {
        orderId: 0,
        paymentCode: "",
        freightCost: 0.0,
        paid: 0.0
    };

    init();

    function init() {
        $scope.payModel.orderId = order.Id;
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

app.controller('cosignController', ['$scope','$consignModal','order','ownerService', function ($scope,$consignModal,order, ownerService) {
    $scope.consignModel = {
        orderId: 0,
        paymentCode: ""
    };

    init();

    function init() {
        $scope.consignModel.orderId = order.Id;
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
            var msg;
            if (error.ExceptionMessage)
                msg = error.ExceptionMessage;
            else if (error.Message)
                msg = error.Message;
            else
                msg = 'Error occured';
            alert(msg);
        });
    }
}]);
