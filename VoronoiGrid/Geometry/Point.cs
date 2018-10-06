using System;

namespace VoronoiGrid.Geometry
{
    public class Point : IComparable<Point>, IEquatable<Point>
    {
        public Point()
        {
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }

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
            return (int)((X * Y + X) * 100);
        }

        public bool Equals(Point other)
        {
            if (other == null)
                return false;
            return X == other.X && Y == other.Y;
        }

        public override string ToString()
        {
            return $"{nameof(Point)}: X: {X:#.00}, Y: {Y:#.00}";
        }
    }
}