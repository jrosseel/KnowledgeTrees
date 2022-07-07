namespace InfoVis.KnowledgeTrees.Query.Model.KnowledgeTreeVisualisation.Information.Graph
{
    using RossBill.Bricks.Cqrs.Query.Contracts;
    using System.Collections.Generic;

    public class KnowledgeTreeGraph : IInformation
    {
        public List<KnowledgeTreeNode> nodes { get; set; }
        public List<KnowledgeTreeEdge> edges { get; set; }
    }
}