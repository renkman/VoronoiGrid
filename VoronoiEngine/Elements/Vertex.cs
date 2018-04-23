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

        public void Replace(IEnumerable<HalfEdge> halfEdges)
        {
            foreach (var halfEdge in halfEdges)
            {
                var replacement = HalfEdges.SingleOrDefault(h => h == halfEdge);
                if (replacement == null)
                    continue;

                var start = replacement.Start;
                var end = replacement.End;

                if (start != null && halfEdge.End == null)
                    throw new InvalidOperationException("Only HalfEdges with End set must replace HalfEdges with Start set.");

                if (end != null && halfEdge.Start == null)
                    throw new InvalidOperationException("Only HalfEdges with Start set must replace HalfEdges with End set.");

                HalfEdges.Remove(replacement);
                HalfEdges.Add(halfEdge);

                if (start != null)
                    halfEdge.Start = start;

                if (end != null)
                    halfEdge.End = end;
            }
        }
    }
}