using VoronoiEngine.Elements;

namespace VoronoiEngine.Events
{
    public class CircleEvent : IEvent
    {
        public Point Vertex { get; set; }
        public Point Point { get; set; }
        public Leaf LeftArc { get; set; }
        public Leaf CenterArc { get; set; }
        public Leaf RightArc { get; set; }
        public Node Parent { get; set; }
    }
}