using System.Collections.Generic;

namespace VoronoiEngine.Elements
{
    public interface INode
    {
        INode Parent { get; set; }

        bool IsLeaf { get; }

        INode Find(Point point);

        void GetDescendants(Point start, TraverseDirection direction, ICollection<INode> descendants, int count);
    }
}