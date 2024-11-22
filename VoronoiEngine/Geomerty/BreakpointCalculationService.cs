﻿using VoronoiEngine.Elements;

namespace VoronoiEngine.Geomerty
{
    public class BreakpointCalculationService : IBreakpointCalculationService
    {
        public Point CalculateBreakpoint(Point left, Point right, double y)
        {
            if (left.Y == right.Y)
                return new Point(right.X / 2d + left.X / 2d, y);
            //    return new Point { X = (int)(right.X * 0.5m), Y = y };

            if (left.Y == y)
                return new Point(left.X, y);

            if (right.Y == y)
                return new Point(right.X, y);

            // x = (ay * by - Sqrt(ay * by((ay - by)²+b²))) / (ay - by)
            //var x = (int)Math.Round((left.Y * right.X - Math.Sqrt(left.Y * right.Y * (Math.Pow(left.Y - right.Y, 2) + Math.Pow(right.X, 2)))) / (left.Y - right.Y));

            var dp = 2.0 * (left.Y - y);
            var a1 = 1.0 / dp;
            var b1 = -2.0 * left.X / dp;
            var c1 = y + dp / 4 + left.X * left.X / dp;
			
            dp = 2.0 * (right.Y - y);
            var a2 = 1.0 / dp;
            var b2 = -2.0 * right.X/dp;
            var c2 = y + dp / 4 + right.X * right.X / dp;
			
            var a = a1 - a2;
            var b = b1 - b2;
            var c = c1 - c2;
			
            var disc = b*b - 4 * a * c;
            var x1 = (-b + Math.Sqrt(disc)) / (2*a);
            var x2 = (-b - Math.Sqrt(disc)) / (2*a);

            var x = left.Y < right.Y ? Math.Max(x1, x2) : Math.Min(x1, x2);

            return new Point(x, y);
        }
        
        public double GetY(Point p, Point newP)
        {
            double dp = 2 * (p.Y - newP.Y);
            double a1 = 1 / dp;
            double b1 = -2 * p.X / dp;
            double c1 = newP.Y + dp / 4 + p.X * p.X / dp;

            return a1 * newP.X * newP.X + b1 * newP.X + c1;
        }
    }
}