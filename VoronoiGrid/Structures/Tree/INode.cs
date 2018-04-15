using System.Collections.Generic;
using VoronoiGrid.Geometry;

namespace VoronoiGrid.Structures.Tree
{
    public interface INode
    {
        INode Parent { get; set; }

        bool IsLeaf { get; }

        INode Find(Point point);

        INode GetNeighbor(INode start, TraverseDirection direction);

        void GetDescendants(Point start, TraverseDirection direction, ICollection<INode> descendants, int count);
    }
}