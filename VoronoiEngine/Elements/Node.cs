using VoronoiEngine.Geomerty;
using VoronoiEngine.Models;
using VoronoiEngine.Utilities;

namespace VoronoiEngine.Elements
{
    public class Node : BaseNode, INode
    {
        //private readonly IBreakpointCalculationService _breakpointCalculationService;
        public INode Parent { get; set; }

        public bool IsLeaf { get { return false; } }

        public INode Left { get; set; }
        public INode Right { get; set; }

        public Tuple<Point> Breakpoint => new Tuple<Point> {
            Left = GetLeaf(TraverseDirection.CounterClockwise).Site,
            Right = GetLeaf(TraverseDirection.Clockwise).Site
        };

        public HalfEdge HalfEdge { get; set; }
        
        public Node(Node parent)
        {
            Parent = parent;
            _breakpointCalculationService = new BreakpointCalculationService();
            _logger = Logger.Instance;
        }

        public Point CalculateBreakpoint(double y)
        {
            return _breakpointCalculationService.CalculateBreakpoint(Breakpoint.Left, Breakpoint.Right, y);            
        }

        public INode Find(Point site)
        {
            var breakpoint = CalculateBreakpoint(site.Y);

            //if (breakpoint.X > site.X)
            if (site.CompareTo(breakpoint) < 0)
            {
                if (Left != null)
                    return Left.Find(site);
            }
            return Right != null ? Right.Find(site) : null;
        }

        public InsertSiteModel Insert(Point site)
        {
            var breakpoint = CalculateBreakpoint(site.Y);

            if (site.CompareTo(breakpoint) < 0)
            {
                if (Left != null)
                    return Left.Insert(site);
            }
            if (Right == null)
                throw new InvalidOperationException("Nodes without leaves must not exist!");

            return Right.Insert(site);
        }
                                
        public Leaf GetLeaf(TraverseDirection direction)
        {
            var left = direction == TraverseDirection.CounterClockwise;
            var node = left ? Left : Right;
            while (!node.IsLeaf)
                node = left ? ((Node)node).Right : ((Node)node).Left;
            return (Leaf)node;
        }

        public override string ToString()
        {
            return $"Node Left: {Breakpoint?.Left?.ToString()}, Right: {Breakpoint?.Right?.ToString()}";
        }
    }
}