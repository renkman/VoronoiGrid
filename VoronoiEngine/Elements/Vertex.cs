using System.Collections.Generic;

namespace VoronoiEngine.Elements
{
    public class Vertex : IGeometry
    {
        public Vertex()
        {
            Edges = new List<HalfEdge>();
        }

        public Point Point { get; set; }

        public ICollection<HalfEdge> Edges {get;}
    }
}