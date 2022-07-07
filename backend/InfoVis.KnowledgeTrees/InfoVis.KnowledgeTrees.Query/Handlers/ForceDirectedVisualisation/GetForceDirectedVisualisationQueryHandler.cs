namespace InfoVis.KnowledgeTrees.Query.Handlers.ForceDirectedVisualisation
{
    using System.Threading.Tasks;
    using System.Linq;
    using System.Data.Entity;
    using System.Collections.Generic;

    using RossBill.Bricks.Data;
    using RossBill.Bricks.Cqrs.Query.Contracts;

    using SqlQueryRepository.Contracts;

    using Model.ForceDirectedVisualisation.Information;
    using Model.ForceDirectedVisualisation.Query;
    

    public class GetForceDirectedVisualisationQueryHandler
        : IQueryHandler<GetForceDirectedGraphForSubjectQueryTopicScopedQuery, ForceDirectedVisualisationInfo>
    {
        private IKnowledgeTreeQueryHolder _queryHolder;

        public GetForceDirectedVisualisationQueryHandler(IKnowledgeTreeQueryHolder qH)
        {
            _queryHolder = qH;
        }

        public async Task<ForceDirectedVisualisationInfo> Retrieve(GetForceDirectedGraphForSubjectQueryTopicScopedQuery query)
        {
            var topics = await _queryHolder.TopicRepository
                                    .GetAll(t => t.Chapter.SubjectInLevel.Id == query.SubjectId)
                                    .Include(t => t.Chapter)
                                        .Include(t => t.Chapter.SubjectField)
                                    .Include(t => t.BackwardDependencies)
                                    .ExecuteQueryAsync();


            var tempNodes = new List<TempRelevantTopicInfo>();
            // First prepare all nodes and keep their index
            for(var i = 0; i < topics.Count(); i++)
            {
                var topic = topics.ElementAt(i);
                tempNodes.Add(new TempRelevantTopicInfo
                {
                    Index = i,
                    Id = topic.Id,
                    Name = topic.Name,
                    FieldId = topic.Chapter.SubjectFieldId ?? 0
                });
            }

            // Then, create all links
            var links = new List<ForceDirectedLink>();
            // Go over all topics
            foreach(var topic in topics)
            {
                // Then go over all backward dependencies for that topic. Note that the backward dependency ids will be the source!
                foreach(var dependency in topic.BackwardDependencies)
                {
                    if(tempNodes.Any(n => n.Id == dependency.ReliesOnId))
                        links.Add(new ForceDirectedLink{
                            source = tempNodes.First(n => n.Id == dependency.ReliesOnId).Index,
                            target = tempNodes.First(n => n.Id == dependency.DependantId).Index,
                            // We work with a fixed scenario, where 
                            value = 1
                        });
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
