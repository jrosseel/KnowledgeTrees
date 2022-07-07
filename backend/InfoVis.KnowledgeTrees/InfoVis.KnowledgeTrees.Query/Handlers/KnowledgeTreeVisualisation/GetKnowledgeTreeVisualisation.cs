namespace InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using RossBill.Bricks.Cqrs.Query.Contracts;
    using RossBill.Bricks.Data;

    using SqlQueryRepository.Contracts;

    using Model.KnowledgeTreeVisualisation.Query;
    using Model.KnowledgeTreeVisualisation.Information;
    using Shared.Config;
    using TreeBuilder.Chapter;
    using TreeBuilder;
    using NodePickingEngine;
    using TreeBuilder.Topic;
    public class GetKnowledgeTreeVisualisation
        : IQueryHandler<GetKnowledgeTreeForSubjectQuery, KnowledgeTreeVisInformation>
    {
        private IKnowledgeTreeQueryHolder _queryHolder;

        public GetKnowledgeTreeVisualisation(IKnowledgeTreeQueryHolder queryHolder)
        {
            _queryHolder = queryHolder;
        }

        public async Task<KnowledgeTreeVisInformation> Retrieve(GetKnowledgeTreeForSubjectQuery query)
        {
            var config = new KnowledgeTreeConfig(query.nodesPerColumn, query.widthPerColumnInPixels, query.heightInPixels, null);

            var modelBuilder = query.scopeToTopic
                                        ? _CreateTopicScopedTreeModelBuilder(config)
                                        : _CreateChapterScopedTreeModelBuilder(config);

            var nodePickingEngine = query.scopeToTopic
                                        ? await _CreateTopicScopedTreeNodePickingEngine(query.subjectId, config, modelBuilder)
                                        : await _CreateChapterScopedTreeNodePickingEngine(query.subjectId, config, modelBuilder);

            

            //builder.SetSubject(query.subjectId, subjectName);

            // Add the initial root nodes
            modelBuilder.AddTreeColumn(nodePickingEngine.PickInitial());

            int currClass = 1;
            while (!nodePickingEngine.IsExhausted(currClass))
            {
                var prev = modelBuilder.GetLastTreeColumn();

                var curr = nodePickingEngine.PickNext(prev, currClass);

                // Catch check to avoid empty columns
                if (curr != null)
                {
                    modelBuilder.AddTreeColumn(curr);
                    prev.Next = curr;

                    if (curr.ExhaustedClass)
                        currClass++;
                }
                // Always exhausted in the null-case
                else
                    currClass++;
            }

            return modelBuilder.Build();
        }

        private AbstractKnowledgeTreeBuilder _CreateTopicScopedTreeModelBuilder(KnowledgeTreeConfig config)
        {
            return new TopicKnowledgeTreeBuilder(config);
        }

        private AbstractKnowledgeTreeBuilder _CreateChapterScopedTreeModelBuilder(KnowledgeTreeConfig config)
        {
            return new ChapterKnowledgeTreeBuilder(config);
        }
  
        private async Task<AbstractTreeNodePickingEngine> _CreateTopicScopedTreeNodePickingEngine(int subjectId, KnowledgeTreeConfig config, AbstractKnowledgeTreeBuilder builder)
        {
            var topicsDb = await _queryHolder.TopicRepository
                                    .GetAll(t => t.Chapter.SubjectInLevelId == subjectId)
                                    .Include(t => t.Chapter.SubjectInLevel.Subject)
                                    .Include(t => t.Chapter.SubjectField)
                                    .Include(t => t.Chapter.Class.Level)
                                    .ExecuteQueryAsync();

            var topics = topicsDb.ToList();
            builder.SetSubject(subjectId, topicsDb.First().Chapter.SubjectInLevel.Subject.Name);

            return new TopicTreeNodePickingEngine(config, topics.First().Chapter.Class.Level.Classes.Count, topics);
        }

        private async Task<AbstractTreeNodePickingEngine> _CreateChapterScopedTreeNodePickingEngine(int subjectId, KnowledgeTreeConfig config, AbstractKnowledgeTreeBuilder builder)
        {
            var chaptersDb = await _queryHolder.ChapterRepository
                                    .GetAll(t => t.SubjectInLevelId == subjectId)
                                    .Include(t => t.Topics)
                                        .Include("Topics.BackwardDependencies.ReliesOnTopic")
                                    .Include(c => c.SubjectInLevel.Subject)
                                    .Include(t => t.SubjectField)
                                    .Include(t => t.Class.Level)
                                    .ExecuteQueryAsync();

            var chapters = chaptersDb.ToList();
            // Set the subject (has to be done here because only access to db from this class #dirty
            builder.SetSubject(subjectId, chaptersDb.First().SubjectInLevel.Subject.Name);

            return new ChapterTreeNodePickingEngine(config, chapters.First().Class.Level.Classes.Count, chapters);

        }
    }
}