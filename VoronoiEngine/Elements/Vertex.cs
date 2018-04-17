using System.Collections.Generic;

namespace VoronoiEngine.Elements
{
    public class Vertex : IGeometry
    {
        public Vertex()
        {
            HalfEdges = new HashSet<HalfEdge>();
        }

        public Point Point { get; set; }

        public ICollection<HalfEdge> HalfEdges {get;}
    }
}