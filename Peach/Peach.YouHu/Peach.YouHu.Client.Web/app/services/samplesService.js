'use strict';
app.factory('samplesService', ['$http', function ($http) {

    var serviceBase = 'http://localhost:20268/';
    var samplesServiceFactory = {};

    var _getOrders = function () {

        return $http.get(serviceBase + 'api/samples').then(function (results) {
            return results;
        });
    };

    samplesServiceFactory.getOrders = _getOrders;

    return samplesServiceFactory;

}]);