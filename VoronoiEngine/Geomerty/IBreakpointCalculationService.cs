using VoronoiEngine.Elements;

namespace VoronoiEngine.Geomerty
{
    public interface IBreakpointCalculationService
    {
        Point CalculateBreakpoint(Point lefta, Point right, int y);
    }
}