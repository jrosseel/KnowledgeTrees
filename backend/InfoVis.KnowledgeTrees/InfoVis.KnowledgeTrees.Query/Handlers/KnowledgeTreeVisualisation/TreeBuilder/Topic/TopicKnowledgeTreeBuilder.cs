namespace InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.TreeBuilder.Topic
{
    using Model.KnowledgeTreeVisualisation.Information.Graph;
    using Shared.Config;
    using Shared.Helpers;
    using System;
    using System.Linq;

    internal class TopicKnowledgeTreeBuilder : AbstractKnowledgeTreeBuilder
    {
        public TopicKnowledgeTreeBuilder(KnowledgeTreeConfig config)
            : base(config)
        { }

        protected override KnowledgeTreeNode CreateGraphNode(TreeNodeStub node)
        {
            var typedNode = (TopicTreeNodeStub)node;

            return new KnowledgeTreeNode
            {
                id = typedNode.Topic.Id,
                name = typedNode.Topic.Name,
                description = typedNode.Topic.Description,
            };
        }

        protected override void CreateEdges(KnowledgeTreeGraph graph, TreeNodeStub addedNode)
        {
            var topic = ((TopicTreeNodeStub)addedNode).Topic;

            var backwardDependencies = topic.BackwardDependencies;

            foreach (var dependency in backwardDependencies)
            {
                if (graph.nodes.Any(n => n.id == dependency.ReliesOnId))
                {
                    var sourceIndex = FindNodeIndex(graph, dependency.ReliesOnId);
                    var targetIndex = FindNodeIndex(graph, topic.Id);

                    if (sourceIndex != null & targetIndex != null
                        && !graph.edges.Any(n => n.source == sourceIndex.Value
                                                && n.target == targetIndex.Value)
                        && Math.Abs(dependency.DependantTopic.Chapter.Class.Number - dependency.ReliesOnTopic.Chapter.Class.Number) <= 1)
                    {
                        graph.edges.Add(new KnowledgeTreeEdge
                        {
                            source = sourceIndex.Value,
                            target = targetIndex.Value,
                            sourceId = dependency.ReliesOnId,
                            targetId = topic.Id,
                            description = dependency.Description
                        });
                    }
                }
            }
        }
    }
}
