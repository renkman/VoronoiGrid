﻿using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Geomerty;
using VoronoiEngine.Models;
using VoronoiEngine.Utilities;

namespace VoronoiEngine.Structures
{
    public class BeachLine
    {
        private readonly int _height;
        private readonly ICircleEventCalculationService _circleEventCalculationService;
        private readonly IBreakpointCalculationService _breakpointCalculationService;
        private readonly Logger _logger;

        public BeachLine(int height) : this(height, new CircleEventCalculationService(), new BreakpointCalculationService())
        {
        }

        public BeachLine(int height, ICircleEventCalculationService circleEventCalculationService, IBreakpointCalculationService breakpointCalculationService)
        {
            _height = height;
            _circleEventCalculationService = circleEventCalculationService;
            _breakpointCalculationService = breakpointCalculationService;
            _logger = Logger.Instance;
        }

        public INode Root { get; private set; }

        public CircleEvent FindCircleEventAbove(Point site)
        {
            var arc = Root?.Find(site) as Leaf;
            return arc?.CircleEvent;
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
            var arc = root as Leaf;
            if (root.IsLeaf && arc.Site.Y - point.Y < 0.0001)
                return InsertLeafOnSameYAxisLikeRoot(arc, point);

            var result = root.Insert(point);

            // Reset root if root was a leaf
            if (root.IsLeaf)
                Root = root.Parent;
            return result;
        }

        public ICollection<CircleEvent> GenerateCircleEvent(ICollection<Leaf> sites, double y)
        {
            var circleEvents = sites?.Select(s => _circleEventCalculationService.DetermineCircleEvent(s, y)).Where(e => e != null).ToList();
            if ((circleEvents?.Any()).GetValueOrDefault())
                _logger.Log(
                    $"Found circle events: {string.Join(", ", circleEvents.Select(c => $"{c.Point} at Arc: {c.Arc.Site}").ToArray())}");
            return circleEvents;
        }
               
        public Node RemoveLeaf(Leaf leaf)
        {
            var parent = leaf.Parent as Node;
            var parentParent = parent.Parent as Node;
            leaf.Parent = null;
            parent.Parent = null;

            RemoveLeaf(leaf, parent, parentParent, parent.Left == leaf);
            return parentParent;
        }

        public void	FinishEdge(INode node, int width)
        {
            if (node.IsLeaf)
                return;

            var innerNode = (Node)node;
            var mx = innerNode.HalfEdge.Direction.X > 0.0
                ? Math.Max(width, innerNode.HalfEdge.Point.X + 10)
                : Math.Min(0.0, innerNode.HalfEdge.Point.X - 10);

            //if(innerNode.HalfEdge.EndPoint != null)
            //    throw new InvalidOperationException($"Endoint for {innerNode.HalfEdge} already set!");
            var end = new Point(mx, mx * innerNode.HalfEdge.F + innerNode.HalfEdge.G);

            if (double.IsNaN(end.Y))
            {
                end.X = innerNode.HalfEdge.Point.X;
                end.Y = 0;
            }

            _logger.Log($"{node} Set {innerNode.HalfEdge} Direction: {innerNode.HalfEdge.Direction} End: {end}");



            innerNode.HalfEdge.EndPoint = end;
            			
            FinishEdge(innerNode.Left, width);
            FinishEdge(innerNode.Right, width);
        }

        public override string ToString()
        {
            var output = "";
            BuildStringFromTree(Root, output, 0);
            return output;
        }

        private InsertSiteModel InsertLeafOnSameYAxisLikeRoot(Leaf arc, Point point)
        {
            var newRoot = new Node(null);
            Root = newRoot;

            var newLeaf = new Leaf(point);
            var start = new Point((arc.Site.X + point.X) / 2, _height);

            var halfEdge = newLeaf.Site.X > arc.Site.X ?
                new HalfEdge(start, arc.Site, point) :
                new HalfEdge(start, point, arc.Site);
            newRoot.HalfEdge = halfEdge;
            newRoot.Left = arc;
            arc.Parent = newRoot;
            newRoot.Right = newLeaf;
            newLeaf.Parent = newRoot;
            
            return new InsertSiteModel
            {
                HalfEdge = halfEdge
            };
        }

        private void RemoveLeaf(Leaf leaf, Node parent, Node parentParent, bool isLeft)
        {
            _logger.Log($"Remove leaf {leaf.Site}");
            if (parentParent == null)
            {
                Root = isLeft ? parent.Right : parent.Left;
                return;
            }

            var sibling = isLeft ? parent.Right : parent.Left;
            sibling.Parent = parentParent;
            
            if (parentParent.Left == parent)
                parentParent.Left = sibling;
            else
                parentParent.Right = sibling;

            parent.Left = null;
            parent.Right = null;

            if(sibling.Parent == null)
                throw new InvalidOperationException($"Remove leaf {leaf.Site}: Parent of node {0} is null");
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