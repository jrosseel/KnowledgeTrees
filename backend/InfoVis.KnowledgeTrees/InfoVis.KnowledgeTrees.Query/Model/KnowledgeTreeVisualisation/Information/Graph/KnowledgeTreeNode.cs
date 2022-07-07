namespace InfoVis.KnowledgeTrees.Query.Model.KnowledgeTreeVisualisation.Information.Graph
{
    using RossBill.Bricks.Cqrs.Query.Contracts;

    public class KnowledgeTreeNode : IInformation
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public int x { get; set; }
        public float y { get; set; }
        public float size { get; set; }
    
        public int laneId { get; set; }
        public int laneIndex { get; set; }

    }
}
