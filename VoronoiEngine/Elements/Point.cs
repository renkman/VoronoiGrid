namespace VoronoiEngine.Elements
{
    public class Point : IComparable<Point>, IEquatable<Point>
    {
        public Point()
        {
        }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public int XInt => (int)Math.Round(X);
        public int YInt => (int)Math.Round(Y);

        public int CompareTo(Point other)
        {
            if (Equals(X, other.X))
                return 0;
            if (X < other.X)
                return -1;
            return 1;
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
            return XInt ^ YInt + YInt;
        }

        public bool Equals(Point other)
        {
            if (other == null)
                return false;
            return Equals(X, other.X) && Equals(Y, other.Y);
        }

        public double CalculateDistance(Point other)
        {
            return Math.Sqrt((other.X - X) * (other.X - X) + (other.Y - Y) * (other.Y - Y));
        }

        public override string ToString()
        {
            return $"{nameof(Point)}: X: {X}, Y: {Y}";
        }

        private static bool Equals(double a, double b)
        {
            return Math.Abs(a - b) < 0.0001;
        }
    }
}