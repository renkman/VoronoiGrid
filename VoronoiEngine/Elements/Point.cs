using System;

namespace VoronoiEngine.Elements
{
    public class Point : IComparable<Point>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int CompareTo(Point other)
        {
            return X.CompareTo(other.X);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point
            {
                X = Math.Abs(a.X - b.X),
                Y = Math.Abs(a.Y - b.Y)
            };
        }
    }
}