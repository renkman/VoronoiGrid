namespace VoronoiEngine.Elements
{
    public class Node : INode
    {
        public INode Left { get; set; }
        public INode Right { get; set; }
        public Point Point { get; set; }
    }
}