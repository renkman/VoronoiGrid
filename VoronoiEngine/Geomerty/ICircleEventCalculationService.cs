using VoronoiEngine.Elements;
using VoronoiEngine.Events;

namespace VoronoiEngine.Geomerty
{
    public interface ICircleEventCalculationService
    {
        CircleEvent DetermineCircleEvent(Leaf arc, double y);
    }
}