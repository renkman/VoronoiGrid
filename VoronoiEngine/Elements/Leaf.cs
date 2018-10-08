using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Events;
using VoronoiEngine.Geomerty;
using VoronoiEngine.Models;
using VoronoiEngine.Utilities;

namespace VoronoiEngine.Elements
{
    public class Leaf : BaseNode, INode
    {
        public Leaf(Point site)
        {
            Site = site;
            _logger = Logger.Instance;
            _breakpointCalculationService = new BreakpointCalculationService();
        }

        public INode Parent { get; set; }
                
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

        public InsertSiteModel Insert(Point site)
        {
            var leaf = new Leaf(site);
            var parent = Parent as Node;
            var node = new Node(parent);
            if(parent != null)
            {
                if (this == parent.Left)
                    parent.Left = node;
                else
                    parent.Right = node;
            }
            var result = ReplaceLeaf(node, leaf, this);
            return result;
        }

        public Node GetParent(TraverseDirection direction)
        {
            var parent = (Node)Parent;
            INode node = this;
            while ((direction == TraverseDirection.CounterClockwise ? parent.Left : parent.Right) == node)
            {
                if (parent.Parent == null)
                    return null;
                node = parent;
                parent = (Node)parent.Parent;
            }
            return parent;
        }
    }
}