using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;

namespace VoronoiEngine.Geomerty
{
    public class CircleEventEdgeCalculationService : ICircleEventCalculationService
    {
        public CircleEvent DetermineCircleEvent(ICollection<INode> arcs, double y)
        {
            var leaves = arcs.Cast<Leaf>().Select(l => l).ToList();
            //var sites = leaves.Select(l => l.Site).Distinct().ToList(); //.OrderBy(s => s.X).Distinct().ToList();

            //if (sites.Count != 3)
            //    return null;

            //var edges = arcs.Select(a => a.Parent).Cast<Node>().Select(p => p.HalfEdge).Distinct().ToList();
            //if (edges.Count != 2)
            //    throw new InvalidOperationException($"Corrupt tree structure with {edges.Count} parent edges.");

            var leftParent = leaves[0].GetParent(TraverseDirection.CounterClockwise);
            var rightParent = leaves[0].GetParent(TraverseDirection.Clockwise);
            var left = leftParent?.GetLeaf(TraverseDirection.CounterClockwise);
            var right = rightParent?.GetLeaf(TraverseDirection.Clockwise);
            
            if (left == null || right == null || left == right)
                return null;

            var leftEdge = leftParent.HalfEdge;
            var rightEdge = rightParent.HalfEdge;

            // Check conversion by edge intersection
            var intersection = leftEdge.Intersect(rightEdge);
            if (intersection == null)
                return null;

            var a = left.Site;
            var c = right.Site;

            var x = a.X - intersection.X;
            var dy = a.Y - intersection.Y;

            var d = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(dy, 2));

            var circleEventPoint = new Point(intersection.X, intersection.Y - d);

            if (circleEventPoint.Y >= y)
                return null;

            var circleEvent = new CircleEvent
            {
                LeftArc = left,
                CenterArc = leaves[0],
                RightArc = right,
                Edges = new List<HalfEdge> { leftEdge, rightEdge },
                Vertex = intersection,
                Point = circleEventPoint
            };
            leaves[0].CircleEvent = circleEvent;
            return circleEvent;
        }
    }
}