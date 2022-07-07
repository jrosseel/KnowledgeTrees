(function (app) {

    'use strict';

    var KnowledgeTreeRawDataController = (function () {

        function KnowledgeTreeRawDataController($state, $scope, ktRawDataService, ktRawRenderService) {
            console.log('Creating KnowledgeTreeRawData Controller');
            // instantiate controller
            var vm = {
                isLoaded: false,

                subjects: [],
                selectedSubject: null,

                scopeToTopic: false
            };           

            function _init() {
                vm.subjects = ktRawDataService.getSubjects();
            };

            function _renderKnowledgeTree() {
                if(vm.scopeToTopic) 
                    ktRawRenderService.renderTopicScoped(vm.selectedSubject, '#d3-renderframe');
                else
                    ktRawRenderService.renderChapterScoped(vm.selectedSubject, '#d3-renderframe');
            };


            // Register controller on scope
            vm.renderKnowledgeTree = _renderKnowledgeTree;
            $scope.vm = vm;

            // Kick off init
            _init();
        };

        KnowledgeTreeRawDataController.$inject = ['$state', '$scope', 'KnowledgeTreeRawDataService', 'KnowledgeTreeRawRenderService'];
        return KnowledgeTreeRawDataController;
    }());

    app.controller('KnowledgeTreeRawDataController', KnowledgeTreeRawDataController);

}(angular.module('app.home')));