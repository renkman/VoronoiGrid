using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;

namespace VoronoiEngine.Geomerty
{
    public class CircleEventEdgeCalculationService : ICircleEventCalculationService
    {
        public CircleEvent DetermineCircleEvent(ICollection<INode> arcs, double y)
        {
            var leaves = arcs.Cast<Leaf>().Select(l => l).ToList();
            var sites = leaves.Select(l => l.Site).Distinct().ToList(); //.OrderBy(s => s.X).Distinct().ToList();

            if (sites.Count != 3)
                return null;

            var edges = arcs.Select(a => a.Parent).Cast<Node>().Select(p => p.HalfEdge).Distinct().ToList();
            if (edges.Count != 2)
                throw new InvalidOperationException($"Corrupt tree structure with {edges.Count} parent edges.");

            // Check conversion by edge intersection
            var intersection = edges[0].Intersect(edges[1]);
            if (intersection == null)
                return null;

            var x = sites[0].X - intersection.X;
            var dy = sites[0].Y - intersection.Y;

            var d = Math.Sqrt(Math.Sqrt(x) + Math.Sqrt(dy));

            var circleEventPoint = new Point(intersection.X, intersection.Y - d);

            if (circleEventPoint.Y >= y)
                return null;

            var circleEvent = new CircleEvent
            {
                LeftArc = leaves[0],
                CenterArc = leaves[1],
                RightArc = leaves[2],
                Edges = edges,
                Vertex = intersection,
                Point = circleEventPoint
            };
            leaves[1].CircleEvent = circleEvent;
            return circleEvent;
        }
    }
}
