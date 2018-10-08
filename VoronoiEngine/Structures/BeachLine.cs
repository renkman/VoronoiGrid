using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Geomerty;
using VoronoiEngine.Models;
using VoronoiEngine.Utilities;

namespace VoronoiEngine.Structures
{
    public class BeachLine
    {
        private readonly ICircleEventCalculationService _circleEventCalculationService;
        private readonly IBreakpointCalculationService _breakpointCalculationService;
        private readonly Logger _logger;

        public BeachLine() : this(new CircleEventEdgeCalculationService(), new BreakpointCalculationService())
        {
        }

        public BeachLine(ICircleEventCalculationService circleEventCalculationService, IBreakpointCalculationService breakpointCalculationService)
        {
            _circleEventCalculationService = circleEventCalculationService;
            _breakpointCalculationService = breakpointCalculationService;
            _logger = Logger.Instance;
        }

        public INode Root { get; private set; }

        public CircleEvent FindCircleEventAbove(Point site)
        {
            if (Root == null)
                return null;

            var arc = Root.Find(site) as Leaf;
            if (arc?.CircleEvent == null)
                return null;

            return arc.CircleEvent;
        }

        public InsertSiteModel InsertSite(Point point)
        {
            var leaf = new Leaf(point);

            // Add new site to empty tree
            if (Root == null)
            {
                Root = leaf;
                _logger.Log($"Add {leaf.Site.ToString()} as Root");
                return null;
            }

            var root = Root;
            var result = root.Insert(point);

            // Reset root if root was a leaf
            if (root.IsLeaf)
                Root = root.Parent;
            return result;
        }

        public ICollection<CircleEvent> GenerateCircleEvent(ICollection<Leaf> sites, double y)
        {
            if (sites == null)
                return null;
            return sites.Select(s => _circleEventCalculationService.DetermineCircleEvent(new[] { s }, y)).ToList();
        }

        public ICollection<CircleEvent> GenerateCircleEvent(Point site)
        {
            var root = Root as Node;
            if (root == null)
                return null;

            var circleEvents = new List<CircleEvent>();

            // Find triple of consecutive arcs where the arc for the new site is
            // the left arc...
            var leftArcs = new List<INode>();
            root.GetDescendants(site, TraverseDirection.Clockwise, leftArcs, 3);

            if (leftArcs.Count == 3)
            {
                var leftEvent = _circleEventCalculationService.DetermineCircleEvent(leftArcs, site.Y);
                if (leftEvent != null)
                {
                    var leftArcsString = string.Join(", ", leftArcs.Select(a => a.ToString()).ToArray());
                    _logger.Log($"Found circle event for arcs: {leftArcsString} at Point: {leftEvent.Point.ToString()} and Vertex: {leftEvent.Vertex.ToString()}");
                    circleEvents.Add(leftEvent);
                }
            }

            // ... and where it is the right arc.
            var rightArcs = new List<INode>();
            root.GetDescendants(site, TraverseDirection.CounterClockwise, rightArcs, 3);

            if (rightArcs.Count < 3)
                return circleEvents;

            var rightEvent = _circleEventCalculationService.DetermineCircleEvent(rightArcs, site.Y);
            if (rightEvent != null)
            {
                var rightArcsString = string.Join(", ", rightArcs.Select(a => a.ToString()).ToArray());
                _logger.Log($"Found circle event for arcs: {rightArcsString} at Point: {rightEvent.Point.ToString()} and Vertex: {rightEvent.Vertex.ToString()}");
                circleEvents.Add(rightEvent);
            }
            return circleEvents;
        }

        public CircleEvent GenerateSingleCircleEvent(Leaf arc, double y)
        {
            var root = Root as Node;
            if (root == null)
                return null;

            // Find triple of consecutive arcs where the passed arc is the middle arc
            var leftArc = root.GetNeighbor(arc, TraverseDirection.Clockwise);
            if (leftArc == null)
                return null;

            var rightArc = root.GetNeighbor(arc, TraverseDirection.CounterClockwise);
            if (rightArc == null)
                return null;

            var arcs = new List<INode> { leftArc, arc, rightArc };
            var sites = arcs.Cast<Leaf>().Select(a => a.Site).Distinct().ToList();
            if (sites.Count != 3)
                return null;

            var circleEvent = _circleEventCalculationService.DetermineCircleEvent(arcs, y);
            if (circleEvent == null)
                return null;

            var arcsString = string.Join(", ", arcs.Select(a => a.ToString()).ToArray());
            _logger.Log($"Found circle event for arcs: {arcsString} at Point: {circleEvent.Point.ToString()} and Vertex: {circleEvent.Vertex.ToString()}");
            return circleEvent;
        }

        public Node RemoveLeaf(Leaf leaf)
        {
            var parent = leaf.Parent as Node;
            var parentParent = parent.Parent as Node;
            leaf.Parent = null;
            parent.Parent = null;

            RemoveLeaf(leaf, parent, parentParent, parent.Left == leaf);
            if (parentParent == null)
                return null;

            return parentParent;
        }

        public override string ToString()
        {
            var output = "";
            BuildStringFromTree(Root, output, 0);
            return output;
        }

        private void RemoveLeaf(Leaf leaf, Node parent, Node parentParent, bool isLeft)
        {
            if (parentParent == null)
            {
                Root = isLeft ? parent.Right : parent.Left;
                return;
            }

            //if (isLeft)
            //    parent.Left = null;
            //else
            //    parent.Right = null;

            var sibling = isLeft ? parent.Right : parent.Left;
            sibling.Parent = parentParent;

            //if (isLeft)
            //    parent.Right = null;
            //else
            //    parent.Left = null;

            if (parentParent.Left == parent)
                parentParent.Left = sibling;
            else
                parentParent.Right = sibling;
        }

        private void BuildStringFromTree(INode node, string output, int level)
        {
            if (node == null)
                return;

            var tabs = string.Join("", Enumerable.Range(0, level).Select(n => "\t").ToArray());
            output += $"{tabs}{node.ToString()}{Environment.NewLine}";
            if (node.IsLeaf)
                return;

            var innerNode = node as Node;
            BuildStringFromTree(innerNode.Left, output, ++level);
            BuildStringFromTree(innerNode.Right, output, level);
            level--;
        }
    }
}