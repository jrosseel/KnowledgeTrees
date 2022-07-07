(function (app) {

    var KnowledgeTreeRawRenderService = (function () {

        function KnowledgeTreeRawRenderService(ktRawRenderWorker) {

            // IMPORTANT: change this if viewport is too much different
            var _dimensions = { x:950, y:900 };

            // Pull out configuration from the construct algorithm
            var _chapterRenderSettings = {
                gravity: 0.05,
                distance: 60,
                charge: -500,
                showLabels: true,
                dimensions: _dimensions
            };
            var _topicRenderSettings = {
                gravity: 0.05,
                distance: 60,
                charge: -50,
                showLabels: false, 
                dimensions: _dimensions
            };

            function _renderChapterScoped(subject, anchor) {
                var fileLoc = _createDataUrl(subject, false);
                
                ktRawRenderWorker.render(fileLoc, anchor, _chapterRenderSettings);
            };

            function _renderTopicScoped(subject, anchor) {
                var fileLoc = _createDataUrl(subject, true);

                ktRawRenderWorker.render(fileLoc, anchor, _topicRenderSettings);
            };


            function _createDataUrl(subject, scaleToTopic) {
                return '/data/' + _getScaleSuffix(scaleToTopic) + '/' + subject.toLowerCase() + '.js';
            };

            function _getScaleSuffix(scaleToTopic) {
                return scaleToTopic 
                            ? 'topic'
                            : 'chapter';
            };

            return {
                renderChapterScoped: _renderChapterScoped,
                renderTopicScoped: _renderTopicScoped
            };
        };

        KnowledgeTreeRawRenderService.$inject = ['KnowledgeTreeRawRenderWorker'];
        
        return KnowledgeTreeRawRenderService;
    }());

    app.service('KnowledgeTreeRawRenderService', KnowledgeTreeRawRenderService);

}(angular.module('app.home')));