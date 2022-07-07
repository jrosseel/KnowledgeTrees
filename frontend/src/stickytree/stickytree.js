(function () {
    'use strict';

    var app = angular.module('app.stickytree', ['ui.router', 'ui.bootstrap', 'app.common']);

    app.config(['$stateProvider', 'LOCAL_VERSION_NUMBER', function ($stateProvider, LOCAL_VERSION_NUMBER) {

        // Home configuration
        $stateProvider
            .state('stickytree', {
                parent: 'root',
                url: '/stickytree',
                views: {
                    page: { templateUrl: 'src/stickytree/views/stickytree.html?v=' + LOCAL_VERSION_NUMBER }
                }
            })
    }]);
}());
