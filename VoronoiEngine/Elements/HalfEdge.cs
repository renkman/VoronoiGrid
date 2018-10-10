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

        public Point EndPoint { get; set; }

        public Vertex Start { get; set; }

        public Vertex End { get; set; }      

        public Point Left { get; }

        public Point Right { get; }

        public Point Direction { get; }

        public double F { get; }

        public double G { get; }

        public HalfEdge Neighbor { get; set; }

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
        
        public override string ToString()
        {
            return $"HalfEdge at Point: {Point}, EndPoint: {EndPoint}, Left: {Left}, Right: {Right}";
        }
                
        public Point Intersect(HalfEdge halfEdge)
        {
            var x = (halfEdge.G - G) / (F - halfEdge.F);

            if (double.IsNaN(x))
                return null;

            var y = F * x + G;

            if ((x - Point.X) / Direction.X < 0)
                return null;
            if ((y - Point.Y) / Direction.Y < 0)
                return null;

            if ((x - halfEdge.Point.X) / halfEdge.Direction.X < 0)
                return null;
            if ((y - halfEdge.Point.Y) / halfEdge.Direction.Y < 0)
                return null;

            var p = new Point(x, y);
            return p;
        }
    }
}