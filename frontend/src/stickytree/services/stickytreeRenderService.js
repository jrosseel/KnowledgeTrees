// @src: http://bl.ocks.org/mbostock/3750558 (Sticky force layout - d3)
(function (app) {

    'use strict';

    var StickytreeRenderService = (function () {

        function StickytreeRenderService(SwimmingLanesRenderWorker, GraphRenderWorker) {

            // Pull out configuration from the construct algorithm
            var _chapterRenderSettings = {
                gravity: 0.5,
                distance: 60,
                charge: -500,
                showLabels: true
            };
            var _topicRenderSettings = {
                gravity: 0.05,
                distance: 60,
                charge: -50,
                showLabels: false
            };

            function _renderChapterScoped(subject, anchor) {
                var fileLoc = _createDataUrl(subject, false);

                // Render the swimming lanes
                SwimmingLanesRenderWorker.render(fileLoc, anchor, _chapterRenderSettings);
                // Then render the graph on top
                GraphRenderWorker.render(fileLoc, anchor, _chapterRenderSettings);
            };

            function _renderTopicScoped(subject, anchor) {
                var fileLoc = _createDataUrl(subject, true);

                // Render the swimming lanes
                SwimmingLanesRenderWorker.render(fileLoc, anchor, _chapterRenderSettings);
                // Then render the graph on top
                GraphRenderWorker.render(fileLoc, anchor, _chapterRenderSettings);
            };


            // Generate URL to JS data file
            function _createDataUrl(subject, scopeToTopic) {
                return 'data/tree/' + _getScaleSuffix(scopeToTopic) + '/' + subject.toLowerCase() + '.js';
            };

            function _getScaleSuffix(scopeToTopic) {
                return scopeToTopic
                            ? 'topic'
                            : 'chapter';
            };

            return {
                renderChapterScoped: _renderChapterScoped,
                renderTopicScoped: _renderTopicScoped
            };
        };

        StickytreeRenderService.$inject = ['SwimmingLanesRenderWorker', 'GraphRenderWorker'];

        return StickytreeRenderService;
    }());

    app.service('StickytreeRenderService', StickytreeRenderService);

}(angular.module('app.stickytree')));
