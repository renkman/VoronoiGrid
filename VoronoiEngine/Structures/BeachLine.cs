using System.Collections.Generic;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Geomerty;

namespace VoronoiEngine.Structures
{
    public class BeachLine
    {
        private readonly ICircleEventCalculationService _circleEventCalculationService;

        public BeachLine()
        {
            _circleEventCalculationService = new CircleEventCalculationService();
        }

        public BeachLine(ICircleEventCalculationService circleEventCalculationService)
        {
            _circleEventCalculationService = circleEventCalculationService;
        }

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
                return new List<HalfEdge>();
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
                var leftEvent = _circleEventCalculationService.DetermineCircleEvent(leftArcs);
                if (leftEvent != null)
                    circleEvents.Add(leftEvent);
            }

            // ... and where it is the right arc.
            var rightArcs = new List<INode>();
            root.GetDescendants(site, TraverseDirection.CounterClockwise, rightArcs, 3);

            if (rightArcs.Count < 3)
                return circleEvents;

            var rightEvent = _circleEventCalculationService.DetermineCircleEvent(rightArcs);
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
            var circleEvent = _circleEventCalculationService.DetermineCircleEvent(arcs);
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
    }
}