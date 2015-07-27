'use strict';
app.controller('loginController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.loginData = {
        userName: "",
        password: ""
    };

    $scope.message = "";

    $scope.login = function () {

        authService.login($scope.loginData).then(

            function (response) {
                $location.path('/' + response.role.toLowerCase() + '/orders');
            },
         function (response) {

             if (response) {
                 var errors = [];
                 for (var key in response.data.ModelState) {
                     for (var i = 0; i < response.data.ModelState[key].length; i++) {
                         errors.push(response.data.ModelState[key][i]);
                     }
                 }

                 $scope.message = "Failed to login due to:" + errors.join(' ');
             } else {

                 $scope.message = "Failed to login due to: server offline";
             }
         });
    };

}]);