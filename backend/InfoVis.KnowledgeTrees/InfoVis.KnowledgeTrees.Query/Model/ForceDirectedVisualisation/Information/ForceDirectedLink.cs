namespace InfoVis.KnowledgeTrees.Query.Model.ForceDirectedVisualisation.Information
{
    using RossBill.Bricks.Cqrs.Query.Contracts;

    public class ForceDirectedLink : IInformation
    {
        public int source { get; set; }
        public int target { get; set; }
        public int value { get; set; }
    }
}