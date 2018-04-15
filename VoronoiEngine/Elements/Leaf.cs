using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Events;

namespace VoronoiEngine.Elements
{
    public class Leaf : INode
    {
        public INode Parent { get; set; }

        public Leaf(Point site)
        {
            Site = site;
        }

        public bool IsLeaf { get { return true; } }

        public Point Site { get; }

        public CircleEvent CircleEvent { get; set; }

        public INode Find(Point point)
        {
            return this;
        }

        public void GetDescendants(Point start, TraverseDirection direction, ICollection<INode> descendants, int count)
        {
            if (Site == start)
            {
                descendants.Add(this);
                return;
            }

            if (descendants.Any())
                descendants.Add(this);
        }

        public INode GetNeighbor(INode start, TraverseDirection direction)
        {
            return this;
        }

        public Leaf Clone()
        {
            return new Leaf(Site);
        }

        public override string ToString()
        {
            return $"Leaf: {Site.ToString()}, CircleEvent: {CircleEvent?.Point.ToString()}";
        }
    }
}