using System;
using VoronoiEngine.Events;

namespace VoronoiEngine.Elements
{
    public class Leaf : INode
    {
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
    }
}