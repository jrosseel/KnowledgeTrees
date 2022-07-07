using InfoVis.KnowledgeTrees.Data;

namespace InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.Shared.Helpers
{
    /// <summary>
    /// This class will be used to temporarely store and annotate all the nodes, before transforming them into a view.
    /// </summary>
    internal class TreeNodeStub
    {
        public int Depth { get; set; }
    }

    internal class ChapterTreeNodeStub : TreeNodeStub
    {
        public Chapter Chapter { get; set; }
    }

    internal class TopicTreeNodeStub : TreeNodeStub
    {
        public Topic Topic { get; set; }
    }
}
