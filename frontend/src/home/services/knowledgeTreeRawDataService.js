(function (app) {

    // Service that notifies the user through a bootstrap subtle popup
    var KnowledgeTreeRawDataService = (function () {

        function KnowledgeTreeRawDataService() {

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
        return KnowledgeTreeRawDataService;
    }());
    
    app.service('KnowledgeTreeRawDataService', KnowledgeTreeRawDataService);

}(angular.module('app.home')));