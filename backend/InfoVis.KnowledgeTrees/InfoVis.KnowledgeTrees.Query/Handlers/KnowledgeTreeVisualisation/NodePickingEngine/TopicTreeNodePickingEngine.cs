using InfoVis.KnowledgeTrees.Data;
using InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.Shared;
using InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.Shared.Config;
using InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.Shared.Helpers;
using InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.TreeBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.NodePickingEngine
{
    internal class TopicTreeNodePickingEngine : AbstractTreeNodePickingEngine
    {
        private IEnumerable<Topic> _topics;
        private List<Topic> _seenTopics;
        private IEnumerable<Topic> _rootNodes;

        private IEnumerable<IGrouping<int, Topic>> _topicsPerLane;
        private int _nodesPerColumn;

        public TopicTreeNodePickingEngine(KnowledgeTreeConfig config, int maxClass, IEnumerable<Topic> topics)
            : base(config, maxClass)
        {
            _topics = topics;
            // Order per class, then group per subjectField.
            _topicsPerLane = topics.OrderBy(c => c.Chapter.Class.Number)
                                    .GroupBy(c => c.Chapter.SubjectFieldId.Value);
            _nodesPerColumn = config.NodesPerColumn;

            _seenTopics = new List<Topic>();
            _rootNodes = _FindRootNodes(topics);
        }

        public override bool IsExhausted(int currClass)
        {
            return _seenTopics.Count() == _topics.Count() || currClass > _maxClass;
        }

        public override TreeColumnStub PickInitial()
        {
            var treeColumnStub = new TreeColumnStub
            {
                ColumnNumber = 1,
                Fields = _InitFieldStubs()
            };

            // Try to get one of every field
            var limit = Math.Min(_nodesPerColumn, treeColumnStub.Fields.Count);
            for (var i = 0; i < limit; i++)
            {
                var lane = _topicsPerLane.ElementAt(i);

                // We only want an item from form 1 as a root
                if (lane != null && lane.Any(it => it.Chapter.Class.Number == 1 && _rootNodes.Any(r => r.Id == it.Id)))
                {
                    // We start with 1 chapter per field. We only pick root nodes in this first instance.
                    var topic = lane.First(it => it.Chapter.Class.Number == 1 && _rootNodes.Any(r => r.Id == it.Id));

                    // Add the field and its one node.
                    var field = treeColumnStub.Fields.Single(f => f.FieldId == topic.Chapter.SubjectFieldId);
                    field.Nodes.Add(new TopicTreeNodeStub
                    {
                        Topic = topic,
                        Depth = 0
                    });

                    // Remove the node from the possible next pick by adding it to the seen list.
                    _seenTopics.Add(topic);
                }
            }

            return treeColumnStub;
        }

        public override TreeColumnStub PickNext(TreeColumnStub prev, int currentClass)
        {
            var treeColumnStub = new TreeColumnStub
            {
                Previous = prev,
                ColumnNumber = prev.ColumnNumber + 1,
                Fields = _InitFieldStubs()
            };

            // Always try to start from the previous nodes.
            // Also exhaust every chapter of a current class before moving on to the next class.
            int addCount = 0;
            int fieldsCnt = 0;
            bool seenOneInField = false;

            var nodesForCol = _ChooseNodesForCol(treeColumnStub.Fields.Count);

            while (addCount < nodesForCol)
            {
                // Scenario 1: We inspect the current field and try to find a node that is allowed in this field.
                if (fieldsCnt < treeColumnStub.Fields.Count)
                {
                    var nextNode = _PickNextNode(prev, fieldsCnt, currentClass);
                    // Scenario 1.1: Such a node exists. We add it to the current column.
                    if (nextNode != null)
                    {
                        // Add the node to the appropriate field
                        treeColumnStub.Fields.Single(f => f.FieldId == nextNode.Topic.Chapter.SubjectFieldId.Value)
                                             .Nodes
                                             .Add(nextNode);
                        _seenTopics.Add(nextNode.Topic);
                        addCount++;

                        // Allow a maximum of two nodes of the same field per turn. If one is already seen, increase the field count
                        if (seenOneInField)
                        {
                            seenOneInField = false;
                            fieldsCnt++;
                        }
                        else seenOneInField = true;
                    }
                    // Scenario 1.2: No such node exists. We try to find one in the next subject field.
                    else
                        fieldsCnt++;
                }
                // Scenario 2: We've exhausted all the fields and found not enough nodes
                else
                {
                    // Scenario 2.1: But there exists a new root of the current class (Form 1, ...)
                    if (_CurrentClassStillAvailable(currentClass))
                    {
                        // We insert the root into the field and continue
                        var nextNode = _FetchNewRoot(currentClass);
                        treeColumnStub.Fields.Single(f => f.FieldId == nextNode.Topic.Chapter.SubjectFieldId.Value)
                                             .Nodes
                                             .Add(nextNode);
                        _seenTopics.Add(nextNode.Topic);
                        addCount++;
                    }
                    // Scenario 2.2: We exhausted the whole class.
                    else
                    {
                        // We notify the next iteration that class is exhausted. Then break the loop and continue
                        treeColumnStub.ExhaustedClass = true;
                        break;
                    }
                }
            }

            // Check to avoid empty columns
            return addCount == 0 ? null
                                 : treeColumnStub;
        }



        private TopicTreeNodeStub _PickNextNode(TreeColumnStub prev, int fieldsCnt, int currentClass)
        {
            // The collection and ordering of chaptersPerLane is equal to that of the TreeFieldStub collection. So safe ElementAt
            //       @see _InitFieldStubs
            var nodesAtField = _topicsPerLane.ElementAt(fieldsCnt);

            // A node is available when..
            var availableNextNodes = nodesAtField.Where(t =>
                                        // It is of the current class
                                        t.Chapter.Class.Number == currentClass
                                        // It has not yet been added to the system
                                        && !_seenTopics.Any(t2 => t2.Id == t.Id));
            // All its dependencies are already seen
            //&& _AllDependenciesSeen(c));

            if (availableNextNodes.Any())
            {
                return new TopicTreeNodeStub
                {
                    Topic = availableNextNodes.First()
                };
            }
            else
                return null;
        }

        private bool _AllDependenciesSeen(Topic t)
        {
            foreach (var dep in t.BackwardDependencies)
            {
                if (!_seenTopics.Any(ch => ch.Id == dep.ReliesOnId))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Finds the root nodes. These are the nodes that have no backward dependencies.
        /// </summary>
        private IEnumerable<Topic> _FindRootNodes(IEnumerable<Topic> topics)
        {
            return topics.Where(t => !t.BackwardDependencies.Any());
        }

        private TreeColumnStub _InitTreeColumnStub(TreeColumnStub prev)
        {
            return new TreeColumnStub
            {
                Previous = prev,
                ColumnNumber = prev.ColumnNumber + 1,

                Fields = _InitFieldStubs()
            };
        }

        // We always need to contain all the swimming lanes
        private List<TreeFieldStub> _InitFieldStubs()
        {
            var treeStubs = new List<TreeFieldStub>();
            for (var i = 0; i < _topicsPerLane.Count(); i++)
            {
                var lane = _topicsPerLane.ElementAt(i);
                treeStubs.Add(new TreeFieldStub
                {
                    FieldId = lane.Key,
                    FieldName = lane.First().Chapter.SubjectField.Name,
                    Nodes = new List<TreeNodeStub>()
                });
            }
            return treeStubs;
        }

        private bool _CurrentClassStillAvailable(int curr)
        {
            return _rootNodes.Any(top => top.Chapter.Class.Number == curr
                                            && !_seenTopics.Any(t => t.Id == top.Id));
        }

        private TopicTreeNodeStub _FetchNewRoot(int curr)
        {
            return new TopicTreeNodeStub
            {
                Topic = _rootNodes.First(node => node.Chapter.Class.Number == curr
                                                    && !_seenTopics.Any(c => c.Id == node.Id))
            };
        }
    }
}