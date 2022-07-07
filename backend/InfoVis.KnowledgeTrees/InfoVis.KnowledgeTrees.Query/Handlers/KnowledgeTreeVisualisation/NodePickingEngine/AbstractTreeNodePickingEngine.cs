using InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.Shared;
using InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.Shared.Config;
using System;

namespace InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.TreeBuilder
{
    internal abstract class AbstractTreeNodePickingEngine
    {
        protected KnowledgeTreeConfig _config;
        protected int _maxClass;

        public AbstractTreeNodePickingEngine(KnowledgeTreeConfig config, int maxClass)
        {
            _config = config;
            _maxClass = maxClass;
        }

        public abstract TreeColumnStub PickInitial();
        public abstract TreeColumnStub PickNext(TreeColumnStub prev, int currentClass);
        public abstract bool IsExhausted(int currClass);

        protected int _ChooseNodesForCol(int numberOfFields)
        {
            return new Random((int)DateTime.Now.Ticks).Next(_config.NodesPerColumn - 4, Math.Min(_config.NodesPerColumn, numberOfFields));
        }
    }
}
