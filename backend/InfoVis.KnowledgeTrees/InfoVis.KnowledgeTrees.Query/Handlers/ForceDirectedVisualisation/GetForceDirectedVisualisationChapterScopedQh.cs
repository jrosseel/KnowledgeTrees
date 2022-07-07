namespace InfoVis.KnowledgeTrees.Query.Handlers.ForceDirectedVisualisation
{
    using Model.ForceDirectedVisualisation.Information;
    using Model.ForceDirectedVisualisation.Query;
    using SqlQueryRepository.Contracts;
    using RossBill.Bricks.Cqrs.Query.Contracts;
    using RossBill.Bricks.Data;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public class GetForceDirectedVisualisationChapterScopedQh : IQueryHandler<GetForceDirectedGraphForSubjectQueryChapterScoped, ForceDirectedVisualisationInfo>
    {
        private IKnowledgeTreeQueryHolder _queryHolder;

        public GetForceDirectedVisualisationChapterScopedQh(IKnowledgeTreeQueryHolder qH)
        {
            _queryHolder = qH;
        }

        public async Task<ForceDirectedVisualisationInfo> Retrieve(GetForceDirectedGraphForSubjectQueryChapterScoped query)
        {
            var chapters = await _queryHolder.ChapterRepository
                                    .GetAll(t => t.SubjectInLevelId == query.SubjectId)
                                    .Include(t => t.Topics)
                                        .Include("Topics.BackwardDependencies.ReliesOnTopic")
                                    .Include(t => t.SubjectField)
                                    .ExecuteQueryAsync();


            var tempNodes = new List<TempRelevantTopicInfo>();
            // First prepare all nodes and keep their index
            for (var i = 0; i < chapters.Count(); i++)
            {
                var chapter = chapters.ElementAt(i);
                tempNodes.Add(new TempRelevantTopicInfo
                {
                    Index = i,
                    Id = chapter.Id,
                    Name = chapter.Name,
                    FieldId = chapter.SubjectFieldId ?? 0
                });
            }

            // Then, create all links
            var links = new List<ForceDirectedLink>();
            // Go over all topics
            foreach (var chapter in chapters)
            {
                var depList = new List<int>();
                // Then go over all backward dependencies for that topic. Note that the backward dependency ids will be the source!
                foreach (var topic in chapter.Topics)
                    foreach (var dependency in topic.BackwardDependencies)
                    {
                        if (tempNodes.Any(n => n.Id == dependency.ReliesOnTopic.ChapterId) && !depList.Any(n => n == dependency.ReliesOnTopic.ChapterId))
                        {
                            links.Add(new ForceDirectedLink
                            {
                                source = tempNodes.First(n => n.Id == dependency.ReliesOnTopic.ChapterId).Index,
                                target = tempNodes.First(n => n.Id == topic.ChapterId).Index,
                                // We work with a fixed scenario, where 
                                value = 1
                            });
                            depList.Add(dependency.ReliesOnTopic.ChapterId);
                        }
                }
            }

            return new ForceDirectedVisualisationInfo
            {
                nodes = tempNodes.Select(t => new ForceDirectedNode { name = t.Name, group = t.FieldId }),
                links = links
            };
        }

        private class TempRelevantTopicInfo
        {
            public int Id { get; set; }
            public int Index { get; set; }
            public string Name { get; set; }
            public int FieldId { get; set; }
        }
    }
}
