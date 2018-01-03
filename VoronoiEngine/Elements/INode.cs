namespace VoronoiEngine.Elements
{
    public interface INode
    {
        bool IsLeaf { get; }

        INode Find(Point point);
    }
}