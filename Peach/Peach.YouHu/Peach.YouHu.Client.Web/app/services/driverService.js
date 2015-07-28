'use strict';
app.factory('driverService', ['$http', function ($http) {

        var serviceBase = 'http://localhost:20268/';
        var driverServiceFactory = {};

        var _publish = function (model) {
            return $http.post(serviceBase + 'api/Driver/FreightUnits/Publish', model)
                .success(function(response) {
                    return response;
                })
                .error(function(response) {
                    return response;
                });
        };

        var _listOrders = function() {

            return $http.get(serviceBase + 'api/Driver/Orders/List')
                .success(function(response) {
                    return response;
                })
                .error(function(response) {
                    return response;
                });
        };

        var _listFreights = function() {
            return $http.post(serviceBase + 'api/Driver/FreightUnits/List')
                .success(function(response) {
                    return response;
                })
                .error(function(response) {
                    return response;
                });
        };

        var _confirmDeal = function(model) {
            return $http.post(serviceBase + 'api/Driver/Orders/ConfirmDeal', model)
                .success(function(response) {
                    return response;
                })
                .error(function(response) {
                    return response;
                });
        };

        var _updateOrderState = function (model) {
            return $http.post(serviceBase + 'api/Driver/Orders/UpdateOrderState', model)
                .success(function(response) {
                    return response;
                })
                .error(function(response) {
                    return response;
                });
        };

        var _evaluate = function (model) {
            return $http.post(serviceBase + 'api/Driver/Evaluations/Evaluate', model)
                .success(function (response) {
                    return response;
                })
                .error(function (response) {
                    return response;
                });
        };

        driverServiceFactory.publish = _publish;
        driverServiceFactory.listOrders = _listOrders;
        driverServiceFactory.listFreights = _listFreights;
        driverServiceFactory.confirmDeal = _confirmDeal;
        driverServiceFactory.updateOrderState = _updateOrderState;
        driverServiceFactory.evaluate = _evaluate;

        return driverServiceFactory;
    }
]);