namespace InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.Shared
{
    using Helpers;
    using System.Collections.Generic;
    /// <summary>
    /// This class will be used to temporarely store and annotate all the columns, before transforming them into a view.
    /// </summary>
    internal class TreeColumnStub
    {
        public int ColumnNumber { get; set; }
        public List<TreeFieldStub> Fields { get; set; }

        public TreeColumnStub Previous { get; set; }
        public TreeColumnStub Next { get; set; }
        public bool ExhaustedClass { get; internal set; }
    }
 
}
