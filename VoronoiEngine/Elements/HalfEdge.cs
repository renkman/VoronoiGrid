namespace VoronoiEngine.Elements
{
    public class HalfEdge : IGeometry
    {
        public HalfEdge(Point point)
        {
            Point = point;
        }

        public Point Point { get; set; }

        public Vertex Start { get; set; }

        public Vertex End { get; set; }
    }
}