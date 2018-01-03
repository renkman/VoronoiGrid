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
    }
}