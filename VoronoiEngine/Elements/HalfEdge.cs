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
        }

        public Point Point { get; set; }

        public Vertex Start { get; set; }

        public Vertex End { get; set; }

        public Point Left { get; }

        public Point Right { get; }

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
    }
}