(function (app) {

    'use strict';

    var PageHeaderController = (function () {

        function PageHeaderController($state, $scope, $location) {
            var vm = {};

            function _init() {}

            function _routeIs(routeName) {
                return $location.path() === routeName;
            };

            $scope.vm = vm;

            _init();
        }

        PageHeaderController.$inject = ['$state', '$scope', '$location'];

        return PageHeaderController;
    }());


    app.controller('PageHeaderController', PageHeaderController);

}(angular.module('app.common')));