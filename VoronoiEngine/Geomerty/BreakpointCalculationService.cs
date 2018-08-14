using System;
using VoronoiEngine.Elements;

namespace VoronoiEngine.Geomerty
{
    public class BreakpointCalculationService : IBreakpointCalculationService
    {
        public Point CalculateBreakpoint(Point left, Point right, int y)
        {
            if (left.Y == right.Y)
                return new Point { X = (int)(right.X / 2m + left.X / 2m), Y = y };
            //    return new Point { X = (int)(right.X * 0.5m), Y = y };

            if (left.Y == y)
                return new Point { X = left.X, Y = y };

            if (right.Y == y)
                return new Point { X = right.X, Y = y };

            // x = (ay * by - Sqrt(ay * by((ay - by)²+b²))) / (ay - by)
            var x = (int)Math.Round((left.Y * right.X - Math.Sqrt(left.Y * right.Y * (Math.Pow(left.Y - right.Y, 2) + Math.Pow(right.X, 2)))) / (left.Y - right.Y));

            return new Point { X = x, Y = y };
        }
    }
}