using VoronoiEngine.Models;

namespace VoronoiEngine.Elements
{
    public interface INode
    {
        INode Parent { get; set; }

        bool IsLeaf { get; }

        INode Find(Point point);
        
        InsertSiteModel Insert(Point site);
    }
}