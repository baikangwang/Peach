﻿var app = angular.module('YouHuApp', ['ngRoute', 'LocalStorageModule', 'ui.bootstrap', 'toaster', 'chieffancypants.loadingBar']);

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

    //$routeProvider.when("/driver/orders", {
    //    controller: "samplesController",
    //    templateUrl: "/app/views/samples.html"
    //});

    $routeProvider.when("/owner/orders", {
        controller: "ownerOrdersController",
        templateUrl: "/app/views/owner/orders.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);