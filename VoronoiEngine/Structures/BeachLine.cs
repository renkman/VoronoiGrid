using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;

namespace VoronoiEngine.Structures
{
    public class BeachLine
    {
        public INode Root { get; private set; }

        public CircleEvent FindCircleEventAbove(Point site)
        {
            if (Root == null)
                return null;

            var arc = Root.Find(site) as Leaf;
            if (arc.CircleEvent == null)
                return null;

            return arc.CircleEvent;
        }

        public ICollection<HalfEdge> InsertSite(Point point)
        {
            var leaf = new Leaf(point);

            // Add new site to empty tree
            if (Root == null)
            {
                Root = leaf;
                return new List<HalfEdge>();
            }

            // Add site to existing tree
            var rootNode = Root as Node;
            if (rootNode != null)
            {
                rootNode.Insert(point, ReplaceLeaf);
                return new List< HalfEdge > ();
            }

            var rootLeaf = Root as Leaf;
            var node = new Node(null);
            Root = node;
            ReplaceLeaf(node, leaf, rootLeaf);
            return new List<HalfEdge> { node.Edge };
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
                var leftEvent = DetermineCircleEvent(leftArcs);
                if (leftEvent != null)
                    circleEvents.Add(leftEvent);
            }

            // ... and where it is the right arc.
            var rightArcs = new List<INode>();
            root.GetDescendants(site, TraverseDirection.CounterClockwise, rightArcs, 3);

            if (rightArcs.Count < 3)
                return circleEvents;

            var rightEvent = DetermineCircleEvent(rightArcs);
            if (rightEvent != null)
                circleEvents.Add(rightEvent);
            return circleEvents;
        }

        public CircleEvent GenerateSingleCircleEvent(Leaf arc)
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
            var circleEvent = DetermineCircleEvent(arcs);
            return circleEvent;
        }

        public void RemoveLeaf(Leaf leaf)
        {
            var parent = leaf.Parent as Node;
            var parentParent = parent.Parent as Node;
            if (parent.Left == leaf)
            {
                parent.Left = null;
                var right = parent.Right;
                if (parentParent.Left == parent)
                    parentParent.Left = right;
                else
                    parentParent.Right = right;
                return;
            }

            parent.Right = null;
            var left = parent.Left;
            if (parentParent.Left == parent)
                parentParent.Left = left;
            else
                parentParent.Right = left;
        }

        private static void ReplaceLeaf(Node subRoot, Leaf newLeaf, Leaf arc)
        {
            // Build subtree
            var node = new Node(subRoot);

            subRoot.Left = node;
            subRoot.Right = arc;
            subRoot.Breakpoint = new Tuple { Left = newLeaf.Site, Right = arc.Site };
            var subRootEdgeStart = subRoot.CalculateBreakpoint(newLeaf.Site.Y);
            subRoot.Edge = subRoot.Edge ?? new HalfEdge(subRootEdgeStart);
            subRoot.Edge.Add(subRootEdgeStart);

            arc.Parent = subRoot;

            node.Left = arc.Clone();
            node.Right = newLeaf;
            node.Breakpoint = new Tuple { Left = arc.Site, Right = newLeaf.Site };
            var nodeEdgeStart = node.CalculateBreakpoint(newLeaf.Site.Y);
            node.Edge = new HalfEdge(nodeEdgeStart);
            node.Edge.Add(nodeEdgeStart);
            node.Left.Parent = node;
            newLeaf.Parent = node;
        }

        private static CircleEvent DetermineCircleEvent(ICollection<INode> arcs)
        {
            var leaves = arcs.Cast<Leaf>().Select(l => l).OrderBy(s => s.Site.X).ToList();
            var circumcenter = CheckConversion(leaves[0].Site, leaves[1].Site, leaves[2].Site);
            if (circumcenter == null)
                return null;

            var circleEventPoint = CalculateCircle(circumcenter, leaves[0].Site);
            var circleEvent = new CircleEvent
            {
                Point = circleEventPoint,
                Vertex = circumcenter,
                LeftArc = leaves[0],
                CenterArc = leaves[1],
                RightArc = leaves[2]
            };
            leaves[1].CircleEvent = circleEvent;
            return circleEvent;
        }

        private static Point CheckConversion(Point a, Point b, Point c)
        {
            Point ba = b - a;
            Point ca = c - a;
            var baLength = Math.Pow(ba.X, 2) + Math.Pow(ba.Y, 2);
            var caLength = Math.Pow(ca.X, 2) + Math.Pow(ca.Y, 2);
            var denominator = 2 * (ba.X * ca.Y - ba.Y * ca.X);
            //if (denominator <= 0)
            if (denominator >= 0)
                // Equals 0 for colinear points.  Less than zero if points are ccw and arc is diverging.
                return null;  // Don't use this circle event!

            var circumcenter = new Point()
            {
                X = (int)Math.Round(a.X + (ca.Y * baLength - ba.Y * caLength) / denominator),
                Y = (int)Math.Round(a.Y + (ba.X * caLength - ca.X * baLength) / denominator)
            };
            return circumcenter;
        }

        private static Point CalculateCircle(Point circumcenter, Point a)
        {
            var radius = (int)(((circumcenter.X - a.X) * 2 + (circumcenter.Y - a.Y) * 2) * 0.5m);
            return new Point { X = circumcenter.X, Y = circumcenter.Y - radius };
        }
    }
}