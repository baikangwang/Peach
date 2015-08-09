﻿'use strict';
app.factory('ownerService', ['$http', function ($http) {

    var serviceBase = 'http://nltc369:8080';//'http://180.76.137.222:8080/';
        var ownerServiceFactory = {};

        var _publish = function(publish) {
            return $http.post(serviceBase + 'api/Owner/Orders/Publish', publish)
                .success(function(response) {
                    return response;
                })
                .error(function(response) {
                    return response;
                });
        };

        var _list = function() {

            return $http.get(serviceBase + 'api/Owner/Orders/List')
                .success(function(response) {
                    return response;
                })
                .error(function(response) {
                    return response;
                });
        };

        var _makeDeal = function(deal) {
            return $http.post(serviceBase + 'api/Owner/Orders/MakeDeal', deal)
                .success(function(response) {
                    return response;
                })
                .error(function(response) {
                    return response;
                });
        };

        var _pay = function(payment) {
            return $http.post(serviceBase + 'api/Owner/Orders/Pay', payment)
                .success(function(response) {
                    return response;
                })
                .error(function(response) {
                    return response;
                });
        };

        var _consign = function(consign) {
            return $http.post(serviceBase + 'api/Owner/Orders/Consign', consign)
                .success(function(response) {
                    return response;
                })
                .error(function(response) {
                    return response;
                });
        };

        var _find = function(orderId) {
            return $http(
            {
                url: serviceBase + 'api/Owner/FreightUnits/Find',
                method: "GET",
                params: { Id: orderId }
            }).success(function (response) {
                return response;
            }).error(function (response) {
                return response;
            });
        };

        var _evaluate = function (evaluate) {
            return $http.post(serviceBase + 'api/Owner/Evaluations/Evaluate', evaluate)
                .success(function (response) {
                    return response;
                })
                .error(function (response) {
                    return response;
                });
        };

        ownerServiceFactory.publish = _publish;
        ownerServiceFactory.list = _list;
        ownerServiceFactory.makeDeal = _makeDeal;
        ownerServiceFactory.pay = _pay;
        ownerServiceFactory.consign = _consign;
        ownerServiceFactory.findFrieghtUnit = _find;
        ownerServiceFactory.evaluate = _evaluate;

        return ownerServiceFactory;
    }
]);