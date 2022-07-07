namespace InfoVis.KnowledgeTrees.Query.Model.KnowledgeTreeVisualisation.Information.SwimmingLanes
{
    using RossBill.Bricks.Cqrs.Query.Contracts;

    public class KnowledgeTreeSwimmingLane : IInformation
    {
        public float x { get; set; }
        public float y { get; set; }
        public int laneId { get; set; }
    }
}
