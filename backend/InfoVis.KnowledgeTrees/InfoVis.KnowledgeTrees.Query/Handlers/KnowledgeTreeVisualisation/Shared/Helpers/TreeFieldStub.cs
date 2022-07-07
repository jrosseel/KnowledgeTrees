using System;
namespace InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.Shared.Helpers
{
    using System.Collections.Generic;

    internal class TreeFieldStub
    {
        public int FieldId { get; set; }
        public string FieldName { get; internal set; }
        public List<TreeNodeStub> Nodes { get; set; }
    }
}
