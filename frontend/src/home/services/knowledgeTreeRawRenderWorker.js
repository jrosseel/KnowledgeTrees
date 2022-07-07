(function (app) {

    var KnowledgeTreeRawRenderWorker = (function () {

        function KnowledgeTreeRawRenderWorker() {

            // Configure Colour Palette
            var _colorPallete = d3.scale.category20();
            var _tooltip = {};

            function _render(dataLoc, anchor, config) {
                $(anchor).empty();

                var svg = d3.select(anchor).append("svg")
                                .attr("width", config.dimensions.x)
                                .attr("height", config.dimensions.y);

                /*d3.xhr("http://localhost:50718/api/KnowledgeTree/GetForceDirectedVisgraph")
                  .header("Content-Type", "application/json")
                  .post(JSON.stringify({ SubjectId: 15, ScopeToTopic: true }), */
                d3.json(dataLoc,
                    function(error, result) {
                        if (error) throw error;
                        
                        var cb = _tickForce.bind(this);
                        var force = _createForce(result.nodes, result.links, cb, config);

                        _configureArrows(svg);

                        var paths = _configurePaths(svg, force);

                        var links = _configureLinks(svg, result.links, result.nodes);
                        var nodes = config.showLabels 
                                        ? _configureNodesWithLabels(svg, force, result.nodes)
                                        : _configureNodesWithTooltips(svg, force, result.nodes);


                        // Register necessary this-params for cb
                        this.paths = paths;
                        this.nodes = nodes;
                    });
            };

            function _createForce(nodes, links, cb, config) {
                return d3.layout.force()
                            .nodes(d3.values(nodes))
                            .links(links)
                            .size([config.dimensions.x, config.dimensions.y])
                            .linkDistance(config.distance)
                            .charge(config.charge)
                            .on("tick", cb)
                            .start();
            };

            function _configureLinks(svg, links, nodes) {
                // Compute the distinct nodes from the links.

                // Don't see the point here
                /*for(var j = 0; j < links.length; j++) {
                  var link = links[j];
                 
                  link.source = nodes[link.source.index] || 
                        (nodes[link.source] = {name: link.source});
                    link.target = nodes[link.target.index] || 
                        (nodes[link.target] = {name: link.target});
                    link.value = +link.value;
                };*/
            };

            function _configureNodesWithLabels(svg, force, nodes) {
                var nodes = _configureNodes(svg, force, nodes);
                
                var circles = _configureCircles(nodes);
                _addLabels(nodes);

                return nodes;
            };

            function _configureNodesWithTooltips(svg, force, nodes) {
                var nodes = _configureNodes(svg, force, nodes);
                
                var circles = _configureCircles(nodes);
                
                circles.on("mouseover", function(d) {      
                            _tooltip.transition()        
                                .duration(200)      
                                .style("opacity", .9);      
                            _tooltip.html(d.name)  
                                .style("left", (d3.event.pageX) + "px")     
                                .style("top", (d3.event.pageY - 28) + "px");    
                            })                  
                        .on("mouseout", function(d) {       
                            _tooltip.transition()        
                                .duration(500)      
                                .style("opacity", 0);   
                            });

                return nodes;
            };

            function _configureNodes(svg, force, nodes) {
                var nodes = svg .selectAll(".node")
                                    .data(force.nodes())
                                .enter().append("g")
                                    .attr("class", "node")
                                    .call(force.drag);
                return nodes;
            };


            function _configureCircles(nodes) {
                nodes.append("circle")
                        .attr("r", 10)
                        .style("fill", function(d) { return _colorPallete(d.group); });
                return nodes;
            }

            function _addLabels(nodes) {
                return nodes.append("text")
                            .attr("x", 12)
                            .attr("dy", ".35em")
                            .text(function(d) { return d.name; });
            };

            function _addTooltips(svg, force) {
                
            };

            function _configureArrows(svg) {
                // build the arrow.
                svg.append("svg:defs").selectAll("marker")
                    .data(["end"])      // Different link/path types can be defined here
                  .enter().append("svg:marker")    // This section adds in the arrows
                    .attr("id", String)
                    .attr("viewBox", "0 -5 10 10")
                    .attr("refX", 15)
                    .attr("refY", -1.5)
                    .attr("markerWidth", 6)
                    .attr("markerHeight", 6)
                    .attr("orient", "auto")
                  .append("svg:path")
                    .attr("d", "M0,-5L10,0L0,5");
            };

            function _configurePaths(svg, force) {
                // add the links and the arrows
                var paths = svg.append("svg:g").selectAll("path")
                                    .data(force.links())
                              .enter().append("svg:path")
                                    //.attr("class", function(d) { return "link " + d.type; })
                                    .attr("class", "link")
                                    .attr("marker-end", "url(#end)");
                return paths;
            }

            function _tickForce(e) {
                this.paths.attr("d", function(d) {
                    var dx = d.target.x - d.source.x,
                        dy = d.target.y - d.source.y,
                        dr = Math.sqrt(dx * dx + dy * dy);
                    return "M" + 
                        d.source.x + "," + 
                        d.source.y + "A" + 
                        dr + "," + dr + " 0 0,1 " + 
                        d.target.x + "," + 
                        d.target.y;
                });

                this.nodes
                    .attr("transform", function(d) { 
                    return "translate(" + d.x + "," + d.y + ")"; });
            };


            // Will be executed only once at page load. Creates the tooltip holder.
            function _init() {
                _tooltip = d3.select("body").append("div")   
                            .attr("class", "tooltip")               
                            .style("opacity", 0);
            }
            _init();

            return {
                render: _render
            };
        };
        return KnowledgeTreeRawRenderWorker;
    }());
    
    app.service('KnowledgeTreeRawRenderWorker', KnowledgeTreeRawRenderWorker);

}(angular.module('app.home')));