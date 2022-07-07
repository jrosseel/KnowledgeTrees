namespace InfoVis.KnowledgeTrees.Query.Model.KnowledgeTreeVisualisation.Information
{
    using Graph;
    using RossBill.Bricks.Cqrs.Query.Contracts;
    using SwimmingLanes;
    using System.Collections.Generic;

    public class KnowledgeTreeVisInformation : IInformation
    {
        public int subjectId { get; set; }
        public string subjectName { get; set; }

        public int heightInPixels { get; set; }
        public int widthInPixels { get; set; }
        public int widthPerColumnInPixels { get; set; }

        public IEnumerable<SwimmingLaneLegendEntry> swimmingLaneLegend { get; set; }
        public IEnumerable<KnowledgeTreeSwimmingLane> swimmingLanes { get; set; }
        public KnowledgeTreeGraph graph { get; set; }
    }
}