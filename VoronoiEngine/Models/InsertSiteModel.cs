using VoronoiEngine.Elements;

namespace VoronoiEngine.Models
{
    public class InsertSiteModel
    {
        public HalfEdge HalfEdge { get; set; }
        public ICollection<Leaf> Leaves { get; set; }
    }
}