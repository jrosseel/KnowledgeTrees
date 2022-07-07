namespace InfoVis.KnowledgeTrees.Query.Handlers.KnowledgeTreeVisualisation.TreeBuilder.Chapter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Data;
    using Shared;
    using Shared.Helpers;
    using Shared.Config;

    internal class ChapterTreeNodePickingEngine : AbstractTreeNodePickingEngine
    {
        private IEnumerable<Chapter> _chapters;
        private List<Chapter> _seenChapters;
        private IEnumerable<Chapter> _rootNodes;

        private IEnumerable<IGrouping<int, Chapter>> _chaptersPerLane;
        private int _nodesPerColumn;

        public ChapterTreeNodePickingEngine(KnowledgeTreeConfig config, int maxClass, IEnumerable<Chapter> chapters)
            : base(config, maxClass)
        {
            _chapters = chapters;
            // Order per class, then group per subjectField.
            _chaptersPerLane = chapters.OrderBy(c => c.Class.Number)
                                       .GroupBy(c => c.SubjectFieldId.Value);
            _nodesPerColumn = config.NodesPerColumn;

            _seenChapters = new List<Chapter>();
            _rootNodes = _FindRootNodes(chapters);
        }

        public override bool IsExhausted(int currClass)
        {
            return _seenChapters.Count() == _chapters.Count() || currClass > _maxClass;
        }

        public override TreeColumnStub PickInitial()
        {
            var treeColumnStub = new TreeColumnStub
            {
                ColumnNumber = 1,
                Fields = _InitFieldStubs()
            };

            // Try to get one of every field
            var limit = Math.Min(_nodesPerColumn,treeColumnStub.Fields.Count);
            for (var i = 0; i < limit; i++)
            {
                var lane = _chaptersPerLane.ElementAt(i);

                // We only want an item from form 1 as a root
                if (lane != null && lane.Any(it => it.Class.Number == 1 && _rootNodes.Any(r => r.Id == it.Id)))
                {
                    // We start with 1 chapter per field. We only pick root nodes in this first instance.
                    var chapter = lane.First(it => it.Class.Number == 1 && _rootNodes.Any(r => r.Id == it.Id));

                    // Add the field and its one node.
                    var field = treeColumnStub.Fields.Single(f => f.FieldId == chapter.SubjectFieldId);
                    field.Nodes.Add(new ChapterTreeNodeStub { Chapter = chapter,
                                                              Depth = 0 });

                    // Remove the node from the possible next pick by adding it to the seen list.
                    _seenChapters.Add(chapter);
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

            while(addCount  < nodesForCol)
            {
                // Scenario 1: We inspect the current field and try to find a node that is allowed in this field.
                if (fieldsCnt < treeColumnStub.Fields.Count)
                {
                    var nextNode = _PickNextNode(prev, fieldsCnt, currentClass);
                    // Scenario 1.1: Such a node exists. We add it to the current column.
                    if (nextNode != null)
                    {
                        // Add the node to the appropriate field
                        treeColumnStub.Fields.Single(f => f.FieldId == nextNode.Chapter.SubjectFieldId.Value)
                                             .Nodes
                                             .Add(nextNode);
                        _seenChapters.Add(nextNode.Chapter);
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
                        treeColumnStub.Fields.Single(f => f.FieldId == nextNode.Chapter.SubjectFieldId.Value)
                                             .Nodes
                                             .Add(nextNode);
                        _seenChapters.Add(nextNode.Chapter);
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

        

        private ChapterTreeNodeStub _PickNextNode(TreeColumnStub prev, int fieldsCnt, int currentClass)
        {
            // The collection and ordering of chaptersPerLane is equal to that of the TreeFieldStub collection. So safe ElementAt
            //       @see _InitFieldStubs
            var nodesAtField = _chaptersPerLane.ElementAt(fieldsCnt);

            // A node is available when..
            var availableNextNodes = nodesAtField.Where(c =>
                                        // It is of the current class
                                        c.Class.Number == currentClass
                                        // It has not yet been added to the system
                                        && !_seenChapters.Any(c2 => c2.Id == c.Id));
                                        // All its dependencies are already seen
                                       //&& _AllDependenciesSeen(c));
                                        
            if (availableNextNodes.Any())
            {
                return new ChapterTreeNodeStub
                {
                    Chapter = availableNextNodes.First()
                };
            }
            else
                return null;
        }

        private bool _AllDependenciesSeen(Chapter c)
        {
            foreach(var dep in c.Topics.SelectMany(t => t.BackwardDependencies))
            {
                if (!_seenChapters.Any(ch => ch.Id == dep.ReliesOnTopic.ChapterId))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Finds the root nodes. These are the nodes that have no backward dependencies.
        /// </summary>
        private IEnumerable<Chapter> _FindRootNodes(IEnumerable<Chapter> chapters)
        {
            return chapters.Where(c => c.Topics.Any(t => !t.BackwardDependencies.Any()));
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
            for(var i = 0; i < _chaptersPerLane.Count(); i++)
            {
                var lane = _chaptersPerLane.ElementAt(i);
                treeStubs.Add(new TreeFieldStub
                {
                    FieldId = lane.Key,
                    FieldName = lane.First().SubjectField.Name,
                    Nodes = new List<TreeNodeStub>()
                });
            }
            return treeStubs;
        }

        private bool _CurrentClassStillAvailable(int curr)
        {
            return _rootNodes.Any(ch => ch.Class.Number == curr 
                                            && ! _seenChapters.Any(c => c.Id == ch.Id));
        }

        private ChapterTreeNodeStub _FetchNewRoot(int curr)
        {
            return new ChapterTreeNodeStub
            {
                Chapter = _rootNodes.First(node => node.Class.Number == curr
                                                    && !_seenChapters.Any(c => c.Id == node.Id))
            };
        }
    }
}