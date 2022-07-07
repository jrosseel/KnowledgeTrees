namespace InfoVis.KnowledgeTrees.Query.Model.KnowledgeTreeVisualisation.Information.Graph
{
    public class KnowledgeTreeEdge
    {

        public int source { get; set; }
        public int target { get; set; }
    
        // Internal so will not be serialized
        internal int classNmbr { get; set; }

        public string description { get; set; }

        public int targetId { get; internal set; }
        public int sourceId { get; internal set; }
    }
}
