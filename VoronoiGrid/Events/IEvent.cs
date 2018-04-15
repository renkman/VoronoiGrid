using VoronoiGrid.Geometry;

namespace VoronoiGrid.Events
{
    public interface IEvent
    {
        Point Point { get; set; }
    }
}
