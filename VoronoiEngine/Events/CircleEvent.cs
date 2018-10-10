using System.Collections.Generic;
using VoronoiEngine.Elements;

namespace VoronoiEngine.Events
{
    public class CircleEvent : IEvent
    {
        public Point Point { get; set; }
        
        public Leaf Arc { get; set; }
        
        public ICollection<HalfEdge> Edges { get; set; }

        public double PrecisionVertex { get; set; }
    }
}