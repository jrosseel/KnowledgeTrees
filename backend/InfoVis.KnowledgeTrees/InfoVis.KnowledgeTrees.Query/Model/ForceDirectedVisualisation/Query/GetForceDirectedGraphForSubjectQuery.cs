using InfoVis.KnowledgeTrees.Query.Model.ForceDirectedVisualisation.Query;

namespace InfoVis.KnowledgeTrees.Query.Model.ForceDirectedVisualisation.Query
{
    using RossBill.Bricks.Cqrs.Query.Contracts;

    public class GetForceDirectedGraphForSubjectQuery : IQuery
    {
        public int SubjectId { get; set; }
        public bool ScopeToTopic { get; set; }


        public GetForceDirectedGraphForSubjectQueryChapterScoped AsChapterScoped()
        {
            return new GetForceDirectedGraphForSubjectQueryChapterScoped
            {
                SubjectId = this.SubjectId,
                ScopeToTopic = false
            };
        }

        public GetForceDirectedGraphForSubjectQueryTopicScopedQuery AsTopicScoped()
        {
            return new GetForceDirectedGraphForSubjectQueryTopicScopedQuery
            {
                SubjectId = this.SubjectId,
                ScopeToTopic = true

            };
        }
    }

}
