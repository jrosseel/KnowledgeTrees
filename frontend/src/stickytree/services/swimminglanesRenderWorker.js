(function (app) {
    'use strict';

    // Service that notifies the user through a bootstrap subtle popup
    var SwimmingLanesRenderWorker = (function () {

        function SwimmingLanesRenderWorker() {
        	//#section Render Chart
        	function _render(dataLoc, anchor, config) {
				// Renders the background chart (swimming lanes = stacked area chart)
				// Inspired by @src: https://bl.ocks.org/mbostock/3885211, 29/04/2016
				function _initRenderChart() {
					// Empty canvas before showing new data inside of it
					$(anchor).empty();
					d3.json(dataLoc, _renderChart);
				};

				// Render the swimming lanes
				function _renderChart(error, data) {

					if(! error) {
						// Enter correct data for X and Y axis
						var x = _createX(data.swimmingLanes, data.widthInPixels)
						var xAxis = _createXAxis(x);
						var y = _createY(data.swimmingLanes, data.heightInPixels);
						var yAxis = _createYAxis(y);

						// Colors used in swimming lanes
						var colors = d3.scale.category20c();

						var stackLayout = _createStackLayout();
						var nest = _configureSwimmingLanesNesting(data.swimmingLaneLegend);

						var areaChart = _createAreaChart(x, y);
						var formattedData = _formatSLData(data.swimmingLanes);

						var layers = stackLayout(nest.entries(data.swimmingLanes));

						// extend the domain with the formatted data
						x.domain(d3.extent(formattedData, function(d) { return d.x; }));
						y.domain([0, d3.max(formattedData, function(d) { return d.y0 + d.y; })]);

						var svg = _createSvgContainer(anchor, data.widthInPixels, data.heightInPixels);
						svg.selectAll(".layer")
							.data(layers)
							.enter().append("path")
							//.attr("class", "layer")
							//.attr("data-legend",function(d) { return d.values[0].laneName})
							.attr("class", function(d) {
								return "layer swimmingLane lane" + d.values[0].laneId;
							})
							.attr("laneId", function(d, i) {
								return "lane" + d.values[0].laneId;
							})
							.attr("color", function(d, i) {
								return colors(i);
							})
							.attr("d", function(d) {
								return areaChart(d.values);
							})
							.style("fill", function(d, i) {
								return colors(i);
							});

						// It appends a 'g' element to the SVG. g element is used to group SVG shapes together
						// @src: http://stackoverflow.com/questions/17057809/d3-js-what-is-g-in-appendg-d3-js-code
						svg.append("g")
							.attr("class", "x axis")
							.attr("transform", "translate(0," + data.heightInPixels + ")")
							.call(xAxis);

						svg.append("g")
							.attr("class", "y axis")
							.call(yAxis);

						_configureSLLegend(data.swimmingLaneLegend);
					}
					else { // if(! error)
						// TODO: throw error handling
					}
				};

				function _createX(data, width) {
					var x = d3.time.scale()
									.range([0, width]);
					return x;
				}

				// Creates the chart x-axis
				function _createXAxis(x) {
					return d3.svg.axis()
									.scale(x)
									.orient("bottom");
				};

				function _createY(data, height) {
					var y = d3.scale.linear().range([height, 0]);

					return y;
				}

				// Creates the chart y-axis
				function _createYAxis(y) {
					return d3.svg.axis()
									.scale(y)
									.orient("left");
				};

				// Creator function for the stack layout.
				function _createStackLayout() {
					return d3.layout.stack()
								.offset("zero")
								.values(function(d) {
									return d.values;
								})
								.x(function(d) {
									return d.x;
								})
								.y(function(d) {
									return d.y;
								});
				};

				// Nesting groups the data into swimming lanes - Sorting from @src: http://bl.ocks.org/phoebebright/raw/3176159/
				function _configureSwimmingLanesNesting(legend) {
					return d3.nest()
								.key(function(d) {
									return d.laneId;
								})
								// BUGFIX: wrong order of the swimming lanes.
								.sortKeys(function(a,b) { return _findIndexInLegend(a, legend) -_findIndexInLegend(b, legend) });
				};

					function _findIndexInLegend(item, legend) {
						for(var i = 0; i < legend.length; i++) {
							if(legend[i].id == item)
								return i;
						}
						return 0;
					}

				function _createAreaChart(x, y) {
					return d3.svg.area()
									.interpolate("cardinal")
									.x(function(d) {
										return x(d.x);
									})
									.y0(function(d) {
										return y(d.y0);
									})
									.y1(function(d) {
										return y(d.y0 + d.y);
									});
				};

				function _createSvgContainer(anchor, width, height) {
					d3.select(anchor)
							 .append("div")
							 .attr("style", "width:" + width + "; height:" + height + "; visibility:hidden;");

					return	d3.select(anchor).append("svg")
								.attr("id", "swimmingLanes")
								.attr("width", width)
								.attr("height", height)
								.append("g");
				};

				function _formatSLData(data) {
					var data = [].concat.apply([], data);

					/* Change data formats etc.*/
					data.forEach(function(d) {
						//d.x = format.parse(d.x);
						d.y = +d.y;
					});

					return data;
				};

				// The swimming lanes legend
				function _configureSLLegend(swimmingLaneLegendData) {

					var _forcefocus = [];

					function _createLegend(swimmingLaneLegendData) {
						//legend
						d3.select('#legendBase').html("").insert('div', '.chart').attr('class', 'legend').selectAll('span')
							.data(swimmingLaneLegendData)
							.enter().append('span')
							.attr('laneId', function(d) {
								return "lane" + d.id;
							})
							.attr('class', 'layer swimmingLane')
							.html(function(d) { return d.name; })
							.each(function(d) {
								var color = d3.selectAll(".layer.swimmingLane.lane" + d.id).data([0]).attr("color");
								d3.select(this).style('background-color', color);
							})
							.on('mouseover', _onLegendMouseOver)
							.on('mouseout', _onLegendMouseOut)
							.on('click', _onLegendClick);
					};

					function _onLegendMouseOver() {
						var hovered = d3.select(this).attr("laneId");
						//SELECT ALL SWIMMINGLANES AND FILTER THE SWIMMINGLANE YOU HOVERED
						d3.selectAll(".layer.swimmingLane").filter(function() {
							if (d3.select(this).attr("laneId") !== hovered) {
								return true;
							} else {
								d3.select(this).classed("focus", true);
								return false;
							}
						}).classed("faded", true);

						// HIDE NODES
						d3.selectAll(".chapternode").filter(function() {
							if (d3.select(this).attr("laneId") !== hovered) {
								//*//TODO HIDE edges
								var nodeId = d3.select(this).attr("nodeId");
								d3.selectAll(".chapterlink").filter(function() {
									if ("node"+d3.select(this).attr("targetId") == nodeId || "node"+d3.select(this).attr("sourceId") == nodeId) {
										return true;
									} else {
									//	d3.select(this).classed("focus", true);
										return false;
									}
								}).classed("faded", true);
								//*/
								return true;
							} else {
								d3.select(this).classed("focus", true);
								return false;
							}
						}).classed("faded", true);
					};

					function _onLegendMouseOut() {
						if(_forcefocus.length === 0)
						{
							d3.selectAll(".layer.swimmingLane").classed("focus", false)
															   .classed("faded", false);
							// HIDE NODES
							d3.selectAll(".chapternode").classed("focus", false)
														.classed("faded", false);
						 	///HIDE edges
							d3.selectAll(".chapterlink").classed("focus", false)
														.classed("faded", false);
						}
					};

					// On legend click: show that swimming lane for filter
					function _onLegendClick() {
						var clicked = d3.select(this).attr("laneId");
						console.log("Clicked legend item at: " + clicked);

						var index = _forcefocus.indexOf(clicked);
						console.log(_forcefocus);
						if (index > -1)
							_blurSwimmingLane(this, clicked, index);
						else
							_highlightSwimmingLane(this, clicked, index);
            console.log(_forcefocus);
            console.log("erna");
					};

					function _blurSwimmingLane(ctx, clicked, index) {
						console.log("Item already in focus. Remove focus.");
					    _forcefocus.splice(index, 1);

						// Remove the focus on the swimming lane
						d3.selectAll(".layer.swimmingLane")
						  	.filter(
							  	function() { // If we are on the clicked laneId, remove the class and stop iterating.
									return d3.select(this).attr("laneId") == clicked;
						  		}
						  	).classed("forcefocus", false);

						// Remove the highlighted nodes and their edges
						d3.selectAll(".chapternode")
						  	.filter(function() {
								if (d3.select(this).attr("laneId") == clicked)
								{
									var nodeId = d3.select(this).attr("nodeId");

									// Hide edges linked to this node.
									d3.selectAll(".chapterlink")
									  	.filter(function() {
											return (_createNodeIdFromContext(this, "targetId") == nodeId || _createNodeIdFromContext(this, "sourceId") == nodeId);
									  	})
									  	.classed("forceShow", false);

									// Hide node itself
									d3.select(this).classed("forcefocus", false);
									return false;
								}
						   	});
					};

					function _highlightSwimmingLane(ctx, clicked, index) {
						console.log("new focus");
						_forcefocus.push(clicked);
						//d3.select(this).classed("forcefocus", true);
						d3.selectAll(".layer.swimmingLane").filter(function() {
							if (d3.select(this).attr("laneId") == clicked) {
								d3.select(this).classed("forcefocus", true);
								return false;
							}
						});
						// HIDE NODES
						d3.selectAll(".chapternode").filter(function() {
							if (d3.select(this).attr("laneId") == clicked) {

								//*//HIDE EDGES
							var nodeId = d3.select(this).attr("nodeId");
							console.log("HIERUIT"+nodeId);
							d3.selectAll(".chapterlink").filter(function() {
								if ("node"+d3.select(this).attr("targetId") == nodeId || "node"+d3.select(this).attr("sourceId") == nodeId) {
									return true;
								} else {
								//	d3.select(this).classed("focus", true);
									return false;
								}
							}).classed("forceShow", true);
							//*/

								d3.select(this).classed("forcefocus", true);
								return false;
							}
						});
					};

					function _createNodeIdFromContext(ctx, attr) {
						return "node"+d3.select(ctx).attr(attr);
					};

					_createLegend(swimmingLaneLegendData);
				};

				_initRenderChart();
        	};

			return {
                render: _render
            };
        };


        return SwimmingLanesRenderWorker;
    }());

    app.service('SwimmingLanesRenderWorker', SwimmingLanesRenderWorker);

}(angular.module('app.stickytree')));
