using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Geomerty;

namespace VoronoiEngine.Elements
{
    public class Node : INode
    {
        private readonly IBreakpointCalculationService _breakpointCalculationService;

        public INode Parent { get; set; }

        public bool IsLeaf { get { return false; } }

        public INode Left { get; set; }
        public INode Right { get; set; }

        public Tuple<Point> Breakpoint { get; }

        public HalfEdge HalfEdge { get; set; }
        
        public Node(Node parent)
        {
            Parent = parent;
            Breakpoint = new Tuple<Point>();
            _breakpointCalculationService = new BreakpointCalculationService();
        }

        public Point CalculateBreakpoint(int y)
        {
            return _breakpointCalculationService.CalculateBreakpoint(Breakpoint.Left, Breakpoint.Right, y);            
        }

        public INode Find(Point site)
        {
            var breakpoint = CalculateBreakpoint(site.Y);

            if (site.CompareTo(breakpoint) < 0)
            {
                if (Left != null)
                    return Left.Find(site);
            }
            if (Right != null)
                return Right.Find(site);

            return null;
        }

        public void GetDescendants(Point start, TraverseDirection direction, ICollection<INode> descendants, int count)
        {
            if (descendants.Count == count)
                return;

            if (direction == TraverseDirection.CounterClockwise)
            {
                if (Left != null)
                    Left.GetDescendants(start, direction, descendants, count);

                if (descendants.Count == count)
                    return;

                if (Right != null)
                    Right.GetDescendants(start, direction, descendants, count);
            }
            else
            {
                if (Right != null)
                    Right.GetDescendants(start, direction, descendants, count);

                if (descendants.Count == count)
                    return;

                if (Left != null)
                    Left.GetDescendants(start, direction, descendants, count);
            }
        }

        public INode GetNeighbor(INode start, TraverseDirection direction)
        {
            var consecutives = new List<INode>();
            FindNeighbor(start, direction, consecutives);
            if (consecutives.Count == 2)
                return consecutives[1];
            return null;
        }

        private void FindNeighbor(INode start, TraverseDirection direction, IList<INode> consecutives)
        {
            if (consecutives.Count == 2)
                return;

            if (direction == TraverseDirection.CounterClockwise)
            {
                if (Left != null)
                    FindNeighbour(Left, start, direction, consecutives);

                if (consecutives.Count == 2)
                    return;

                if (Right != null)
                    FindNeighbour(Right, start, direction, consecutives);
            }
            else
            {
                if (Right != null)
                    FindNeighbour(Right, start, direction, consecutives);

                if (consecutives.Count == 2)
                    return;

                if (Left != null)
                    FindNeighbour(Left, start, direction, consecutives);
            }
        }

        private static void FindNeighbour(INode currentNode, INode start, TraverseDirection direction, IList<INode> consecutives)
        {
            var node = currentNode as Node;
            if (node != null)
                node.FindNeighbor(start, direction, consecutives);
            else
            {
                var leaf = currentNode.GetNeighbor(start, direction);
                if (leaf == start || consecutives.Any())
                    consecutives.Add(leaf);
            }
        }

        public ICollection<HalfEdge> Insert(Point site, Func<Node, Leaf, Leaf, Point, ICollection<HalfEdge>> replace)
        {
            var breakpoint = CalculateBreakpoint(site.Y);
            ICollection<HalfEdge> edges = null;

            if (site.CompareTo(breakpoint) < 0)
            {
                if (Left != null)
                {
                    var leftNode = Left as Node;
                    if (leftNode != null)
                        edges = leftNode.Insert(site, replace);
                    else
                    {
                        var leftLeaf = Left as Leaf;

                        edges = ReplaceLeaf(site, leftLeaf, true, breakpoint, replace);
                        //if (Breakpoint.Left.CompareTo(site) == -1)
                        //    Breakpoint.Left = site;                        
                    }
                }
                return edges;
            }

            if (Right == null)
                throw new InvalidOperationException("Nodes without leaves must not exist!");

            var rightNode = Right as Node;
            if (rightNode != null)
                edges = rightNode.Insert(site, replace);
            else
            {
                var rightLeaf = Right as Leaf;
                edges = ReplaceLeaf(site, rightLeaf, false, breakpoint, replace);
                //if (Breakpoint.Right.CompareTo(site) == -1)
                //    Breakpoint.Right = site;
            }
            return edges;
        }

        private ICollection<HalfEdge> ReplaceLeaf(Point site, Leaf arc, bool isLeft, Point breakpoint, Func<Node, Leaf, Leaf, Point, ICollection<HalfEdge>> replace)
        {
            var node = new Node(this);
            if (isLeft)
            {
                Left = node;
                HalfEdge = new HalfEdge(breakpoint, site, arc.Site);
            }
            else
            {
                Right = node;
                HalfEdge = new HalfEdge(breakpoint, arc.Site, site);
            }

            var leaf = new Leaf(site);
            return replace(node, leaf, arc, breakpoint);
        }

        public void UpdateBreakpoints()
        {
            var left = Left.IsLeaf ? ((Leaf)Left).Site : ((Node)Left).Breakpoint.Right;
            var right = Right.IsLeaf ? ((Leaf)Right).Site : ((Node)Right).Breakpoint.Left;

            Breakpoint.Left = left;
            Breakpoint.Right = right;

            if (Parent != null)
            {
                var parent = (Node)Parent;
                parent.UpdateBreakpoints();
            }
        }

        public override string ToString()
        {
            return $"Node Left: {Breakpoint?.Left?.ToString()}, Right: {Breakpoint?.Right?.ToString()}";
        }
    }
}