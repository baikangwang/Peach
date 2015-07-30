var app = angular.module('YouHuApp', ['ngRoute', 'LocalStorageModule', 'ui.bootstrap', 'toaster', 'chieffancypants.loadingBar']);

app.config(function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/app/views/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "/app/views/signup.html"
    });

    $routeProvider.when("/driver/orders", {
        controller: "driverOrdersController",
        templateUrl: "/app/views/driver/orders.html"
    });

    $routeProvider.when("/driver/freightUnits", {
        controller: "freightUnitsController",
        templateUrl: "/app/views/driver/freights.html"
    });

    $routeProvider.when("/owner/orders", {
        controller: "ownerOrdersController",
        templateUrl: "/app/views/owner/orders.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService','$location', function (authService,$location) {
    authService.fillAuthData();

        var oauth = authService.authentication;
        if (oauth) {
            if (oauth.isAuth) {
                $location.path("/" + oauth.role.toLowerCase() + "/orders");
            }
        }
}]);