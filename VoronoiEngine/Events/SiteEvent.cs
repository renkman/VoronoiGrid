using VoronoiEngine.Elements;

namespace VoronoiEngine.Events
{
    public class SiteEvent : IEvent
    {
        public Point Point { get; set; }
    }
}