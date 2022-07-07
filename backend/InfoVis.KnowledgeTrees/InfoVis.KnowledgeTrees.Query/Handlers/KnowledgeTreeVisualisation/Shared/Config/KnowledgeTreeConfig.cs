namespace InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.Shared.Config
{
    internal class KnowledgeTreeConfig
    {
        private const int DEFAULT_NODES_PER_COLUMN = 6;
        private const int DEFUALT_WIDTH_PER_COLUMN = 50;
        // Percentage of the swimming lanes at which the corner should start.
        private const float DEFAULT_START_CORNER = 0.8f;

        public int NodesPerColumn { get; set; }
        public int WidthPerColumn { get; set; }
        public int Height { get; set; }

        public float StartCorner { get; set; }

        public KnowledgeTreeConfig(int? nodesPerColumn, int? widthPerColumn, int height, float? startCorner)
        {
            NodesPerColumn = nodesPerColumn ?? DEFAULT_NODES_PER_COLUMN;
            WidthPerColumn = widthPerColumn ?? DEFUALT_WIDTH_PER_COLUMN;
            Height = height;
            StartCorner = startCorner ?? DEFAULT_START_CORNER;
        }
    }
}
