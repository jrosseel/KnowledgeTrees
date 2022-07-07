namespace InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.TreeBuilder.Chapter
{
    using Model.KnowledgeTreeVisualisation.Information.Graph;
    using Shared.Config;
    using Shared.Helpers;
    using System;
    using System.Linq;

    internal class ChapterKnowledgeTreeBuilder : AbstractKnowledgeTreeBuilder
    {
        public ChapterKnowledgeTreeBuilder(KnowledgeTreeConfig config)
            : base(config)
        {}

        protected override KnowledgeTreeNode CreateGraphNode(TreeNodeStub node)
        {
            var typedNode = (ChapterTreeNodeStub)node;

            return new KnowledgeTreeNode
            {
                id = typedNode.Chapter.Id,
                name = typedNode.Chapter.Name,
                description = typedNode.Chapter.Description,
            };
        }

        protected override void CreateEdges(KnowledgeTreeGraph graph, TreeNodeStub addedNode)
        {
            var chapter = ( (ChapterTreeNodeStub)addedNode ).Chapter;

            var backwardDependencies = chapter.Topics.SelectMany(t => t.BackwardDependencies);

            foreach (var dependency in backwardDependencies)
            {
                if (graph.nodes.Any(n => n.id == dependency.ReliesOnTopic.ChapterId))
                {
                    var sourceIndex = FindNodeIndex(graph, dependency.ReliesOnTopic.ChapterId);
                    var targetIndex = FindNodeIndex(graph, chapter.Id);

                    if (// If both source and target are of this chapter
                        sourceIndex != null & targetIndex != null
                        // And it is not already added
                        && !graph.edges.Any(n => n.source == sourceIndex.Value
                                                && n.target == targetIndex.Value)
                        // And the two topics are not more than one class apart
                        && Math.Abs(dependency.DependantTopic.Chapter.Class.Number - dependency.ReliesOnTopic.Chapter.Class.Number) <= 1)
                    {
                        graph.edges.Add(new KnowledgeTreeEdge
                        {
                            source = sourceIndex.Value,
                            target = targetIndex.Value,
                            sourceId = dependency.ReliesOnTopic.ChapterId,
                            targetId = chapter.Id,
                            description = dependency.Description
                        });
                    }
                }
            }
        }
    }
}
