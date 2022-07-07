(function (app) {
    'use strict';

    // Service that notifies the user through a bootstrap subtle popup
    var StickytreeDataService = (function () {

        function StickytreeDataService() {

            // Gets subjects from folder data/tree
            function _getSubjects() {
                return [
                    'Mathematics',
                    'Physics',
                    'Chemistry',
                    'Biology',
                    'Geography',
                    'CRE'
                ];
            }

            return {
                getSubjects: _getSubjects
            };
        };
        return StickytreeDataService;
    }());

    app.service('StickytreeDataService', StickytreeDataService);

}(angular.module('app.stickytree')));
