namespace InfoVis.KnowledgeTrees.Query.Model.ForceDirectedVisualisation.Information
{
    using RossBill.Bricks.Cqrs.Query.Contracts;

    using System.Collections.Generic;

    public class ForceDirectedVisualisationInfo : IInformation
    {
        public IEnumerable<ForceDirectedNode> nodes { get; set; }
        public IEnumerable<ForceDirectedLink> links { get; set; }
    }
}
