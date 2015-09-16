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
         function (error) {

             if (error) {
                 var msg= YouHuHelper.errorHelper.getErrorMsg(error);
                 $scope.message = "Failed to login due to:" + msg;
             } else {

                 $scope.message = "Failed to login due to: server offline";
             }
         });
    };

}]);