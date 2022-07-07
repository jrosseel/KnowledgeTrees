(function (window) {
    'use strict';

    // Guard against internet explorer console error
    if (!console || !console.log) console.log = function (message) { };

    // Add your modules to the application
    var app = angular.module('app', [
        'app.common',
        'app.home',
        'app.stickytree',

        'ui.router',
        'ngAnimate',
        'ui.bootstrap'
    ]);

    app.constant('SERVICEURL', 'http://localhost:50718');

    /**
     * Do app configuration over here
     * set up default routes
     * add constants if needed
     */
    app.config(['$urlRouterProvider', function ($urlRouterProvider) {
        $urlRouterProvider.when('', '/home')
                          .when('/', '/home')
                          .otherwise('/error-404');
    }]);

    app.run(['$rootScope', function ($rootScope) { }]);

}(window));
