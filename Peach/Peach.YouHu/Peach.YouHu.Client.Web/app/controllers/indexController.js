'use strict';
app.controller('indexController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {
    
    $scope.authentication = {};

    init();

    function init() {
        $scope.authentication = authService.authentication;
    }

    $scope.logOut = function () {
        authService.logOut();
        $scope.authentication = authService.authentication;
        $location.path('/home');
    }



}]);