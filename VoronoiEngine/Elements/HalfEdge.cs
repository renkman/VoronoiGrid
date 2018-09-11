using System;

namespace VoronoiEngine.Elements
{
    public class HalfEdge : IGeometry, IEquatable<HalfEdge>
    {
        public HalfEdge(Point point, Point left, Point right)
        {
            Point = point;
            Left = left;
            Right = right;

            F = (Right.X - Left.X) / (Left.Y - Right.Y);
            G = Point.Y - F * Point.X;
            Direction = new Point(Right.Y - Left.Y, -(Right.X - Left.X));
        }

        public Point Point { get; set; }

        public Vertex Start { get; set; }

        public Vertex End { get; set; }

        public Point Left { get; }

        public Point Right { get; }

        public Point Direction { get; }

        public double F { get; }

        public double G { get; }

        public override int GetHashCode()
        {
            return Left.GetHashCode() + Right.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as HalfEdge;
            if (other == null)
                return false;
            return Equals(other);
        }

        public bool Equals(HalfEdge other)
        {
            if (other == null)
                return false;
            return Left.Equals(other.Left) && Right.Equals(other.Right);
        }

        //public static bool operator ==(HalfEdge a, HalfEdge b)
        //{
        //    if (a == null || b == null)
        //        return false;
        //    return a.Equals(b);
        //}

        //public static bool operator !=(HalfEdge a, HalfEdge b)
        //{
        //    if (a == null || b == null)
        //        return false;
        //    return !a.Equals(b);
        //}

        public override string ToString()
        {
            return $"HalfEdge at Point: {Point}, Left: {Left}, Right: {Right}";
        }
                
        public Point Intersect(HalfEdge halfEdge)
        {
            double x = (halfEdge.G - G) / (F - halfEdge.F);
            double y = F * x + G;

            if ((x - Point.X) / Direction.X < 0)
                return null;
            if ((y - Point.Y) / Direction.Y < 0)
                return null;

            if ((x - halfEdge.Point.X) / halfEdge.Direction.X < 0)
                return null;
            if ((y - halfEdge.Point.Y) / halfEdge.Direction.Y < 0)
                return null;

            var p = new Point((int)x, (int)y);
            return p;
        }
    }
}