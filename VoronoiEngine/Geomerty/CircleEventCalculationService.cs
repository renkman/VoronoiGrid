using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;

namespace VoronoiEngine.Geomerty
{
    public class CircleEventCalculationService : ICircleEventCalculationService
    {
        public CircleEvent DetermineCircleEvent(ICollection<INode> arcs)
        {
            var leaves = arcs.Cast<Leaf>().Select(l => l).ToList();
            var sites = leaves.Select(l => l.Site).Distinct().ToList(); //.OrderBy(s => s.X).Distinct().ToList();

            if (sites.Count != 3)
                return null;

            // Check if the breakpoints of the three consecutive arcs converge
            if (!CheckConversion(sites[0], sites[1], sites[2]))
                return null;

            var circumcenter = CalculateCircumcenter(sites[0], sites[1], sites[2]);
            if (circumcenter == null)
                return null;

            var circleEventPoint = CalculateCircle(circumcenter, sites[0]);
            var circleEvent = new CircleEvent
            {
                Point = circleEventPoint,
                Vertex = circumcenter,
                LeftArc = leaves[0],
                CenterArc = leaves[1],
                RightArc = leaves[2],
                Edges = leaves.Select(l => l.Parent).Cast<Node>().Select(n => n.HalfEdge).Distinct().ToList()
            };
            leaves[1].CircleEvent = circleEvent;
            return circleEvent;
        }

        private static Point CalculateCircumcenter(Point a, Point b, Point c)
        {
            var midpointABX = (a.X + b.X) / 2d;
            var midpointABY = (a.Y + b.Y) / 2d;
            var slopeAB = b.Y != a.Y ? -1d / ((b.Y - a.Y) / (double)(b.X - a.X)) : 0;

            var midpointACX = (a.X + c.X) / 2d;
            var midpointACY = (a.Y + c.Y) / 2d;
            var slopeAC = c.Y != a.Y ? -1d / ((c.Y - a.Y) / (double)(c.X - a.X)) : 0;

            var ab = midpointABY - slopeAB * midpointABX;
            var ac = midpointACY - slopeAC * midpointACX;

            var slope = -slopeAC + slopeAB;
            var ad = ab + -ac;
            var x = ad / slope;

            var circumcenter = new Point();
            circumcenter.PrecisionX = -x;
            circumcenter.PrecisionY = slopeAC * circumcenter.PrecisionX + ac;
            circumcenter.X = (int)Math.Round(-x);
            circumcenter.Y = (int)Math.Round(slopeAC * circumcenter.X + ac);
            return circumcenter;
        }
        
        private static Point CalculateCircle(Point circumcenter, Point point)
        {
            var a = point.Y - circumcenter.PrecisionY;
            var b = point.X - circumcenter.PrecisionX;
            var radius = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
            return new Point {
                X = circumcenter.X,
                PrecisionX = circumcenter.PrecisionX,
                Y = (int)Math.Round(circumcenter.PrecisionY - radius),
                PrecisionY = Math.Round(circumcenter.PrecisionY - radius)
            };
        }

        private static bool CheckConversion(Point a, Point b, Point c)
        {
            //  (x2 − x1)(y2 + y1) + (x3 − x2)(y3 + y2) < 0 -> Counter clockwise
            return (b.X - a.X) * (b.Y + a.Y) + (c.X - b.X) * (c.Y + b.Y) < 0;
            //return ((b.X - a.X) * (c.Y - a.Y) - (c.X - a.X) * (b.Y - a.Y) > 0);
        }
    }
}