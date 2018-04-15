using System;

namespace VoronoiGrid.Geometry
{
    public class Point : IComparable<Point>, IEquatable<Point>
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

        public override bool Equals(object obj)
        {
            var point = obj as Point;
            if (point == null)
                return false;
            return X == point.X && Y == point.Y;
        }

        public override int GetHashCode()
        {
            return X ^ Y + Y;
        }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}