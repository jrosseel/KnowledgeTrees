namespace InfoVis.KnowledgeTrees.Query.Model.KnowledgeTreeVisualisation.Query
{
    using RossBill.Bricks.Cqrs.Query.Contracts;

    public class GetKnowledgeTreeForSubjectQuery : IQuery
    {
        

        /// <summary>Subject Id of the to-render subject</summary>
        public int subjectId { get; set; }

        /// <summary>True if the K-Tree should include all topics. False or null if scoped to chapter.</summary>
        public bool scopeToTopic { get; set; }


        public int heightInPixels;
        /// <summary>Number of nodes for each column. Determines vertical distance between the nodes.</summary>
        public int? nodesPerColumn { get; set; }
        /// <summary>Number of pixels for each column. Determines horizontal distance between the nodes.</summary>
        public int? widthPerColumnInPixels { get; set; }
    }
}
