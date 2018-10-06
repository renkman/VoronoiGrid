using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoronoiEngine.Geomerty;
using VoronoiEngine.Utilities;

namespace VoronoiEngine.Elements
{
    public abstract class BaseNode
    {
        protected IBreakpointCalculationService _breakpointCalculationService;

        protected Logger _logger;

        protected ICollection<HalfEdge> ReplaceLeaf(Node subRoot, Leaf newLeaf, Leaf arc)
        {
            _logger.Log($"Replace leaf {arc.ToString()} with leaf {newLeaf.ToString()}");

            var y = _breakpointCalculationService.GetY(arc.Site, newLeaf.Site);
            var start = new Point(newLeaf.Site.X, y);

            // Build subtree
            var node = new Node(subRoot);
            
            subRoot.Left = node;
            subRoot.Right = arc;
            subRoot.Breakpoint.Left = newLeaf.Site;
            subRoot.Breakpoint.Right = arc.Site;
            subRoot.HalfEdge = new HalfEdge(start, newLeaf.Site, arc.Site);
            arc.Parent = subRoot;

            var arcClone = arc.Clone();
            node.Left = arcClone;
            node.Right = newLeaf;
            node.Breakpoint.Left = arcClone.Site;
            node.Breakpoint.Right = newLeaf.Site;
            node.HalfEdge = new HalfEdge(start, arc.Site, newLeaf.Site);
            node.HalfEdge.Neighbor = subRoot.HalfEdge;
            arcClone.Parent = node;
            newLeaf.Parent = node;          

            return new List<HalfEdge> { subRoot.HalfEdge, node.HalfEdge };
        }
    }
}
