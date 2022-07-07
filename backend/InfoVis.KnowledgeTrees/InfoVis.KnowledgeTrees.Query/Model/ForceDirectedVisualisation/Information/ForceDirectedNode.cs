namespace InfoVis.KnowledgeTrees.Query.Model.ForceDirectedVisualisation.Information
{
    using RossBill.Bricks.Cqrs.Query.Contracts;

    public class ForceDirectedNode : IInformation
    {
        public string name { get; set; }
        public int group { get; set; }
    }
}