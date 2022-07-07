// @src: http://bl.ocks.org/mbostock/3750558 (Sticky force layout - d3)
(function(app) {

	'use strict';

	var GraphRenderWorker = (function() {

		function GraphRenderWorker() {

			var 	LINK_OPACITY_HOVER = 0.7,
			 		LINK_OPACITY_NORMAL = 0.3,
					LINK_OPACITY_CONTEXT = 0.10,
					STROKE_COLOR_NORMAL = "#000",
					STROKE_COLOR_HOVER = "black",
					NODE_SIZE_HOVER = 60,
					NODE_SIZE_NORMAL = 35,
					NODE_OPACITY_NORMAL = 0.55,
					NODE_OPACITY_HOVER = 1,
					FONT_SIZE_NORMAL = "10px",
					FONT_SIZE_HOVER = "15px",
					TEXT_PADDING_SIZE = 10,
					LINK_WIDTH_NORMAL = 2,
					LINK_WIDTH_HOVER = 5,
					LINK_WIDTH_NODE_HOVER = 3;

			function _render(dataLoc, anchor, config) {

				var _force, _drag, _nodes, _links;

				function _initRenderGraph() {
					d3.json(dataLoc, _renderGraph);
				};

				// -- Core system start

				function _renderGraph(error, data) {
					if (error)
					 	throw error;

					// Generate the physics force on the graph
					_force = d3.layout.force()
											.size([data.widthInPixels, data.heightInPixels])
											.gravity(config.gravity)
											.charge(config.charge)
											.linkDistance(config.distance)
											.on("tick", _tick);
					_drag = _force.drag();
					_drag.on("dragstart", _dragStart);

					var graph = data.graph;

					// The SVG output
					var svg = d3.select(anchor).append("svg")
									.attr("id", "graph")
									.attr("width", data.widthInPixels)
									.attr("height", data.heightInPixels);

					_annotateData(graph);
					// Kickoff graph
					_force.nodes(graph.nodes)
						  .links(graph.edges)
						  .start();

					_addArrows(svg);
					// Draw the edges and add the appropriate information
					_links = _drawEdges(svg, graph.edges);
					_nodes = _drawNodes(svg, graph.nodes);
					_drawCircles(_nodes);
					_drawText(_nodes);
					_prepareHoverText(_nodes);
					_prepareNeighbors(_links);


					// Link hover effects
					_links.on("mouseover", _onMouseOverEdge);
					_links.on("mouseout", _onMouseOutEdge);

					// Node hover effects
					_nodes.on("mouseover", _onMouseOverNode);
					_nodes.on("mouseout", _onNodeMouseOut);
				};

				// -- Core system end

				// .on drag function.
				function _dragStart(d) {
					d3.select(this).classed("fixed", d.fixed = true);
				};

				function _tick() {
					_links.attr("x1", function(d) {
							return d.source.x;
						})
						.attr("y1", function(d) {
							return d.source.y;
						})
						.attr("x2", function(d) {
							return d.target.x;
						})
						.attr("y2", function(d) {
							return d.target.y;
						});

					_nodes.attr("transform", function(d) {
						return "translate(" + d.x + "," + d.y + ")";
					});
				};

				// Make all the nodes fixed, so they are on the correct X and Y position
				// Instead of floating randomly
				function _annotateData(data) {
					data.nodes.forEach(function(d) {
						d.fixed = true;
					});
				};

				// Adds arrows to the edges
				function _addArrows(svg) {
					svg.append("defs").selectAll("marker")
						.data(["suit", "licensing", "resolved"])
						.enter().append("marker")
						.attr("id", function(d) {
							return d;
						})
						.attr("viewBox", "0 -5 10 10")
						.attr("refX", 75)
						.attr("refY", 0)
						.attr("markerWidth", 6)
						.attr("markerHeight", 6)
						.attr("orient", "auto")
						.append("path")
						.attr("d", "M0,-5L10,0L0,5 L10,0 L0, -5")
						.style("stroke", "#000");
				};

				// Shows the hovered item info on the left panel
				function _showItemDescription(item) {
					$("#nodeInfo").html("<b>" + item.name + "</b><br>" + item.description);
				};

				// Hides the hovered item info
				function _hideItemDescription() {
					$("#nodeInfo").html("<i>Hover over a chapter or link to see more info.</i>");
				};

				// Draw the links between nodes
				function _drawEdges(svg, edges) {
					return svg.selectAll(".link")
								.data(edges)
								.enter().append("line")
								.attr("opacity", LINK_OPACITY_NORMAL)
								.attr("class", "link chapterlink")
								.attr("targetId", function(edge){return edge.targetId})
								.attr("sourceId", function(edge){return edge.sourceId})
								.style("marker-end", "url(#suit)");

				};

				// Variable that knows if two nodes are neighbors.
				var _neighbors = {};

				// Fill the variable
				function _prepareNeighbors(links) {
					// Fill the linkedByIndex table.
					d3.selectAll('line')
				    	.each(function(edge) {
							if (edge != undefined
									&& edge.source != undefined
									&& edge.source.index != undefined)
								_neighbors[edge.source.index + "," + edge.target.index] = 1;
				    	});
				}

				// Uses the neighbors variable to check if two nodes are neighbors.
				function _neighboring(a, b) {
					return _neighbors[a.index + "," + b.index]
				  	|| _neighbors[b.index + "," + a.index];
				};

				function _drawNodes(svg, nodeData) {
					return svg.selectAll(".node")
								.data(nodeData)
								.enter()
								.append("g")
								.attr("class", "chapternode")
								.attr("nodeId", function(d){
									return "node"+d.id;
								})
								.attr("laneId", function(d){
									return "lane"+d.laneId;
								});
				};

				// Draw the circles around the nodes
				function _drawCircles(nodes) {
					nodes.append("circle")
						.attr("class", "node")
						.call(_drag)
						.on("mousedown.drag", null)
						.on("dblclick", _dblClickNode)
						.on("click", _oneClickNode)
						.style("opacity", NODE_OPACITY_NORMAL)
						.attr("r", NODE_SIZE_NORMAL)
						// Tooltip
						.on('mouseover', _showItemDescription)
						.on('mouseout', _hideItemDescription);
				};

				// Add the labels onto the nodes
				function _drawText(nodes) {
					nodes.append("text")
						.attr("dx", 0)
						.attr("dy", ".35em")
						.style("font-size", FONT_SIZE_NORMAL)
						.attr("class", "text")
						.attr("text-anchor", "middle")
						.style("fill", STROKE_COLOR_NORMAL)
						.text(function(d) {
							return d.name
						});
				};

				// Invisible on draw.
				// Used to have the correct text size ready for when the user hovers
				function _prepareHoverText(nodes) {
					nodes.append("text")
						.attr("dx", 0)
						.attr("dy", ".35em")
						.style("font-size", FONT_SIZE_HOVER)
						.attr("class", "text")
						.attr("text-anchor", "middle")
						.style("fill", "transparent")
						.text(function(d) {
							return d.name;
						}).call(_getTextBox);

					// The rectangle around the text
					nodes.insert("rect", "text")
						.attr("x", function(d) {
							return d.bbox.x - TEXT_PADDING_SIZE / 2;
						})
						.attr("y", function(d) {
							return d.bbox.y - TEXT_PADDING_SIZE / 2;
						})
						.attr("width", function(d) {
							return d.bbox.width + TEXT_PADDING_SIZE;
						})
						.attr("height", function(d) {
							return d.bbox.height + TEXT_PADDING_SIZE;
						})
						.style("fill", "white")
						.style("stroke", "black")
						.on('mouseover', _showItemDescription)
						.on('mouseout', _hideItemDescription)
						.style("opacity", 0);
				};

				// Shows the link's info on the info panel; on hover
				function _onMouseOverEdge(edge) {
					_highlightHoveredEdgeNodes(edge);
					_highlightHoveredEdge(edge);
					_showItemDescription({name: 'Link description', description: edge.description});
				};

				function _highlightHoveredEdgeNodes(edge) {
					for(var i = 0; i < _nodes[0].length; i++) {
						var node = _nodes[0][i];

						if(node.innerHTML.indexOf(edge.source.name) > -1 || node.innerHTML.indexOf(edge.target.name) > -1)
						{
							node.children[0].style.opacity = NODE_OPACITY_HOVER;
							node.children[0].attributes[1].value = NODE_SIZE_HOVER;
						}
					};
				};

				function _highlightHoveredEdge(edge) {
					_links.style('stroke-width', function(anEdge) {
						if (edge.source === anEdge.source && edge.target === anEdge.target)
							return LINK_WIDTH_HOVER;
						else
							return LINK_WIDTH_NORMAL;
					});

					// Make wider if hovered node.
					_links.style('opacity', function(anEdge) {
						if (edge.source === anEdge.source && edge.target === anEdge.target)
							return LINK_OPACITY_HOVER;
						else
							return LINK_OPACITY_NORMAL;
					});
				};

				// Change the link & node styles on hover over a node
				// Variables configurable
				function _onMouseOverNode(node) {
					// Make the nodes edges bigger.
					_links.style('stroke-width', function(l) {
						if (node === l.source || node === l.target)
							return LINK_WIDTH_NODE_HOVER;
						else
							return LINK_WIDTH_NORMAL;
					});
					_links.style('opacity', function(l) {
						if (node === l.source || node === l.target)
							return LINK_OPACITY_HOVER;
						else
							return LINK_OPACITY_CONTEXT;
					});
					_links.style('stroke', function(l) {
						if (node === l.source || node === l.target) {
							return STROKE_COLOR_HOVER;
						}
						else
							return STROKE_COLOR_NORMAL;
					});

					// Change text font size
					d3.select(this).select('text')
						.transition()
						.duration(300)
						.style("font-size", FONT_SIZE_HOVER)
						.text(function(d) {return d.name;}).call(_getTextBox);


					// Make neighboring nodes more visible (white).
					d3.selectAll('circle').style("opacity", function(l) {
						return _neighboring(node,l) ? NODE_OPACITY_HOVER : NODE_OPACITY_NORMAL;
					});

					// Highlight the circle
					d3.select(this).select('circle')
						.style("opacity", NODE_OPACITY_HOVER)
						.transition()
						.duration(300)
						.attr("r", NODE_SIZE_HOVER);

					// Highlight the rectangle textBox
					d3.select(this).select('rect')
						.transition()
						.duration(300)
						.style("opacity", NODE_OPACITY_HOVER);
				};

				// Rectangle around text
				function _getTextBox(selection) {
					selection.each(function(d) {
						d.bbox = this.getBBox();
					})
				};

				// Undos all the onMouseOver attributes.
				function _onMouseOutEdge(edge) {
					for(var i = 0; i < _nodes[0].length; i++) {
						var node = _nodes[0][i];

						node.children[0].style.opacity = NODE_OPACITY_NORMAL;
						node.children[0].attributes[1].value = NODE_SIZE_NORMAL;
					};

					_hideItemDescription();

					// Set width and opacity to normal
					_links.style('stroke-width', function(l) { return LINK_WIDTH_NORMAL; });
					_links.style('opacity', function(l) { return LINK_OPACITY_NORMAL; });
				};

				function _onNodeMouseOut(node) {
					_links.style('stroke-width', 2);

					// Return text back to normal
					d3.select(this).select('text')
						.transition()
						.duration(300)
						.text(function(d) {
							return d.name;
						})
						.style("font-size", "10px")
					// Return all edges back to normal
					_links.style('opacity', function(l) { return LINK_OPACITY_NORMAL; });
					// Return the stroke back to normal
					// Only on nodes that have been changed
					_links.style('stroke', function(l) { if (node === l.source || node === l.target) return STROKE_COLOR_NORMAL; });

					// Return the circle back to normal
					d3.select(this).select('circle')
						.style("opacity", NODE_OPACITY_NORMAL)
						.transition()
						.duration(300)
						.attr("r", NODE_SIZE_NORMAL);

					// Remove the text rectangle
					d3.select(this).select('rect')
						.transition()
						.duration(300)
						.style("opacity", 0);

					// Reset neighbors
					d3.selectAll('circle').style("opacity", function(l) {
						return NODE_OPACITY_NORMAL;
					});
				};

				function _dblClickNode(d) {
					d3.select(this).classed("fixed", d.fixed = false);
				};

				function _oneClickNode(d) {
					console.log(d);
				};

				// Kick off
				_initRenderGraph();
			};

			return {
				render: _render
			}
		}

		return GraphRenderWorker;
	}());

    app.service('GraphRenderWorker', GraphRenderWorker);

}(angular.module('app.stickytree')));
