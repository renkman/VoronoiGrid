using System.Collections.Generic;

namespace VoronoiEngine.Elements
{
    public class HalfEdge : IGeometry
    {
        public HalfEdge(Point point)
        {
            Point = point;
        }

        public Point Point { get; set; }

        public ICollection<Vertex> Vertices { get; set; }
    }
}