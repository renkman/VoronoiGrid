using System;
using System.Collections.Generic;

namespace VoronoiEngine.Elements
{
    public class Node : INode
    {
        public INode Parent { get; set; }

        public bool IsLeaf { get { return false; } }

        public INode Left { get; set; }
        public INode Right { get; set; }

        public Tuple Breakpoint { get; set; }
        public HalfEdge Edge { get; set; }

        public Node(Node parent)
        {
            Parent = parent;
        }

        public Point CalculateBreakpoint(int y)
        {
            if(Breakpoint.Left.Y == Breakpoint.Right.Y)
                return new Point { X = (int)(Breakpoint.Right.X * 0.5m), Y = y };

            if (Breakpoint.Left.Y == y)
                return new Elements.Point { X = Breakpoint.Left.X, Y = y };

            if (Breakpoint.Right.Y == y)
                return new Elements.Point { X = Breakpoint.Right.X, Y = y };
            
            // x = (ay*by - Sqrt(ay*by((ay-by)²+b²))) / (ay-by)
            var x = (int)Math.Round((Breakpoint.Left.Y * Breakpoint.Right.X - Math.Sqrt(Breakpoint.Left.Y * Breakpoint.Right.Y * (Math.Pow(Breakpoint.Left.Y - Breakpoint.Right.Y, 2) + Math.Pow(Breakpoint.Right.X, 2)))) / (Breakpoint.Left.Y - Breakpoint.Right.Y));

            return new Point { X = x, Y = y };
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
            throw new InvalidOperationException("Nodes without leaves must not exist!");
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

        public void Insert(Point site, Action<Node, Leaf, Leaf> replace)
        {
            var breakpoint = CalculateBreakpoint(site.Y);
            if (site.CompareTo(breakpoint) < 0)
            {
                if (Left != null)
                {
                    var leftNode = Left as Node;
                    if (leftNode != null)
                        leftNode.Insert(site, replace);
                    else
                    {
                        var leftLeaf = Left as Leaf;
                        ReplaceLeaf(site, leftLeaf, true, replace);
                        //if (Breakpoint.Left.CompareTo(site) == -1)
                        //    Breakpoint.Left = site;
                    }
                }
                return;
            }

            if (Right == null)
                throw new InvalidOperationException("Nodes without leaves must not exist!");

            var rightNode = Right as Node;
            if (rightNode != null)
                rightNode.Insert(site, replace);
            else
            {
                var rightLeaf = Right as Leaf;
                ReplaceLeaf(site, rightLeaf, false, replace);
                //if (Breakpoint.Right.CompareTo(site) == -1)
                //    Breakpoint.Right = site;
            }
        }

        private void ReplaceLeaf(Point site, Leaf arc, bool isLeft, Action<Node, Leaf, Leaf> replace)
        {
            var node = new Node(this);
            if (isLeft)
                Left = node;
            else
                Right = node;

            var leaf = new Leaf(site);
            replace(node, leaf, arc);
        }
    }
}