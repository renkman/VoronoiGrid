using VoronoiEngine.Geomerty;
using VoronoiEngine.Models;
using VoronoiEngine.Utilities;

namespace VoronoiEngine.Elements
{
    public abstract class BaseNode
    {
        protected IBreakpointCalculationService _breakpointCalculationService;

        protected Logger _logger;

        protected InsertSiteModel ReplaceLeaf(Node subRoot, Leaf newLeaf, Leaf arc)
        {
            _logger.Log($"Replace leaf {arc.ToString()} with leaf {newLeaf.ToString()}");

            var y = _breakpointCalculationService.GetY(arc.Site, newLeaf.Site);
            var start = new Point(newLeaf.Site.X, y);
            
            // Build subtree
            var node = new Node(subRoot);
            
            subRoot.Left = node;
            subRoot.Right = arc;
            subRoot.HalfEdge = new HalfEdge(start, newLeaf.Site, arc.Site);
            arc.Parent = subRoot;

            var arcClone = arc.Clone();
            node.Left = arcClone;
            node.Right = newLeaf;
            node.HalfEdge = new HalfEdge(start, arc.Site, newLeaf.Site);
            node.HalfEdge.Neighbor = subRoot.HalfEdge;
            arcClone.Parent = node;
            newLeaf.Parent = node;

            _logger.Log($"Left edge = {node.HalfEdge}");
            _logger.Log($"Right edge = {subRoot.HalfEdge}");

            return new InsertSiteModel
            {
                HalfEdge = node.HalfEdge,
                Leaves = new List<Leaf> { arcClone, arc }
            };                
        }
    }
}
