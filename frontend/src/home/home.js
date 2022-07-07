(function () {
    'use strict';

    var app = angular.module('app.home', ['ui.router', 'ui.bootstrap', 'app.common']);

    app.config(['$stateProvider', 'LOCAL_VERSION_NUMBER', function ($stateProvider, LOCAL_VERSION_NUMBER) {

        // Home configuration
        $stateProvider
            .state('home', {
                parent: 'root',
                url: '/home',
                views: {
                    page: { templateUrl: 'src/home/views/home.html?v=' + LOCAL_VERSION_NUMBER }
                }
            })
    }]);
}());