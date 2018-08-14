using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoronoiEngine.Elements
{
    public class Cell : IGeometry
    {
        public Cell(Point point)
        {
            Point = point;
            Edges = new List<HalfEdge>();
            Vertices = new List<Vertex>();
        }

        public Point Point { get; set; }
        
        public ICollection<HalfEdge> Edges { get; }

        public ICollection<Vertex> Vertices { get; }
    }
}
