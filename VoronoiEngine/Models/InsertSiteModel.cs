using System.Collections.Generic;
using VoronoiEngine.Elements;

namespace VoronoiEngine.Models
{
    public class InsertSiteModel
    {
        public ICollection<HalfEdge> HalfEdges { get; set; }
        public ICollection<Leaf> Leaves { get; set; }
    }
}