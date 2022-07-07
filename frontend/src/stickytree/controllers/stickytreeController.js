(function (app) {

    'use strict';

    var StickytreeController = (function () {

      // IMPORTANT: change this if viewport is too much different
        function StickytreeController($state, $scope, StickytreeDataService, StickytreeRenderService) {
            console.log('Creating TreeRawData Controller');
            // instantiate controller
            var vm = {
                isLoaded: false,

                subjects: [],
                selectedSubject: null,

                scopeToTopic: false
            };

            function _init() {
                vm.subjects = StickytreeDataService.getSubjects();

                // Render the first topic by default
                vm.isLoaded = true;
                vm.selectedSubject = vm.subjects[0];
                _renderKnowledgeTree();
            };

            function _renderKnowledgeTree() {
                if(vm.scopeToTopic)
                    StickytreeRenderService.renderTopicScoped(vm.selectedSubject, '#d3-renderframe');
                else
                    StickytreeRenderService.renderChapterScoped(vm.selectedSubject, '#d3-renderframe');
            };

            // Register controller on scope
            vm.renderKnowledgeTree = _renderKnowledgeTree;
            $scope.vm = vm;

            // Kick off init
            _init();
        };

        StickytreeController.$inject = ['$state', '$scope', 'StickytreeDataService', 'StickytreeRenderService'];
        return StickytreeController;
    }());

    app.controller('StickytreeController', StickytreeController);

}(angular.module('app.stickytree')));
