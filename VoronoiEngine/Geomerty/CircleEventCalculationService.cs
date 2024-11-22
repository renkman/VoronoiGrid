﻿using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Utilities;

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
            var d = a.CalculateDistance(intersection);
            var pointY = intersection.Y - d;

            if (pointY >= y)
                return null;

            var circleEventPoint = new Point(intersection.X, pointY);

            var circleEvent = new CircleEvent
            {
                Arc = arc,
                Edges = new List<HalfEdge> { leftEdge, rightEdge },
                Point = circleEventPoint
            };
            
            arc.CircleEvent = circleEvent;
            Logger.Instance.Log($"Circle event found A: {left}, B: {arc}, C: {right}");
            return circleEvent;
        }
    }
}