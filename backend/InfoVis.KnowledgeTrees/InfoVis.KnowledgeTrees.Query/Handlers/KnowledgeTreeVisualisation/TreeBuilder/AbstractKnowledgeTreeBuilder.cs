namespace InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.TreeBuilder
{
    using System.Collections.Generic;
    using System.Linq;

    using Model.KnowledgeTreeVisualisation.Information;
    using Model.KnowledgeTreeVisualisation.Information.Graph;
    using Model.KnowledgeTreeVisualisation.Information.SwimmingLanes;

    using Shared;
    using Shared.Helpers;
    using Shared.Config;
    /// <summary>
    /// Contains the logic for adding nodes to the knowledge tree visualisation.
    /// 
    /// The different types with which the visualisation is made (Topic, Chapter) can inherit from this code.
    /// </summary>
    internal abstract class AbstractKnowledgeTreeBuilder
    {
        private List<TreeColumnStub> _treeStub;
        private KnowledgeTreeVisInformation _toBuild;
        private KnowledgeTreeConfig _config;

        // 1. Construct the object
        public AbstractKnowledgeTreeBuilder(KnowledgeTreeConfig config)
        {
            _treeStub = new List<TreeColumnStub>();
            _toBuild = new KnowledgeTreeVisInformation();
            _config = config;
        }

        // 2. Set Subject
        public AbstractKnowledgeTreeBuilder SetSubject(int subjectId, string subjectName)
        {
            _toBuild.subjectId = subjectId;
            _toBuild.subjectName = subjectName;
            return this;
        }

        // 3. Create the tree stubs (comes from tree picking engine)
        public TreeColumnStub GetLastTreeColumn()
        {
            return _treeStub.Last();
        }

        public void AddTreeColumn(TreeColumnStub stub)
        {
            _treeStub.Add(stub);
        }

        // 4. Build the thing
        public KnowledgeTreeVisInformation Build()
        {
            _BuildGraph();
            _BuildChart();
            _toBuild.widthPerColumnInPixels = _config.WidthPerColumn;
            _toBuild.heightInPixels = _config.Height;
            _toBuild.widthInPixels = _treeStub.Count * _config.WidthPerColumn;

            return _toBuild;
        }

        #region Graph

        private void _BuildGraph()
        {
            var graph = new KnowledgeTreeGraph { edges = new List<KnowledgeTreeEdge>(), nodes = new List<KnowledgeTreeNode>() };

            var col = _treeStub.First();

            while (col != null)
            {
                var tempCol = new List<KnowledgeTreeNode>();
                var colCount = 0;
                var nodeCount = _CountNodes(col);

                // Add all the nodes of the column
                for (var j = 0; j < col.Fields.Count; j++)
                {
                    var field = col.Fields[j];

                    for (var i = 0; i < field.Nodes.Count; i++)
                    {
                        colCount++;

                        var node = field.Nodes.ElementAt(i);

                        var toAdd = CreateGraphNode(node);
                        _ConfigureCommonGraphNodeProperties(toAdd, field, j, col.ColumnNumber, colCount, nodeCount);
                        graph.nodes.Add(toAdd);

                        CreateEdges(graph, node);
                    }
                }

                // Iterate to the next col.
                col = col.Next;
            }

            _toBuild.graph = graph;
            _toBuild.graph.edges = _FindEdgesToKeep(graph.edges);
        }

        protected abstract KnowledgeTreeNode CreateGraphNode(TreeNodeStub node);
       

        private void _ConfigureCommonGraphNodeProperties(KnowledgeTreeNode node, TreeFieldStub field, int fieldIndex, int colNumber, int colCount, int nodeCount)
        {
            node.laneId = field.FieldId;
            node.laneIndex = fieldIndex;
            node.x = _CalculateX(colNumber);
            node.y = _CalculateY(_config.Height, colCount, nodeCount);

            // TODO: Match this to the KCSE results.
            node.size = 1;
        } 

        private List<KnowledgeTreeEdge> _FindEdgesToKeep(List<KnowledgeTreeEdge> edges)
        {
            var edgesToKeep = new List<KnowledgeTreeEdge>();
            foreach(var edge in edges)
            {
                if(_EdgeSuitable(edge, edgesToKeep))
                    edgesToKeep.Add(edge);
            }
            return edgesToKeep;
        }

        protected abstract void CreateEdges(KnowledgeTreeGraph graph, TreeNodeStub addedNode);

        //private void _CreateEdges(KnowledgeTreeGraph graph, Chapter chapter)
        //{
        //    var backwardDependencies = chapter.Topics.SelectMany(t => t.BackwardDependencies);

        //    foreach(var dependency in backwardDependencies)
        //    {
        //        if (graph.nodes.Any(n => n.id == dependency.ReliesOnTopic.ChapterId))
        //        {
        //            var sourceIndex = FindNodeIndex(graph, dependency.ReliesOnTopic.ChapterId);
        //            var targetIndex = FindNodeIndex(graph, chapter.Id);

        //            if(sourceIndex != null & targetIndex != null 
        //                && !graph.edges.Any(n => n.source == sourceIndex.Value
        //                                        && n.target == targetIndex.Value))
        //            {
        //                graph.edges.Add(new KnowledgeTreeEdge
        //                {
        //                    source = sourceIndex.Value,
        //                    target = targetIndex.Value,
        //                    sourceId = dependency.ReliesOnTopic.ChapterId,
        //                    targetId = chapter.Id,
        //                    description = dependency.Description
        //                });
        //            }
        //        }
        //    }
        //}
        
        private bool _EdgeSuitable(KnowledgeTreeEdge edge, List<KnowledgeTreeEdge> edges)
        {
            var parents = edges.Where(e => e.source != edge.source && e.target == edge.target);

            return edge.source != edge.target
                && parents.All(s => _EdgeSuitable(edge, s, edges));
        }

        private bool _EdgeSuitable(KnowledgeTreeEdge toInspect, KnowledgeTreeEdge curr, List<KnowledgeTreeEdge> edges)
        {
            if (curr.source != toInspect.source && curr.target == toInspect.target)
                return false;

            var parents = edges.Where(e => e.source != curr.source && e.target == curr.target);

            return parents.All(otherEdge => _EdgeSuitable(toInspect, otherEdge, edges));
        }

        // Do not allow overriding
        protected int? FindNodeIndex(KnowledgeTreeGraph graph, int chapterId)
        {
            for(var i = 0; i < graph.nodes.Count; i++)
            {
                if (graph.nodes.ElementAt(i).id == chapterId)
                    return i;
            }

            return null;
        }

        private int _CalculateX(int columnNumber)
        {
            return (columnNumber * _config.WidthPerColumn) - (_config.WidthPerColumn / 2);
        }

        private float _CalculateY(int height, int node, int count)
        {
            return height - (height * (((float) node / count) - ((float) 1 / (count * 2))));
        }

        #endregion

        #region Chart

        private void _BuildChart()
        {
            var col = _treeStub.First();

            _toBuild.swimmingLaneLegend = _CreateSwimmingLaneLegend(col);
            _toBuild.swimmingLanes = _CreateSwimmingLanes(col);
            
        }

        private IEnumerable<KnowledgeTreeSwimmingLane> _CreateSwimmingLanes(TreeColumnStub col)
        {
            var chart = new List<KnowledgeTreeSwimmingLane>();

            while (col != null)
            {
                var nodeCount = _CountNodes(col);

                for (var i = 0; i < col.Fields.Count; i++)
                {
                    var field = col.Fields.ElementAt(i);

                    chart.Add(new KnowledgeTreeSwimmingLane
                    {
                        x = col.ColumnNumber,
                        y = (float)field.Nodes.Count / nodeCount,
                        laneId = field.FieldId
                    });
                    chart.Add(new KnowledgeTreeSwimmingLane
                    {
                        x = col.ColumnNumber + _config.StartCorner,
                        y = (float)field.Nodes.Count / nodeCount,
                        laneId = field.FieldId
                    });
                }

                col = col.Next;
            }

            return chart;
        }

        private IEnumerable<SwimmingLaneLegendEntry> _CreateSwimmingLaneLegend(TreeColumnStub col)
        {
            var legend = new List<SwimmingLaneLegendEntry>();
            
            foreach(var field in col.Fields)
            {
                legend.Add(new SwimmingLaneLegendEntry
                {
                    id = field.FieldId,
                    name = field.FieldName
                });
            }

            //legend.Reverse();
            return legend;
        }

        private int _CountNodes(TreeColumnStub col)
        {
            var aggrCount = 0;
            foreach (var field in col.Fields)
                aggrCount += field.Nodes.Count;

            return aggrCount;
        }


        #endregion
    }
}
