using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;

namespace VoronoiEngine.Geomerty
{
    public class CircleEventCalculationService : ICircleEventCalculationService
    {
        public CircleEvent DetermineCircleEvent(Leaf arc, double y)
        {
            var leftParent = arc.GetParent(TraverseDirection.CounterClockwise);
            var rightParent = arc.GetParent(TraverseDirection.Clockwise);
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
            //var c = right.Site;

            var x = a.X - intersection.X;
            var dy = a.Y - intersection.Y;

            var d = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(dy, 2));

            var circleEventPoint = new Point(intersection.X, intersection.Y - d);

            if (circleEventPoint.Y >= y)
                return null;

            var circleEvent = new CircleEvent
            {
                Arc = arc,
                Edges = new List<HalfEdge> { leftEdge, rightEdge },
                Vertex = intersection,
                Point = circleEventPoint
            };

            //if (arc.CircleEvent != null)
            //    throw new InvalidOperationException($"Circle event for leaf{arc.Site} already exists");

            arc.CircleEvent = circleEvent;
            return circleEvent;
        }
    }
}