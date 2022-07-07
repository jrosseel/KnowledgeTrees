(function () {
    'use strict';

    var app = angular.module('app.common', ['ui.router']);

    // Define the LOCAL VERSION NUMBER.
    var LOCAL_VERSION_NUMBER = 0.1;

    // Set default states of named views (for instance the default header)
    //@see http://stackoverflow.com/questions/27734497/set-default-view-for-multiple-states-angularjs-ui-router
    app.config(['$stateProvider', function ($stateProvider) {
        $stateProvider.state('root', {
            abstract: true,
            views: {
                'root': {
                    templateUrl: 'src/common/views/layout.html?v=' + LOCAL_VERSION_NUMBER
                },
                'header@root': {
                    templateUrl: 'src/common/views/header.html?v=' + LOCAL_VERSION_NUMBER
                }
            }
        })
        .state('error-404', {
            parent: 'root',
            url: '/error-404',
            views: {
                'page': {
                    templateUrl: 'src/common/views/404.html?v=' + LOCAL_VERSION_NUMBER
                }
            }
        });
    }]);


    // We define the constant here since everything else uses app.common
    // Config parameters that app itself is dependant on have to be defined here.
    app.constant('LOCAL_VERSION_NUMBER', LOCAL_VERSION_NUMBER);
}());