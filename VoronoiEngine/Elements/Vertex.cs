using System;
using System.Collections.Generic;
using System.Linq;

namespace VoronoiEngine.Elements
{
    public class Vertex : IGeometry
    {
        public Vertex()
        {
            HalfEdges = new HashSet<HalfEdge>();
        }

        public Point Point { get; set; }

        public ICollection<HalfEdge> HalfEdges { get; }

        public void AddHalfEdges(IEnumerable<HalfEdge> edges)
        {
            foreach (var edge in edges)
                HalfEdges.Add(edge);
        }
        
        public override string ToString()
        {
            return $"Vertex at Point: {Point}, HalfEdges: {string.Join(", ", HalfEdges.Select(h => h.ToString()).ToArray())}";
        }
    }
}