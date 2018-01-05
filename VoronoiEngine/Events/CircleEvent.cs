using VoronoiEngine.Elements;

namespace VoronoiEngine.Events
{
    public class CircleEvent : IEvent
    {
        public Point Vertex { get; set; }
        public Point Point { get; set; }
    }
}